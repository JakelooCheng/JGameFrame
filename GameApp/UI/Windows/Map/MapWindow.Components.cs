using GameApp;
using Game.Frame.UI;
using Game.Frame.UI.Core;
using Game.Extern.QuickUI;
using UnityEngine.UI;

namespace GameApp.UI.Map
{
    [UIWindow("UI/Prefabs/Map/MapWindow", LayerType.Form, WindowMode.Exclusive, ClearMode.Default)]
    public partial class MapWindow : UIWindow
    {
        #region (禁止编辑)自动生成变量
		// HashCode:10
		private UnityEngine.UI.Button btnSetting;
		#endregion (禁止编辑)自动生成变量

        private void InitComponents()
        {
            QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();
            /// 在这里初始化组件

            #region (禁止编辑)自动生成绑定
			btnSetting = quickUI.UIObjectsArray[0] as UnityEngine.UI.Button;
			#endregion (禁止编辑)自动生成绑定
        }
    }
} 