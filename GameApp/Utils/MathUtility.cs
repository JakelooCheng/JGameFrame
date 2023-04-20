using GameApp.Cache;
using UnityEngine;

namespace GameApp.Utils
{
    public static class MathUtility
    {
        /// <summary>获取场景里世界坐标为 worldPos 的点，在UI rectTransform 下的 localPosition </summary>
        public static Vector2 GetLocalPosition(Vector3 worldPos, RectTransform rectTransform)
        {
            Vector2 localPosition = Vector2.zero;
            var mainCamera = Camera.main;
            var uiCamera = GameCache.Player.CameraMovement.UICamera;
            if (mainCamera != null && uiCamera != null)
            {
                Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, worldPos);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, uiCamera, out localPosition);
            }
            return localPosition;
        }
    }
}
