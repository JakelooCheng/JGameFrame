using GameApp;
using Game.Frame.UI;
using Game.Frame.UI.Core;
using Game.Extern.QuickUI;
using UnityEngine.UI;
using Game.Frame.Timer;
using Game.Frame;

namespace GameApp.UI.HUD
{
    [UIWindow("UI/Prefabs/HUD/DialogWindow", LayerType.HUD, WindowMode.Inclusive, ClearMode.Default)]
    public partial class DialogWindow : UIWindow
    {
        #region (禁止编辑)自动生成变量
		// HashCode:62
		private UnityEngine.RectTransform trDialogCell;
		private UnityEngine.UI.Text txtDialogCell;
		private UnityEngine.UI.Button btnClose;
		#endregion (禁止编辑)自动生成变量

        private void InitComponents()
        {
            QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();
            /// 在这里初始化组件

            #region (禁止编辑)自动生成绑定
			trDialogCell = quickUI.UIObjectsArray[0] as UnityEngine.RectTransform;
			txtDialogCell = quickUI.UIObjectsArray[1] as UnityEngine.UI.Text;
			btnClose = quickUI.UIObjectsArray[2] as UnityEngine.UI.Button;
			#endregion (禁止编辑)自动生成绑定
        }

        private ITimer updateTimer;
        private ITimer UpdateTimer
        {
            set
            {
                if (updateTimer != null)
                {
                    AppFacade.Timer.Cancel(ref updateTimer);
                    updateTimer = null;
                }
                updateTimer = value;
            }
        }
    }
} 