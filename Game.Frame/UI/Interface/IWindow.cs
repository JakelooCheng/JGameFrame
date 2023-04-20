using UnityEngine;

namespace Game.Frame.UI.Interface
{
    public interface IWindow : IUIElement
    {
        bool IsActive { get; }

        int SortingOrder { get; set; }

        Canvas Canvas { get; }

        /// <summary>
        /// 传递给窗口的数据
        /// </summary>
        System.Object Data { get; set;  }

        void Create(RectTransform target, IUIElement parent);

        void Show();

        void Hide();

        void Destroy();
    }
}