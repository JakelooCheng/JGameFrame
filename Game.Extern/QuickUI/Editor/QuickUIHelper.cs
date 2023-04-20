using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Text;
using Game.Frame.UI;

namespace Game.Extern.QuickUI.Editor
{
    [UIWindow("{1}", LayerType.Form, WindowMode.Exclusive, ClearMode.Default)]
    public static partial class QuickUIHelper
    {
        private static StringBuilder stringCache = new StringBuilder();

        /// <summary>
        /// 生成变量的封装类型
        /// </summary>
        public enum PackageType
        {
            Public = 0,
            Private = 1,
            Protected = 2
        }

        /// <summary>
        /// 忽略名称
        /// </summary>
        private static readonly Dictionary<Type, string[]> ignoreNameDic = new Dictionary<Type, string[]>()
        {
            {typeof(Button), new string[] { "Btn", "btn", "Button", "button" } },
            {typeof(RawImage), new string[] { "Image", "Img", "img", "image", "tex", "Tex" } },
            {typeof(Image), new string[] { "Image", "Img", "img", "image", "tex", "Tex" } },
            {typeof(Text), new string[] { "Text", "text", "txt", "Txt" } },
            {typeof(ScrollRect), new string[] { "Scroll", "scroll" } },
        };

        /// <summary>
        /// 替换首部名称
        /// </summary>
        private static readonly Dictionary<Type, string> replaceNameDic = new Dictionary<Type, string>()
        {
            {typeof(RectTransform), "tr" },
            {typeof(Transform), "tr" },
            {typeof(Button), "btn" },
            {typeof(Text), "txt" },
            {typeof(ScrollRect), "scroll" },
            {typeof(LayoutElement), "layout" },
            {typeof(GridLayoutGroup), "layout" },
            {typeof(HorizontalLayoutGroup), "layout" },
            {typeof(VerticalLayoutGroup), "layout" },
            {typeof(Image), "image" },
            {typeof(RawImage), "image" },
            {typeof(GameObject), "go" },
            {typeof(Toggle), "toggle" },
            {typeof(ToggleGroup), "toggle" },
            {typeof(CanvasGroup), "canvas" },
        };

        public static readonly List<string> MarkIconList = new List<string>()
        {
            "❶", "❷", "❸", "❹", "❺", "❻", "❼", "❽", "❾", "❿", 
        };

        private static readonly string boundMark = "@";

        /// <summary>
        /// 默认组件添加在前面的名字
        /// </summary>
        private static readonly string replaceNameDefault = "comp";
        private static readonly string propNameDefault = "Object";

        public static string MakeName(string name, Type type)
        {
            stringCache.Clear();
            stringCache.Append(name);

            // 移除不期望的
            if (ignoreNameDic.TryGetValue(type, out var ignoreNames))
            {
                foreach (var ignoreName in ignoreNames)
                {
                    stringCache.Replace(ignoreName, "");
                }
                if (stringCache.Length == 0)
                {
                    stringCache.Append(propNameDefault);
                }
            }

            // 移除空格
            stringCache.Replace(" ", "");

            // 移除标记符
            stringCache.Replace("@", "");

            // 首字母大写
            var firstWord = stringCache.ToString().Substring(0, 1).ToUpper();
            stringCache.Remove(0, 1);
            stringCache.Insert(0, firstWord);

            // 添加对应的前缀
            if (!replaceNameDic.TryGetValue(type, out string first))
            {
                first = replaceNameDefault;
            }
            stringCache.Insert(0, first);

            return stringCache.ToString();
        }

        /// <summary>
        /// 快速绑定
        /// </summary>
        public static void QuickBound(QuickUIMono comp)
        {
            Transform[] transList = new Transform[] { comp.transform } ;
            List<Transform> tempList = new List<Transform>();
            // 绑定所有子节点
            while (transList != null && transList.Length > 0)
            {
                tempList.Clear();
                foreach (var trans in transList)
                {
                    // 当前节点没有不是根才加进来
                    var transComp = trans.GetComponent<QuickUIMono>();
                    if (transComp != null && transComp != comp)
                    {
                        continue;
                    }

                    // 子节点加入下次遍历
                    for (int index = 0; index < trans.childCount; index++)
                    {
                        var child = trans.GetChild(index);
                        // 判断是否绑定
                        QuickBoundGameObject(child.gameObject, comp);
                        tempList.Add(child);
                    }
                }
                transList = tempList.ToArray();
            }
        }

        private static void QuickBoundGameObject(GameObject target, QuickUIMono comp)
        {
            // 是否标注了绑定
            if (target.name.IndexOf(boundMark) < 0)
            {
                return;
            }

            var gameObjectInfo = new GameObjectInfo(target);
            foreach (var replaceName in replaceNameDic)
            {
                string realName = replaceName.Value.Substring(0, 1).ToUpper() + replaceName.Value.Substring(1);
                // 以固定形式开头
                if (target.name.IndexOf(realName) == 0)
                {
                    foreach (var obj in gameObjectInfo.Objects)
                    {
                        // 有这样的类型
                        if (obj.GetType() == replaceName.Key)
                        {
                            // 没有加过，加进去
                            if (!comp.HasObject(obj))
                            {
                                comp.AddOrRemove(obj, MakeName(obj.name, obj.GetType()));
                            }
                            return;
                        }
                    }
                }
            }
            Debug.LogWarning($"QuickUI Warning 标记了绑定却未以固定命名开头 {target.name}");
        }
    }

    /// <summary>
    /// 每个 GameObject 身上的所有组件
    /// </summary>
    public class GameObjectInfo
    {
        public string[] ObjectNames;

        public UnityEngine.Object[] Objects;

        public GameObject Self;

        public GameObjectInfo(GameObject target)
        {
            this.Self = target;

            var objectNamesList = new List<string>();
            var objectTypsList = new List<UnityEngine.Object>();

            // 特殊处理 GameObject
            objectNamesList.Add("GameObject");
            objectTypsList.Add(target);

            var components = target.transform.GetComponents<Component>();
            foreach (var comp in components)
            {
                objectNamesList.Add(comp.GetType().Name);
                objectTypsList.Add(comp);

                // 当是自身的时候，暴露给父类一个
                if (comp is QuickUIMono)
                {
                    var rectTransform = target.transform.GetComponent<RectTransform>();
                    objectNamesList.Add(rectTransform.GetType().Name + "（到父节点）");
                    objectTypsList.Add(rectTransform);
                }
            }

            ObjectNames = objectNamesList.ToArray();
            Objects = objectTypsList.ToArray();
        }
    }
}