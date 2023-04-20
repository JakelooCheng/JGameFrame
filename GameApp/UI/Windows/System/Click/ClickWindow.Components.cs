using GameApp;
using Game.Frame.UI;
using Game.Frame.UI.Core;
using Game.Extern.QuickUI;
using UnityEngine.UI;

namespace GameApp.UI.System
{
    [UIWindow("UI/Prefabs/System/Click/ClickWindow", LayerType.Top, WindowMode.Permanent, ClearMode.Default)]
    public partial class ClickWindow : UIWindow
    {
        #region (禁止编辑)自动生成变量
		// HashCode:11
		private UnityEngine.GameObject goClickTemp;
		#endregion (禁止编辑)自动生成变量

        private void InitComponents()
        {
            QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();
            /// 在这里初始化组件

            #region (禁止编辑)自动生成绑定
			goClickTemp = quickUI.UIObjectsArray[0] as UnityEngine.GameObject;
			#endregion (禁止编辑)自动生成绑定
        }
    }
} 