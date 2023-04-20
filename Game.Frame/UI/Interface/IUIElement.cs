using UnityEngine;
using Game.Frame.Resource.Interface;
using System;

namespace Game.Frame.UI.Interface
{
    public interface IUIElement
    {
        RectTransform RectTransform { get; }

        IResourceToken Token { get; }

        IUIElement Parent { get; }

        event Action<bool> OnVisibleChange;
    }
}