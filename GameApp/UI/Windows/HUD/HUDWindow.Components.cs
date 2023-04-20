using GameApp;
using Game.Frame.UI;
using Game.Frame.UI.Core;
using Game.Extern.QuickUI;
using UnityEngine.UI;

namespace GameApp.UI.HUD
{
    [UIWindow("UI/Prefabs/HUD/HUDWindow", LayerType.HUD, WindowMode.Permanent, ClearMode.Default)]
    public partial class HUDWindow : UIWindow
    {
        #region (禁止编辑)自动生成变量
		// HashCode:38
		private UnityEngine.RectTransform trDialogCell;
		private UnityEngine.UI.Text txtDialogCell;
		#endregion (禁止编辑)自动生成变量

        private void InitComponents()
        {
            QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();
            /// 在这里初始化组件

            #region (禁止编辑)自动生成绑定
			trDialogCell = quickUI.UIObjectsArray[0] as UnityEngine.RectTransform;
			txtDialogCell = quickUI.UIObjectsArray[1] as UnityEngine.UI.Text;
			#endregion (禁止编辑)自动生成绑定
        }
    }
} 