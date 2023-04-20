using Game.Base.LifeCycle;
using Game.Base.Module;
using Game.Base.Logs;
using Game.Frame.UI.Interface;
using UnityEngine;
using System;
using System.Collections.Generic;
using Game.Frame.UI.Core;
using Game.Frame.Resource.Interface;
using Game.Frame.UI.Utility;
using Game.Frame.Timer;

namespace Game.Frame.UI
{
    public class UIManager : GameModuleBase, IInit, IUIElement, IShutdown
    {
        /// <summary>
        /// 根节点
        /// </summary>
        public IUIElement Parent => null;

        public IResourceToken Token { get; private set; }

        public RectTransform RectTransform { get; set; }

        private readonly List<ILayer> layers = new List<ILayer>((int)LayerType.Count);

        public event Action<bool> OnVisibleChange;

        /// <summary>
        /// 窗口缓存
        /// </summary>
        protected readonly Dictionary<Type, WindowInfo> windowsCache = new Dictionary<Type, WindowInfo>();


        public void Init()
        {
            RectTransform = GameObject.Find("GameEntry/Canvas").GetComponent<RectTransform>();
            Token = AppFacade.Resource.GetToken();

            for (int index = 0; index < (int)LayerType.Count; index ++)
            {
                var layer = new UILayer();
                layer.Create(RectTransform.GetChild(index).GetComponent<RectTransform>(), this);
                layers.Add(layer);
            }

            OnVisibleChange?.Invoke(true);
        }

        /// <summary>
        /// 显示 Layer
        /// </summary>
        /// <param name="layerType"></param>
        public void ShowLayer(LayerType layerType)
        {
            if (layerType != LayerType.Count)
            {
                layers[(int)layerType].Show();
            }
        }

        /// <summary>
        /// 隐藏 Layer
        /// </summary>
        /// <param name="layerType"></param>
        public void HideLayer(LayerType layerType)
        {
            if (layerType != LayerType.Count)
            {
                layers[(int)layerType].Hide();
            }
        }

        /// <summary>
        /// 打开窗口
        /// </summary>
        public void OpenWindow<T>(object data = null, Action<IWindow> callback = null) where T : IWindow
        {
            OpenWindow(typeof(T), data, callback);
        }

        /// <summary>
        /// 打开窗口
        /// </summary>
        public void OpenWindow(Type windowType, object data = null, Action<IWindow> callback = null)
        {
            if (!windowsCache.TryGetValue(windowType, out var windowInfo))
            {
                windowInfo = WindowInfo.Make(windowType);
                if (windowInfo.Attribute == null)
                {
                    return;
                }
                windowsCache.Add(windowType, windowInfo);
            }

            layers[(int)windowInfo.Attribute.LayerType].OpenWindow(windowInfo, data);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseWindow<T>() where T : IWindow
        {
            CloseWindow(typeof(T));
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseWindow(Type windowType)
        {
            if (windowsCache.TryGetValue(windowType, out var windowInfo))
            {
                layers[(int)windowInfo.Attribute.LayerType].CloseWindow(windowInfo);
            }
        }

        /// <summary>
        /// 查找窗口
        /// </summary>
        public T FindWindow<T>() where T : IWindow
        {
            return (T)FindWindow(typeof(T));
        }

        /// <summary>
        /// 查找窗口
        /// </summary>
        public object FindWindow(Type windowType)
        {
            if (windowsCache.TryGetValue(windowType, out var windowInfo))
            {
                return windowInfo.Window;
            }
            return null;
        }

        public void Shutdown()
        {
            OnVisibleChange?.Invoke(false);
        }
    }
}