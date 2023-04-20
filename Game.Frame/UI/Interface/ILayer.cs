using Game.Frame.UI.Utility;
using UnityEngine;

namespace Game.Frame.UI.Interface
{
    public interface ILayer : IUIElement
    {
        Canvas Canvas { get; }

        void Create(RectTransform target, IUIElement parent);

        void Show();

        void Hide();

        void OpenWindow(WindowInfo windowInfo, System.Object data);

        void CloseWindow(WindowInfo windowInfo);
    }
}