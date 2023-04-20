using System.Collections.Generic;
using UnityEngine;

namespace Game.Extern.QuickUI
{
    /// <summary>
    /// QuickUIMono UI 快速绑定。获取 UI 组件的入口。
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public partial class QuickUIMono : MonoBehaviour
    {
        /// <summary>
        /// 所有已绑定物件的数组
        /// </summary>
        [SerializeField]
        public List<Object> UIObjectsArray = new List<Object>();
    }
}