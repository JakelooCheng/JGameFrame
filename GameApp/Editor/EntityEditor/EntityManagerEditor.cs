using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameApp.Entitys;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using TableData;

namespace GameApp.Editor
{
    [CustomEditor(typeof(EntityManager), true)]
    public class EntityManagerEditor : OdinEditor
    {
        private SerializedProperty startEntitysProperty;

        protected void Awake()
        {
            startEntitysProperty = serializedObject.FindProperty("StartEntitys");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            MergeGameObject(true);
        }

        private void MergeGameObject(bool force = false)
        {
            var entityManager = target as EntityManager;
            // 数量没变化时不合并
            if (!force)
            {
                if (entityManager.transform.childCount == entityManager.StartEntitys.Count)
                {
                    return;
                }
            }
            // 游戏运行中不合并
            if (Application.isPlaying)
            {
                return;
            }

            // 获取所有孩子
            HashSet<GameObject> children = new HashSet<GameObject>();
            GetAllChildren(entityManager.transform, children);
            // 先移除不存在的
            for (int index = 0; index < entityManager.StartEntitys.Count; index ++)
            {
                if (!children.Contains(entityManager.StartEntitys[index].GameObject))
                {
                    entityManager.StartEntitys.RemoveAt(index--);
                }
                else
                {
                    children.Remove(entityManager.StartEntitys[index].GameObject);
                }
            }
            // 再添加刚存在的
            foreach (var child in children)
            {
                entityManager.StartEntitys.Add(new MapEntityConfig(child));
            }
        }

        /// <summary>
        /// 递归组装所有 Entity
        /// </summary>
        private void GetAllChildren(Transform root, HashSet<GameObject> children)
        {
            for (int index = 0; index < root.childCount; index++)
            {
                var curGo = root.GetChild(index).gameObject;
                children.Add(curGo);
                if (curGo.name.IndexOf("Root") == 0)
                {
                    GetAllChildren(curGo.transform, children);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            //EditorGUILayout.PropertyField(startEntitysProperty);
            //serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();

            MergeGameObject();
        }

        #region Static
        [UnityEditor.InitializeOnLoadMethod]
        private static void OnLoad()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyItemGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
        }

        /// <summary>
        /// 在 Hierarchy 面板中显示标记，和快捷添加操作
        /// </summary>
        private static void OnHierarchyItemGUI(int instanceID, Rect selectionRect)
        {
            if (Application.isPlaying)
            {
                return;
            }

            var target = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (target == null)
            {
                return;
            }

            // Hierarchy 面板上显示标记
            var manager = target.GetComponentInParent<EntityManager>();
            if (!manager)
            {
                return;
            }

            MapEntityConfig curConfig = null;
            foreach (var entity in manager.StartEntitys)
            {
                if (entity.GameObject == target)
                {
                    DrawHierarchyMark(new Rect(selectionRect), entity.EntityType);
                    curConfig = entity;
                    break;
                }
            }

            // 快捷添加
            if (Event.current.button == 2 && Event.current.type == EventType.MouseDown && curConfig != null)
            {
                if (selectionRect.Contains(Event.current.mousePosition))
                {
                    SelectTypeAndAdd(manager, curConfig);
                }
            }
        }

        /// <summary>
        /// 在弹出菜单中选择类型，然后添加到最上层的 UIElement 中
        /// </summary>
        public static void SelectTypeAndAdd(EntityManager manager, MapEntityConfig config)
        {
            EditorGUIUtility.PingObject(config.GameObject);

            // 构建选择菜单
            var addMenu = new GenericMenu();
            for (int index = 0; index < (int)EntityType.Count; index++)
            {
                int cur = index;
                addMenu.AddItem(new GUIContent(((EntityType)cur).ToString()), false, () =>
                {
                    config.MapEntityType = (EntityType)cur;
                });
            }
            addMenu.ShowAsContext();
        }

        private static void DrawHierarchyMark(Rect markRect, EntityType type)
        {
            markRect.x = 34;
            markRect.width = 80;
            GUI.color = Color.yellow;
            GUI.Label(markRect, type.ToString().Substring(0, 2));
            GUI.color = Color.white;
        }
        #endregion
    }
}