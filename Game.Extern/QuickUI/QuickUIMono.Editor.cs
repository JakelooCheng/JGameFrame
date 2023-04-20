#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Extern.QuickUI
{
    /// <summary>
    /// 此文件下编写 QuickUIMono 在 Editor 下所用逻辑
    /// </summary>
    public partial class QuickUIMono
    {
        public UIPrefabInfo PrefabInfo;

        /// <summary>
        /// 所有已绑定物件的变量命名数组
        /// </summary>
        [SerializeField]
        public List<string> UIObjectsName = new List<string>();

        /// <summary>
        /// 显示在 Hierarchy 面板中的标记
        /// </summary>
        private Dictionary<GameObject, int> boundGameObjectsDic = new Dictionary<GameObject, int>();

        /// <summary>
        /// 编辑器中活跃的 QuickUIMono
        /// </summary>
        public static readonly HashSet<QuickUIMono> ActiveUIComponents = new HashSet<QuickUIMono>();

        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (PrefabInfo == null)
            {
                PrefabInfo = UIPrefabInfo.Make(this.gameObject);
                if (PrefabInfo == null)
                {
                    DestroyImmediate(this);
                    return;
                }
                RebuildParent();
            }
            ActiveUIComponents.Add(this);
            CheckRemove();
        }

        private void OnDisable()
        {
            if (Application.isPlaying)
            {
                return;
            }

            ActiveUIComponents.Remove(this);
        }

        /// <summary>
        /// 添加时，把父组件绑定重合的过继给子组件
        /// </summary>
        private void RebuildParent()
        {
            if (transform.parent == null)
            {
                return;
            }

            var parent = transform.parent.GetComponentInParent<QuickUIMono>();
            if (parent != null)
            {
                for (int index = parent.UIObjectsArray.Count - 1; index >= 0; index--)
                {
                    var from = parent.UIObjectsArray[index];
                    GameObject go = GetGameObject(from);
                    if (go == null)
                    {
                        continue;
                    }
                    // 父指向子的不过继
                    if (from is RectTransform && go.GetComponent<QuickUIMono>())
                    {
                        continue;
                    }
                    var fromParent = go.GetComponentInParent<QuickUIMono>();
                    if (fromParent == this)
                    {
                        AddOrRemove(from, parent.UIObjectsName[index]);
                        parent.AddOrRemove(from, parent.UIObjectsName[index]);
                    }
                }
            }
        }

        /// <summary>
        /// Enable 时初始化缓存
        /// </summary>
        private void InitObjects()
        {
            boundGameObjectsDic.Clear();
            foreach (var uiObj in UIObjectsArray)
            {
                AddToBoundGameObjectDic(uiObj, 1);
            }
        }

        /// <summary>
        /// 缓存绑定的 Object 的 GameObject
        /// </summary>
        /// <param name="target">要添加的模板</param>
        /// <param name="addValue">为负时是从字典里减去值</param>
        private bool AddToBoundGameObjectDic(UnityEngine.Object target, int addValue)
        {
            GameObject needAdd = GetGameObject(target);
            if (target == null)
            {
                return false;
            }

            if (!boundGameObjectsDic.TryGetValue(needAdd, out int total))
            {
                boundGameObjectsDic.Add(needAdd, 0);
            }
            total += addValue;
            boundGameObjectsDic[needAdd] = total;

            // 没有绑定组件就移除
            if (total <= 0)
                boundGameObjectsDic.Remove(needAdd);

            return true;
        }

        /// <summary>
        /// 获取某 GameObject 已被绑定的组件数
        /// </summary>
        /// <param name="from">任意 GameObject</param>
        /// <returns>绑定组件数</returns>
        public int GetBoundGameObjectCount(GameObject from)
        {
            if (!boundGameObjectsDic.TryGetValue(from, out int total))
            {
                total = 0;
            }
            return total;
        }

        /// <summary>
        /// 添加或移除绑定（如果已有就移除）
        /// </summary>
        /// <param name="from">要添加的组件</param>
        /// <param name="name">变量名（重名进行重命名处理）</param>
        public void AddOrRemove(UnityEngine.Object from, string name)
        {
            EditorUtility.SetDirty(this);
            // 添加到缓存
            int index = UIObjectsArray.IndexOf(from);
            if (index >= 0)
            {
                UIObjectsArray.RemoveAt(index);
                UIObjectsName.RemoveAt(index);
                AddToBoundGameObjectDic(from, -1);
            }
            else
            {
                // 添加到绑定，并判断类型是否支持
                if (!AddToBoundGameObjectDic(from, 1))
                {
                    Debug.LogError($"QuickUI Error 不被支持的类型！");
                    return;
                }
                // 有重名变量进行重命名
                if (UIObjectsName.Contains(name))
                {
                    int startNumber = 0;
                    name += startNumber;
                    while (UIObjectsName.Contains(name))
                    {
                        name += ++startNumber;
                    }
                    Debug.LogWarning($"QuickUI Warning 变量名重复！已被重命名为 {name}");
                }

                UIObjectsArray.Add(from);
                UIObjectsName.Add(name);
            }
        }

        /// <summary>
        /// 检查如果有移除的则从列表移除它
        /// </summary>
        public void CheckRemove()
        {
            for (int index = UIObjectsArray.Count - 1; index >= 0; index --)
            {
                if (!UIObjectsArray[index])
                {
                    UIObjectsArray.RemoveAt(index);
                    UIObjectsName.RemoveAt(index);
                }
            }
            InitObjects();
        }

        public void ClearAllCache()
        {
            boundGameObjectsDic.Clear();
            UIObjectsName.Clear();
            UIObjectsArray.Clear();
        }

        /// <summary>
        /// 由 Object 获取 GameObject
        /// </summary>
        /// <param name="from">UnityEngine.Object</param>
        /// <returns>目标的 GameObject</returns>
        public GameObject GetGameObject(UnityEngine.Object from)
        {
            if (from is GameObject goTarget)
                return goTarget;
            else if (from is Component compTarget)
                return compTarget.gameObject;
            else
                return null;
        }

        /// <summary>
        /// 是否有 Object
        /// </summary>
        public bool HasObject(UnityEngine.Object from)
        {
            return UIObjectsArray.Contains(from);
        }

        /// <summary>
        /// 获取数据的 Hash，校验是否变更
        /// </summary>
        public override int GetHashCode()
        {
            int result = 0;
            for (int index = 0; index < UIObjectsName.Count; index ++)
            {
                result += UIObjectsName[index].Length * (index + 1);
            }
            return result;
        }
    }
}
#endif