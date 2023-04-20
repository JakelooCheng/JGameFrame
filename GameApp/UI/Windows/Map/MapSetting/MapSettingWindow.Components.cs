using GameApp;
using Game.Frame.UI;
using Game.Frame.UI.Core;
using Game.Extern.QuickUI;
using UnityEngine.UI;

namespace GameApp.UI.Map
{
    [UIWindow("UI/Prefabs/Map/MapSetting/MapSettingWindow", LayerType.Form, WindowMode.Exclusive, ClearMode.Default)]
    public partial class MapSettingWindow : UIWindow
    {
        #region (禁止编辑)自动生成变量
		// HashCode:143
		private UnityEngine.UI.Button btnSave;
		private UnityEngine.UI.Button btnSetting;
		private UnityEngine.UI.Button btnContinue;
		private UnityEngine.UI.Button btnQuit;
		private UnityEngine.UI.Button btnShutdown;
		#endregion (禁止编辑)自动生成变量

        private void InitComponents()
        {
            QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();
            /// 在这里初始化组件

            #region (禁止编辑)自动生成绑定
			btnSave = quickUI.UIObjectsArray[0] as UnityEngine.UI.Button;
			btnSetting = quickUI.UIObjectsArray[1] as UnityEngine.UI.Button;
			btnContinue = quickUI.UIObjectsArray[2] as UnityEngine.UI.Button;
			btnQuit = quickUI.UIObjectsArray[3] as UnityEngine.UI.Button;
			btnShutdown = quickUI.UIObjectsArray[4] as UnityEngine.UI.Button;
			#endregion (禁止编辑)自动生成绑定
        }
    }
} 