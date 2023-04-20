using GameApp;
using Game.Frame.UI;
using Game.Frame.UI.Core;
using Game.Extern.QuickUI;
using UnityEngine.UI;

namespace GameApp.UI.System
{
    [UIWindow("UI/Prefabs/System/Transition/CommonUITransitionWindow", LayerType.Toast, WindowMode.Permanent, ClearMode.Default)]
    public partial class CommonUITransitionWindow : UIWindow
    {
        #region (禁止编辑)自动生成变量
		// HashCode:66
		private UnityEngine.RectTransform trDipToBlackTransition;
		private UnityEngine.RectTransform trDipToImageTransition;
		#endregion (禁止编辑)自动生成变量

        private void InitComponents()
        {
            QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();
            /// 在这里初始化组件

            #region (禁止编辑)自动生成绑定
			trDipToBlackTransition = quickUI.UIObjectsArray[0] as UnityEngine.RectTransform;
			trDipToImageTransition = quickUI.UIObjectsArray[1] as UnityEngine.RectTransform;
			#endregion (禁止编辑)自动生成绑定
        }
    }
} 