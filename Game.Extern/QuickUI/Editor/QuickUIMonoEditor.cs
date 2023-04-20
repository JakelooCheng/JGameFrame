using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


namespace Game.Extern.QuickUI.Editor
{
    /// <summary>
    /// QuickUIMono UI 快速绑定。Editor 面板
    /// </summary>
    [CustomEditor(typeof(QuickUIMono))]
    public class QuickUIMonoEditor : UnityEditor.Editor
    {
        private void Awake()
        {
            if (!Application.isPlaying)
            {
                CheckInit();
            }
        }

        private void CheckInit()
        {
            var comp = target as QuickUIMono;
            if (comp.PrefabInfo == null)
            {
                return;
            }

            // 校验数据
            comp.CheckRemove();

            // 组装 HashCode
            if (comp.PrefabInfo.ComponentScriptObject != null && comp.PrefabInfo.PrefabType != UIPrefabInfo.UIPrefabType.Item)
            {
                comp.PrefabInfo.IsCreateByQuickUI = QuickUIHelper.CheckHashCode(comp.PrefabInfo.ComponentScriptPath, out int hashCode);
                comp.PrefabInfo.ScriptHashCode = hashCode;
            }

            // 规范化命名
            if (comp.gameObject.name != comp.PrefabInfo.PrefabName 
                && comp.PrefabInfo.ComponentScriptObject == null 
                && comp.PrefabInfo.PrefabType != UIPrefabInfo.UIPrefabType.Item)
            {
                Debug.LogWarning($"QuickUI Warning 将预制体重命名为 {comp.PrefabInfo.PrefabName}");
                comp.gameObject.name = comp.PrefabInfo.PrefabName;
            }
        }

        #region InspectorGUI
        public override void OnInspectorGUI()
        {
            var comp = target as QuickUIMono;
            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox($"游戏运行状态中不可编辑！", MessageType.Info);
                return;
            }
            if (comp.PrefabInfo == null)
            {
                return;
            }
            DrawAddBox(comp);
            DrawObjects(comp);
            DrawPrefabInfo(comp);
        }

        /// <summary>
        /// 绘制拖拽添加窗
        /// </summary>
        private void DrawAddBox(QuickUIMono comp)
        {
            var dragArea = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUI.Box(dragArea, "拖拽添加区域");
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (Event.current.type == EventType.DragPerform)
            {
                if (DragAndDrop.objectReferences.Length > 0)
                {
                    var obj = DragAndDrop.objectReferences[0] as GameObject;
                    if (obj != null)
                    {
                        var targetComp = obj.GetComponentInParent<QuickUIMono>();
                        if (targetComp == (target as QuickUIMono))
                        {
                            SelectTypeAndAdd(obj);
                        }
                        else
                        {
                            Debug.LogError("QuickUI Error 此 GameObject 向上并不是此组件。");
                        }
                    }
                    else
                    {
                        Debug.LogError("QuickUI Error 不是 GameObject。");
                    }
                }
                DragAndDrop.AcceptDrag();
            }
        }

        /// <summary>
        /// 绘制已经加进来的变量
        /// </summary>
        private void DrawObjects(QuickUIMono comp)
        {
            EditorGUILayout.BeginVertical("frameBox");
            if (comp.UIObjectsArray.Count != comp.UIObjectsName.Count)
            {
                EditorGUILayout.HelpBox($"严重错误：变量和 Objects 的数量不匹配！", MessageType.Error);
                if (GUILayout.Button("重置修复"))
                    comp.ClearAllCache();
            }
            else
            {
                GUILayout.Label(comp.UIObjectsArray.Count > 0 ? "已绑定组件：" : "暂无绑定组件");
                for (int index = 0; index < comp.UIObjectsArray.Count; index++)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.DelayedTextField(comp.UIObjectsName[index]);
                    EditorGUILayout.ObjectField(comp.UIObjectsArray[index], comp.UIObjectsArray[index].GetType(), false);
                    if (GUILayout.Button("移除"))
                    {
                        comp.AddOrRemove(comp.UIObjectsArray[index], "");
                    }
                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space(2);
                }
                if (GUILayout.Button("快速绑定"))
                {
                    QuickUIHelper.QuickBound(comp);
                }
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 展示预制体信息
        /// </summary>
        private void DrawPrefabInfo(QuickUIMono comp)
        {
            EditorGUILayout.BeginVertical("frameBox");
            if (!comp.PrefabInfo.IsCreateByQuickUI)
            {
                EditorGUILayout.HelpBox($"已手动创建过UI脚本，且不符合自动生成格式。无法自动生成！", MessageType.Warning);
                EditorGUILayout.EndVertical();
                return;
            }

            // 绘制脚本信息
            if (comp.PrefabInfo.ComponentScriptObject != null)
            {
                GUILayout.Label("已绑定脚本：");
                EditorGUILayout.ObjectField(comp.PrefabInfo.NormalScriptObject, comp.PrefabInfo.NormalScriptObject.GetType(), false);
                if (comp.PrefabInfo.ComponentScriptObject != null)
                {
                    EditorGUILayout.ObjectField(comp.PrefabInfo.ComponentScriptObject, comp.PrefabInfo.ComponentScriptObject.GetType(), false);
                }
                if (comp.PrefabInfo.ScriptHashCode != comp.GetHashCode())
                {
                    EditorGUILayout.HelpBox($"需要同步变量！", MessageType.Info);
                }

                if (GUILayout.Button("同步变量到脚本"))
                {
                    QuickUIHelper.AddToFile(comp.PrefabInfo, comp);
                }
            }
            else
            {
                GUILayout.Label("暂无绑定脚本");
                if (GUILayout.Button("创建脚本"))
                {
                    QuickUIHelper.CreateUIScript(comp.PrefabInfo, comp.GetHashCode());
                }
            }

            if (comp.PrefabInfo.PrefabType == UIPrefabInfo.UIPrefabType.Item && comp.UIObjectsArray.Count > 0)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("【变量代码】到剪贴板"))
                {
                    GUIUtility.systemCopyBuffer = QuickUIHelper.GetVariables(comp, false);
                }
                GUILayout.Space(2);
                if (GUILayout.Button("【赋值代码】到剪贴板"))
                {
                    GUIUtility.systemCopyBuffer = QuickUIHelper.GetAssignment(comp, false);
                }
            }

