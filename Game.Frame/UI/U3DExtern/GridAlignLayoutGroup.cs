using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Game.Frame.UI
{
    /// <summary>
    /// UI 对齐网格布局
    /// 用于在网格布局换行时改变子元素排列方向
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class GridAlignLayoutGroup : UIBehaviour, ILayoutGroup
    {
        /// <summary>
        /// 换行后对齐方式
        /// </summary>
        public enum Align
        { 
            Left = 0,
            Center = 1,
            Right = 2
        }

        public Vector4 Padding;
        public Vector2 CellSize = new Vector2(100, 100);
        public bool UseChildSize = false;

        public Vector2 Space;
        [Header("不满一行时对齐方式")]
        public Align NextAlign;

        [Header("子元素影响自身高度")]
        public bool ContentSizeFitHeight = true;
        [Header("影响外层 Fitter")]
        public ContentSizeFitter rebuildTarget;

        private RectTransform self;

        [HideInInspector]
        public int ColCount = 0;


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

        private void Flush()
        {
            if (!self)
            {
                return;
            }

            // 计算列数量
            ColCount = (int)((self.sizeDelta.x - Padding.x) / (CellSize.x + Space.x));
            if (ColCount < 1)
            {
                ColCount = 1;
            }
            RectTransform[] lastItems = new RectTransform[ColCount];
            Vector2 startPos = new Vector2(Padding.x, Padding.y);

            // 获取子元素大小
            if (UseChildSize && self.childCount > 0)
            {
                CellSize = self.GetChild(0).GetComponent<RectTransform>().sizeDelta;
            }

            // 遍历处理每一个节点
            int childCount = 0;
            foreach (RectTransform child in self)
            {
                if (child && child.gameObject.activeSelf && child.transform.localScale.z > 0)
                {
                    // 更新倒数元素列表，用于之后单独处理最后几个子元素
                    lastItems[childCount % ColCount] = child;

                    // 改变子元素位置
                    Vector2 curSpace = new Vector2(childCount % ColCount * Space.x, childCount / ColCount * Space.y);
                    child.anchorMax = Vector2.up;
                    child.anchorMin = Vector2.up;
                    // Padding + CellSize + Space + 自身中心偏移
                    child.anchoredPosition = new Vector2(startPos.x + childCount % ColCount * CellSize.x + curSpace.x + child.sizeDelta.x / 2, 
                        -(startPos.y + childCount / ColCount * CellSize.y + curSpace.y + child.sizeDelta.y / 2));

                    childCount++;
                }
            }

            // 处理最后余数节点
            int remainder = childCount % ColCount;
            if (NextAlign == Align.Center)
            {
                float startX = Padding.x + ((ColCount - remainder) * CellSize.x + (ColCount - remainder) * Space.x) / 2;
                for (int index = 0; index < remainder; index++)
                {
                    lastItems[index].anchoredPosition = new Vector2(startX + index * CellSize.x + index * Space.x + lastItems[index].sizeDelta.x / 2, lastItems[index].anchoredPosition.y);
                }
            }
            else if (NextAlign == Align.Right)
            {
                float startX = Padding.x + ColCount * CellSize.x + (ColCount - 1) * Space.x;
                for (int index = 0; index < remainder; index++)
                {
                    lastItems[index].anchoredPosition = new Vector2(startX - index * CellSize.x - index * Space.x - lastItems[index].sizeDelta.x / 2, lastItems[index].anchoredPosition.y);
                }
            }

            // 改变高度
            if (ContentSizeFitHeight)
            {
                int lineCount = (childCount / ColCount + (remainder > 0 ? 1 : 0));
                self.sizeDelta = new Vector2(self.sizeDelta.x, Padding.y + Padding.w + lineCount * CellSize.y + (lineCount - 1) * Space.y);
            }
        }

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