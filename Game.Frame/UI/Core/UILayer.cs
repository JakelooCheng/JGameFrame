using Game.Frame.UI.Interface;
using Game.Frame.Resource.Interface;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Frame.UI.Utility;
using Game.Base.Logs;

namespace Game.Frame.UI.Core
{
    public class UILayer : ILayer
    {
        public Canvas Canvas { get; private set; }
        public IResourceToken Token { get; private set; }
        public IUIElement Parent { get; private set; }
        public RectTransform RectTransform { get; set; }
        public event Action<bool> OnVisibleChange;

        private int CurLayerSortingOrder => Canvas.sortingOrder;

        /// <summary>
        /// 互斥界面栈
        /// </summary>
        protected readonly LinkedList<WindowInfo> exclusiveWindowsList = new LinkedList<WindowInfo>();

        /// <summary>
        /// 已经打开的共存窗口列表
        /// </summary>
        protected readonly HashSet<WindowInfo> inclusiveWindowsList = new HashSet<WindowInfo>();

        /// <summary>
        /// 已经打开的常驻窗口列表
        /// </summary>
        protected readonly HashSet<WindowInfo> permanentWindowsList = new HashSet<WindowInfo>();


        public void Create(RectTransform target, IUIElement parent)
        {
            RectTransform = target;
            Parent = parent;
            Token = AppFacade.Resource.GetToken();

            Canvas = target.GetComponent<Canvas>();
            if (Canvas == null)
                Canvas = target.gameObject.AddComponent<Canvas>();
        }

        public void Show()
        {
            OnVisibleChange?.Invoke(true);
            Canvas.enabled = true;
        }

        public void Hide()
        {
            Canvas.enabled = false;
            OnVisibleChange?.Invoke(false);
        }

        public void OpenWindow(WindowInfo windowInfo, System.Object data)
        {
            PreloadWindow(windowInfo, info =>
            {
                ShowWindowInLayer(info, data);
            });
        }

        /// <summary>
        /// 预加载 Window
        /// </summary>
        private void PreloadWindow(WindowInfo windowInfo, Action<WindowInfo> loadCallback)
        {
            if (windowInfo.Window == null)
            {
                LoadWindow(windowInfo, loadCallback);
            }
            else
            {
                loadCallback?.Invoke(windowInfo);
            }
        }

        /// <summary>
        /// 加载并创建 Window
        /// </summary>
        /// <param name="windowInfo"></param>
        /// <param name="loadCallback"></param>
        private void LoadWindow(WindowInfo windowInfo, Action<WindowInfo> loadCallback)
        {
            AppFacade.Resource.LoadAsset(windowInfo.Attribute.AssetPath, asset =>
            {
                if (asset.RealAsset == null)
                {
                    Log.Error("UIManager UILayer Error 不存在 UI 预制体！");
                }
                windowInfo.Asset = asset;

                var windowGameObject = GameObject.Instantiate(asset.RealAsset, RectTransform) as GameObject;
                windowGameObject.name = windowInfo.ComponentType.Name;
                var windowComponent = (IWindow)Activator.CreateInstance(windowInfo.ComponentType);

                windowComponent.Create(windowGameObject.GetComponent<RectTransform>(), this);
                windowGameObject.SetActive(false);
                windowInfo.Window = windowComponent;
 
                loadCallback?.Invoke(windowInfo);
            }, Token);
        }

        private void ShowWindowInLayer(WindowInfo windowInfo, System.Object data)
        {
            var windowComponent = windowInfo.Window;
            windowComponent.Data = data;
            windowComponent.SortingOrder = GetNextSortingOrder();

            switch (windowInfo.Attribute.WindowMode)
            {
                case WindowMode.Exclusive: // 互斥窗口
                    // 关闭所有共存窗口
                    foreach (var window in inclusiveWindowsList)
                    {
                        HideWindowInLayer(window);
                    }
                    // 把窗口置于栈顶
                    exclusiveWindowsList.Remove(windowInfo);
                    // 隐藏栈内窗口
                    var lastWindow = exclusiveWindowsList.Last;
                    if (lastWindow != null)
                    {
                        lastWindow.Value.Window.Hide();
                    }
                    exclusiveWindowsList.AddLast(windowInfo);
                    break;
                case WindowMode.Inclusive: // 共存窗口，直接盖在上面，不影响其他窗口
                    inclusiveWindowsList.Add(windowInfo);
                    break;
                case WindowMode.Permanent: // 常驻窗口，不影响其他窗口
                    permanentWindowsList.Add(windowInfo);
                    break;
            }

            windowComponent.Show();
        }

        private void HideWindowInLayer(WindowInfo windowInfo)
        {
            var windowComponent = windowInfo.Window;
            windowComponent.Hide();
            windowComponent.Data = null;

            switch (windowInfo.Attribute.WindowMode)
            {
                case WindowMode.Exclusive: // 互斥窗口
                    exclusiveWindowsList.Remove(windowInfo);
                    // 恢复栈顶窗口
                    var lastWindow = exclusiveWindowsList.Last;
                    if (lastWindow != null)
                    {
                        lastWindow.Value.Window.Show();
                    }
                    break;
                case WindowMode.Inclusive: // 共存窗口，直接盖在上面，不影响其他窗口
                    inclusiveWindowsList.Remove(windowInfo);
                    break;
                case WindowMode.Permanent: // 常驻窗口，不影响其他窗口
                    permanentWindowsList.Remove(windowInfo);
                    break;
            }
        }

        /// <summary>
        /// 获取下一个 SortingOrder
        /// </summary>
        /// <returns>最上层的 SortingOrder</returns>
        private int GetNextSortingOrder()
        {
            int curMaxSortingLayer = CurLayerSortingOrder;
            var stackLastSortingOrder = exclusiveWindowsList.Last?.Value.Window.SortingOrder;
            if (stackLastSortingOrder > curMaxSortingLayer)
            {
                curMaxSortingLayer = (int)stackLastSortingOrder;
            }
            // 直接找最大值
            foreach (var window in inclusiveWindowsList)
            {
                int order = window.Window.SortingOrder;
                if (order > curMaxSortingLayer)
                    curMaxSortingLayer = order;
            }
            foreach (var window in permanentWindowsList)
            {
                int order = window.Window.SortingOrder;
                if (order > curMaxSortingLayer)
                    curMaxSortingLayer = order;
            }
            return curMaxSortingLayer + 10;
        }

        public void CloseWindow(WindowInfo windowInfo)
        {
            HideWindowInLayer(windowInfo);
        }
    }
}