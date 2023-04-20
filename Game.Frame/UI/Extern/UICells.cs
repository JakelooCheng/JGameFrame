using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Frame.UI
{ 
    /// <summary>
    /// 用于管理少量重复组件。如标签列表等。
    /// 方便后续整体优化性能，使用方便。
    /// <typeparam name="Cell">子元素需继承 UICell</typeparam>
    public class UICells<Cell> where Cell : UICell, new()
    {
        public RectTransform Root;

        protected GameObject template;

        protected List<Cell> cells = new List<Cell>();

        private int curIndex = 0;

        /// <summary>
        /// Create 里先执行初始化
        /// </summary>
        /// <param name="root"></param>
        public void Init(Transform root)
        {
            Root = root.GetComponent<RectTransform>();
            template = root.GetChild(0).gameObject;
            for (int index = 0; index < root.childCount; index ++)
            {
                AddCell(root.GetChild(index).gameObject);
            }
        }

        /// <summary>
        /// Create 里先执行初始化，忽略 startIndex 的 Cell，比如 Title
        /// </summary>
        /// <param name="root"></param>
        public void Init(Transform root, int startIndex)
        {
            Root = root.GetComponent<RectTransform>();
            template = root.GetChild(startIndex).gameObject;
            for (int index = startIndex; index < root.childCount; index++)
            {
                AddCell(root.GetChild(index).gameObject);
            }
        }

        /// <summary>
        /// 拿一个 Cell
        /// </summary>
        public virtual Cell GetCell()
        {
            if (cells.Count <= curIndex)
            {
                AddCell(GameObject.Instantiate(template, Root));
            }
            var cell = cells[curIndex++];
            cell.Root.SetUIActive(true);
            return cell;
        }

        /// <summary>
        /// 拿指定位置的 Cell，重载索引器
        /// </summary>
        public Cell this[int index]
        {
            get
            {
                return cells[index < cells.Count ? index : cells.Count - 1];
            }
        }

        /// <summary>
        /// 获取组件数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return cells.Count;
        }

        /// <summary>
        /// 获取 Cell List
        /// </summary>
        public List<Cell> GetCells()
        {
            return cells;
        }

        /// <summary>
        /// 释放所有 Cell
        /// </summary>
        public virtual void ReleaseAll()
        {
            for (int index = 0; index < curIndex; index++)
            {
                this[index].Root.SetUIActive(false);
            }
            curIndex = 0;
        }

        /// <summary>
        /// 释放下次刷新时不会用到的 Cell（性能更优）
        /// </summary>
        /// <param name="newCount">下次刷新的数量</param>
        public virtual void ReleaseAll(int newCount)
        {
            if (newCount < curIndex)
            {
                for (int index = newCount; index < curIndex; index ++)
                {
                    this[index].Root.SetUIActive(false);
                }
            }
            curIndex = 0;
        }

        /// <summary>
        /// 释放所有 Cell
        /// </summary>
        /// <param name="releaseCallback">每个Cell被释放时回调</param>
        public virtual void ReleaseAll(Action<Cell> releaseCallback)
        {
            for (int index = 0; index < curIndex; index++)
            {
                releaseCallback(this[index]);
                this[index].Root.SetUIActive(false);
            }
            curIndex = 0;
        }

        protected virtual void AddCell(GameObject root)
        {
            var cell = new Cell
            {
                Root = root
            };
            cell.Init(root.transform);
            root.SetUIActive(false);
            cells.Add(cell);
        }
    }

    /// <summary>
    /// 简易 UICell 池
    /// </summary>
    /// <typeparam name="Cell">子元素需继承 UICell</typeparam>
    public class UICellsPool<Cell> : UICells<Cell> where Cell : UICellPool, new()
    {
        private Queue<Cell> freeCells = new Queue<Cell>();

        /// <summary>
        /// 拿一个 Cell
        /// </summary>
        public override Cell GetCell()
        {
            if (freeCells.Count == 0)
            {
                AddCell(GameObject.Instantiate(template, Root));
            }
            var cell = freeCells.Dequeue();
            cells.Add(cell);
            cell.Root.SetUIActive(true);
            return cell;
        }

        /// <summary>
        /// 释放所有 Cell
        /// </summary>
        public override void ReleaseAll()
        {
            ReleaseAll(0);
        }

        /// <summary>
        /// 释放所有 Cell
        /// </summary>
        public override void ReleaseAll(int newCount)
        {
            for (int index = cells.Count - 1; index >= 0; index--)
            {
                Release(cells[index]);
            }
        }

        /// <summary>
        /// 释放对应的 Cell
        /// </summary>
        /// <param name="cell"></param>
        public void Release(Cell cell)
        {
            cell.Release();
            freeCells.Enqueue(cell);
            cells.Remove(cell);
            cell.Root.SetUIActive(false);
        }

        protected override void AddCell(GameObject root)
        {
            var cell = new Cell
            {
                Root = root
            };
            cell.Init(root.transform);
            root.SetUIActive(false);
            freeCells.Enqueue(cell);
        }
    }

    /// <summary>
    /// 少量重复组件中的子组件（列表管理）
    /// </summary>
    public abstract class UICell
    {
        public GameObject Root;
        
        public UICell()
        {

        }

        /// <summary>
        /// Cell 生成实例时执行
        /// </summary>
        public abstract void Init(Transform root);
    }

    /// <summary>
    /// 少量重复组件中的子组件（池管理）
    /// </summary>
    public abstract class UICellPool : UICell
    {
        /// <summary>
        /// Cell 回收时执行
        /// </summary>
        /// <param name="root"></param>
        public abstract void Release();
    }
}