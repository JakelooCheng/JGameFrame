using Game.Frame.UI.Interface;
using UnityEngine;
using Game.Frame.Resource.Interface;
using Game.Frame.Resource;
using UnityEngine.UI;
using System;
using Game.Base.Logs;

namespace Game.Frame.UI.Core
{
    public class UIWindow : IWindow
    {
        public bool IsActive { get; private set; } = false;
        public int SortingOrder
        {
            get => Canvas.sortingOrder;
            set
            {
                if (Canvas != null)
                {
                    Canvas.sortingOrder = value;
                }
            }
        }
        public Canvas Canvas { get; private set; }
        public System.Object Data { get; set; }
        public IResourceToken Token { get; private set; }
        public IUIElement Parent { get; private set; }
        public RectTransform RectTransform { get; set; }
        public event Action<bool> OnVisibleChange;

        private GraphicRaycaster raycaster;

        public void Create(RectTransform target, IUIElement parent)
        {
            RectTransform = target;
            Parent = parent;
            Token = AppFacade.Resource.GetToken();

            // 归正
            RectTransform.localScale = Vector3.one;
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.sizeDelta = Vector2.zero;

            Canvas = target.GetComponent<Canvas>();
            if (Canvas == null)
            {
                Canvas = target.gameObject.AddComponent<Canvas>();
                Canvas.overrideSorting = true;
            }

            raycaster = RectTransform.GetComponent<GraphicRaycaster>();
            if (raycaster == null)
                raycaster = RectTransform.gameObject.AddComponent<GraphicRaycaster>();

            parent.OnVisibleChange += OnParentVisible;

            try
            {
                OnCreate();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        public void Show()
        {
            if (IsActive)
            {
                Hide(); // 如果之前 Show 的话，强制 Hide 再 Show 触发刷新。
            }

            IsActive = true;
            RectTransform.gameObject.SetActive(IsActive);

            try
            {
                OnShow();
                OnVisibleChange?.Invoke(IsActive);
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }

        /// <summary>
        /// 父节点显隐变化
        /// </summary>
        /// <param name="isActive">显示/隐藏</param>
        private void OnParentVisible(bool isActive)
        {
            OnVisibleChange?.Invoke(isActive);

            if (isActive)
                OnShow();
            else
                OnHide();
        }

        public void Hide()
        {
            if (!IsActive)
            {
                return;
            }

            IsActive = false;
            RectTransform.gameObject.SetActive(IsActive);

            try
            {
                OnVisibleChange?.Invoke(IsActive);
                OnHide();
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }

        public void Destroy()
        {
            if (!RectTransform)
            {
                return;
            }

            //TODO 清理 Fragment 缓存

            Hide();

            GameObject.Destroy(RectTransform.gameObject);
            RectTransform = null;

            Parent.OnVisibleChange -= OnParentVisible;
            AppFacade.Resource.ReleaseToken(Token);

            OnDestroy();
        }

        public void CloseWindow()
        {
            AppFacade.UI.CloseWindow(this.GetType());
        }

        protected virtual void OnCreate()
        {

        }

        protected virtual void OnShow()
        {

        }

        protected virtual void OnHide()
        {

        }

        protected virtual void OnDestroy()
        {

        }
    }
}