            // 判断预制体是否有更改
            if (UIPrefabInfo.IsDirtyPrefab(comp.gameObject))
            {
                EditorGUILayout.HelpBox($"建议保存/覆盖预制体后，再进行代码生成！", MessageType.Warning);
            }
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Static
        [UnityEditor.InitializeOnLoadMethod]
        private static void OnLoad()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyItemGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;

            UnityEditor.SceneManagement.PrefabStage.prefabSaved -= OnPrefabUpdated;
            UnityEditor.SceneManagement.PrefabStage.prefabSaved += OnPrefabUpdated;
            PrefabUtility.prefabInstanceUpdated -= OnPrefabUpdated;
            PrefabUtility.prefabInstanceUpdated += OnPrefabUpdated;
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

            if (target == null || target.GetComponent<RectTransform>() == null)
            {
                return;
            }

            // Hierarchy 面板上显示标记
            int count = 0;
            foreach (var elementComp in QuickUIMono.ActiveUIComponents)
            {
                count += elementComp.GetBoundGameObjectCount(target);
            }
            if (count > 0)
            {
                DrawHierarchyMark(new Rect(selectionRect), count);
            }

            // 快捷添加
            if (Event.current.button == 2 && Event.current.type == EventType.MouseDown)
            {
                if (selectionRect.Contains(Event.current.mousePosition))
                {
                    SelectTypeAndAdd(target);
                }
            }
        }

        /// <summary>
        /// 监听 Prefab 保存事件
        /// </summary>
        /// <param name="target"></param>
        private static void OnPrefabUpdated(GameObject target)
        {
            if (Application.isPlaying)
            {
                return;
            }

            var comp = target.GetComponent<QuickUIMono>();
            if (comp == null)
            {
                return;
            }

            // 预制体保存的时候执行一次快速绑定
            QuickUIHelper.QuickBound(comp);

            if (comp.UIObjectsArray.Count == 0)
            {
                return;
            }

            if (comp.PrefabInfo.ComponentScriptObject == null && comp.PrefabInfo.PrefabType != UIPrefabInfo.UIPrefabType.Item)
            {
                Debug.LogError($"QuickUI Error {target.name} 已绑定变量但未创建脚本！");
                return;
            }

            // 不需要保存
            QuickUIHelper.CheckHashCode(comp.PrefabInfo.ComponentScriptPath, out int hashCode);
            if (comp.GetHashCode() == hashCode)
            {
                return;
            }
            if (EditorUtility.DisplayDialog($"提示 {target.name}", "绑定的 UI 发生变化，是否立刻同步变量到脚本！", "生成", "稍后")) //显示对话框
            {
                QuickUIHelper.AddToFile(comp.PrefabInfo, comp);
            }
        }

        /// <summary>
        /// 在弹出菜单中选择类型，然后添加到最上层的 UIElement 中
        /// </summary>
        public static void SelectTypeAndAdd(GameObject target)
        {
            EditorGUIUtility.PingObject(target);

            // 判断父类有无 UI 组件
            var targetComp = target.GetComponentInParent<QuickUIMono>();
            if (targetComp == null)
            {
                Debug.LogError("QuickUI Error 此 GameObject 向上未找到相关组件，请先添加 QuickUIMono");
                return;
            }

            // 构建选择菜单
            var objectInfo = new GameObjectInfo(target);
            var addMenu = new GenericMenu();
            for (int index = 0; index < objectInfo.Objects.Length; index++)
            {
                var obj = objectInfo.Objects[index];
                var name = objectInfo.ObjectNames[index];
                bool hasObject = targetComp.HasObject(obj);

                // 要跳转到父节点的
                if (name.Contains("（到父节点）"))
                {
                    var parentComp = target.transform.parent?.GetComponentInParent<QuickUIMono>();
                    if (parentComp != null)
                    {
                        addMenu.AddItem(new GUIContent(objectInfo.ObjectNames[index] + (parentComp.HasObject(obj) ? "【已绑定】" : "")), false, () =>
                        {
                            parentComp.AddOrRemove(obj, QuickUIHelper.MakeName(obj.name, obj.GetType()));
                        });
                    }
                }
                else
                {
                    addMenu.AddItem(new GUIContent(objectInfo.ObjectNames[index] + (hasObject ? "【已绑定】" : "")), false, () =>
                    {
                        targetComp.AddOrRemove(obj, QuickUIHelper.MakeName(obj.name, obj.GetType()));
                    });
                }
            }
            addMenu.ShowAsContext();
        }

        private static void DrawHierarchyMark(Rect markRect, int count)
        {
            markRect.x = 34;
            markRect.width = 80;
            GUI.color = Color.yellow;
            GUI.Label(markRect, QuickUIHelper.MarkIconList[count > 10 ? 9 : count - 1]);
            GUI.color = Color.white;
        }
        #endregion
    }
}