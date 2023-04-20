using GameApp;
using Game.Frame.UI;
using Game.Frame.UI.Core;
using Game.Extern.QuickUI;
using UnityEngine.UI;
using UnityEngine;
using Game.Frame.Timer;
using Game.Frame;
using System;

namespace GameApp.UI.System
{
    [UIWindow("UI/Prefabs/System/Toast/ToastWindow", LayerType.Toast, WindowMode.Permanent, ClearMode.Default)]
    public partial class ToastWindow : UIWindow
    {
        #region (禁止编辑)自动生成变量
		// HashCode:33
		private UnityEngine.RectTransform trToastList;
		private UnityEngine.RectTransform trToastPool;
		#endregion (禁止编辑)自动生成变量

        private UICellsPool<ToastCell> toastPool = new UICellsPool<ToastCell>();

        private void InitComponents()
        {
            QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();
            /// 在这里初始化组件

            #region (禁止编辑)自动生成绑定
			trToastList = quickUI.UIObjectsArray[0] as UnityEngine.RectTransform;
			trToastPool = quickUI.UIObjectsArray[1] as UnityEngine.RectTransform;
			#endregion (禁止编辑)自动生成绑定

            toastPool.Init(trToastPool);
            ToastCell.ReleaseCall = ReleaseCell;
        }

        public class ToastCell : UICellPool
        {
            private Text titleText;
            private Text descText;

            public static Action<ToastCell> ReleaseCall;

            private ITimer releaseTimer;

            public override void Init(Transform root)
            {
                titleText = root.GetChild(0).GetComponent<Text>();
                descText = root.GetChild(1).GetComponent<Text>();
            }

            public void SetCell(string title, string desc)
            {
                titleText.text = title;
                descText.text = desc;

                releaseTimer = AppFacade.Timer.Wait(2000, () =>
                {
                    ReleaseCall.Invoke(this);
                });
            }

            public override void Release()
            {
                releaseTimer = null;
            }
        }
    }
} 