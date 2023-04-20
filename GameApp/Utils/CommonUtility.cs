using Game.Base.Logs;
using Game.Frame;
using GameApp.UI.System;
using System;
using Game.Frame.Resource;
using Game.Frame.Resource.Interface;
using UnityEngine.UI;
using GameApp.Cache;
using GameApp.Entitys;

namespace GameApp.Utils
{
    /// <summary>
    /// 通用功能
    /// </summary>
    public static class CommonUtility
    {

        #region 设置图片
        public static void SetImageSprite(IResourceToken token, Image itemImage, string path, Action callback = null, bool autoSize = false)
        {
            
        }
        #endregion

        #region UI 转场
        /// <summary>
        /// UI 转场（进入，需手动关闭）
        /// </summary>
        public static void ShowTransIn(TUITransition type, float duration = 1000, Action afterIn = null, object args = null)
        {
            CommonUITransitionWindow transWindow = AppFacade.UI.FindWindow<CommonUITransitionWindow>();
            if (transWindow != null)
            {
                transWindow.TransIn(type, duration, afterIn, args);
            }
        }

        /// <summary>
        /// UI 转场（退出）
        /// </summary>
        public static void ShowTransOut(TUITransition type, float duration = 1000, Action afterOut = null)
        {
            CommonUITransitionWindow transWindow = AppFacade.UI.FindWindow<CommonUITransitionWindow>();
            if (transWindow != null)
            {
                transWindow.TransOut(type, duration, afterOut);
            }
        }

        /// <summary>
        /// UI 转场（进入-等待-退出）
        /// </summary>
        public static void ShowTransWait(TUITransition type, float durationIn = 1000, float durationWait = 1000, float durationOut = 1000, Action afterCallback = null)
        {
            CommonUITransitionWindow transWindow = AppFacade.UI.FindWindow<CommonUITransitionWindow>();
            if (transWindow != null)
            {
                transWindow.TransWait(type, durationIn, durationWait, durationOut, afterCallback);
            }
        }
        #endregion

        #region 弹出 Toast
        /// <summary>
        /// Toast
        /// </summary>
        public static void Toast(string title, string desc)
        {
            if (ToastWindow.Instance != null)
            {
                ToastWindow.Instance.Toast(title, desc);
            }
        }

        /// <summary>
        /// 奖励弹窗
        /// </summary>
        public static void Reward(int itemId, int count, Action closeCallback = null)
        {
            AppFacade.UI.OpenWindow<RewardWindow>(new RewardWindowArgs
            {
                ItemId = itemId,
                Count = count,
                CloseCallback = closeCallback
            });
        }
        #endregion

        #region Player
        /// <summary>
        /// 停止玩家移动
        /// </summary>
        public static void StopPlayer()
        {
            var player = GameCache.Player.PlayerEntity?.GetComp<PlayerComp>();
            if (player != null)
            {
                player.Stop();
            }
        }

        public static void StopCamera()
        {
            GameCache.GameState.CurMapScene?.CameraMovement.Stop();
        }

        public static void RunCamera()
        {
            GameCache.GameState.CurMapScene?.CameraMovement.Run();
        }

        /// <summary>
        /// 恢复玩家移动
        /// </summary>
        public static void RunPlayer()
        {
            var player = GameCache.Player.PlayerEntity?.GetComp<PlayerComp>();
            if (player != null)
            {
                player.Run();
            }
        }
        #endregion
    }
}
