using GameApp;
using Game.Frame.UI;
using Game.Frame.UI.Core;
using Game.Extern.QuickUI;
using UnityEngine.UI;

namespace GameApp.UI.System
{
    [UIWindow("UI/Prefabs/System/Reward/RewardWindow", LayerType.Form, WindowMode.Exclusive, ClearMode.Default)]
    public partial class RewardWindow : UIWindow
    {
        #region (禁止编辑)自动生成变量
		// HashCode:29
		private UnityEngine.UI.Image imageItemIcon;
		private UnityEngine.UI.Button btnClose;
		#endregion (禁止编辑)自动生成变量

        private void InitComponents()
        {
            QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();
            /// 在这里初始化组件

            #region (禁止编辑)自动生成绑定
			imageItemIcon = quickUI.UIObjectsArray[0] as UnityEngine.UI.Image;
			btnClose = quickUI.UIObjectsArray[1] as UnityEngine.UI.Button;
			#endregion (禁止编辑)自动生成绑定
        }
    }
} 