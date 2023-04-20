using GameApp;
using Game.Frame.UI;
using Game.Frame.UI.Core;
using Game.Extern.QuickUI;
using UnityEngine.UI;

namespace GameApp.UI.Start
{
    [UIWindow("UI/Prefabs/Start/StartWindow", LayerType.Form, WindowMode.Exclusive, ClearMode.Default)]
    public partial class StartWindow : UIWindow
    {
        #region (禁止编辑)自动生成变量
		// HashCode:36
		private UnityEngine.UI.Button btnStart;
		private UnityEngine.UI.Button btnOpenSetting;
		#endregion (禁止编辑)自动生成变量

        private void InitComponents()
        {
            QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();
            /// 在这里初始化组件

            #region (禁止编辑)自动生成绑定
			btnStart = quickUI.UIObjectsArray[0] as UnityEngine.UI.Button;
			btnOpenSetting = quickUI.UIObjectsArray[1] as UnityEngine.UI.Button;
			#endregion (禁止编辑)自动生成绑定
        }
    }
} 