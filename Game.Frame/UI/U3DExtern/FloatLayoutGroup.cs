using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.Frame.UI
{
    /// <summary>
    /// 浮动布局，类似 CSS 的 Float 布局
    /// 可实现不同大小子组件靠左或靠右对齐
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class FloatLayoutGroup : UIBehaviour, ILayoutGroup
    {
        /// <summary>
        /// 对齐方式
        /// </summary>
        public enum Align
        {
            Left = 0,
            Right = 1
        }

        [Header("父边框")]
        public Vector4 Padding;

        [Header("子外边距")]
        public Vector4 ChildMargin;


        [Header("浮动对齐方式")]
        public Align CurAlign;


        [Header("子元素影响自身高度")]
        public bool ContentSizeFitHeight = true;

        [Header("影响外层 Fitter")]
        public ContentSizeFitter rebuildTarget;

        private RectTransform self;

        /// <summary>
        /// Unity API
        /// </summary>
        public void SetLayoutHorizontal() { Flush(); }
        public void SetLayoutVertical() { }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            Flush();
        }

        protected override void Awake()
        {
            self = GetComponent<RectTransform>();
        }

        /// <summary>
        /// 刷新总方法
        /// </summary>
        public void Flush()
        {
            if (!self)
            {
                return;
            }

            float maxWidth = self.sizeDelta.x;

            float startX = CurAlign == Align.Left ? Padding.x : maxWidth - Padding.z;
            float startY = -Padding.y - ChildMargin.y;

            float curMaxHeight = 0;

            for (int index = 0; index < self.childCount; index ++)
            {
                // 拿子元素
                RectTransform childTransform = self.GetChild(index).GetComponent<RectTransform>();

                if (!childTransform.gameObject.activeInHierarchy)
                {
                    continue;
                }

                // 计算缩放
                float childWidth = childTransform.sizeDelta.x * childTransform.localScale.x;
                float childHeight = childTransform.sizeDelta.y * childTransform.localScale.y;

                // 调整自身
                if (CurAlign == Align.Left)
                {
                    // 判断是否换行
                    if (startX + childWidth + Padding.z + ChildMargin.x + ChildMargin.z > maxWidth)
                    {
                        startX = Padding.x;
                        startY -= (curMaxHeight + ChildMargin.y + ChildMargin.w);
                        curMaxHeight = 0;
                    }
                    childTransform.pivot = Vector2.up;
                    childTransform.anchoredPosition = new Vector2(startX + ChildMargin.x, startY + ChildMargin.y);
                    startX += (childWidth + ChildMargin.x + ChildMargin.z);
                }
                else
                {
                    // 判断是否换行
                    if (startX - childWidth - Padding.x - ChildMargin.x - ChildMargin.z < 0)
                    {
                        startX = maxWidth - Padding.z;
                        startY -= (curMaxHeight + ChildMargin.y + ChildMargin.w);
                        curMaxHeight = 0;
                    }
                    childTransform.pivot = Vector2.one;
                    childTransform.anchoredPosition = new Vector2(startX - ChildMargin.z, startY + ChildMargin.y);
                    startX -= (childWidth + ChildMargin.x + ChildMargin.z);
                }


                // 刷新最高高度
                if (curMaxHeight < childHeight)
                {
                    curMaxHeight = childHeight;
                }
            }

            /// 调整父高度
            if (ContentSizeFitHeight)
            {
                float contentHeight = Math.Abs(startY) + Padding.w + curMaxHeight + ChildMargin.y + ChildMargin.w;
                self.sizeDelta = new Vector2(self.sizeDelta.x, contentHeight >= 0 ? contentHeight : 0);
            }
        }

        /// <summary>
        /// 外部强制刷新
        /// </summary>
        public void RebuildImmediately()
        {
            Flush();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetLayoutHorizontal();
        }
#endif
    }
}
