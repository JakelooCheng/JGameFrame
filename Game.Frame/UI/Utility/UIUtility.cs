using System;
using System.Collections.Generic;
using System.Reflection;
using Game.Base.ObjectPool;
using Game.Frame.Resource.Interface;
using Game.Frame.UI.Interface;

namespace Game.Frame.UI.Utility
{
    public static class UIUtility
    {
        private static readonly Dictionary<Type, UIWindowAttribute> cacheUIWindowAttribute 
            = new Dictionary<Type, UIWindowAttribute>();

        /// <summary>
        /// 获取 UIWindow 类型信息
        /// </summary>
        /// <param name="elementType">UIWindow</param>
        /// <param name="attribute">UIWindowAttribute</param>
        /// <returns>是否存在</returns>
        public static bool TryGetUIWindowAttribute(Type elementType, out UIWindowAttribute attribute)
        {
            // 先找缓存
            if (cacheUIWindowAttribute.TryGetValue(elementType, out attribute))
            {
                return true;
            }

            // 反射找 Attribute
            attribute = elementType.GetCustomAttribute<UIWindowAttribute>();
            if (attribute != null)
            {
                cacheUIWindowAttribute.Add(elementType, attribute);
                return true;
            }
            return false;
        }
    }

    public class WindowInfo
    {
        public UIWindowAttribute Attribute;

        public IWindow Window;

        public IAsset Asset;

        public Type ComponentType;

        public static WindowInfo Make(Type windowType)
        {
            var info = GlobalObjectPool<WindowInfo>.Get();
            if (UIUtility.TryGetUIWindowAttribute(windowType, out var attr))
            {
                info.Attribute = attr;
                info.ComponentType = windowType;
            }
            return info;
        }

        public void Release()
        {
            GlobalObjectPool<WindowInfo>.Release(this);
        }

        public override bool Equals(object obj)
        {
            return ComponentType.Equals((obj as WindowInfo).ComponentType);
        }

        public override int GetHashCode()
        {
            return ComponentType.GetHashCode();
        }
    }
}