using GameApp;
using Game.Frame.UI;
using Game.Frame.UI.Core;
using Game.Extern.QuickUI;
using UnityEngine.UI;

namespace GameApp.UI.System
{
    [UIWindow("UI/Prefabs/System/SceneLoading/SceneLoadingWindow", LayerType.Top, WindowMode.Exclusive, ClearMode.Default)]
    public partial class SceneLoadingWindow : UIWindow
    {
        #region (禁止编辑)自动生成变量
		// HashCode:80
		private UnityEngine.UI.Button btnSceneLoadingWindow;
		private UnityEngine.RectTransform trLoadingIcon;
		private UnityEngine.UI.Text txtGameTips;
		#endregion (禁止编辑)自动生成变量

        private void InitComponents()
        {
            QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();
            /// 在这里初始化组件

            #region (禁止编辑)自动生成绑定
			btnSceneLoadingWindow = quickUI.UIObjectsArray[0] as UnityEngine.UI.Button;
			trLoadingIcon = quickUI.UIObjectsArray[1] as UnityEngine.RectTransform;
			txtGameTips = quickUI.UIObjectsArray[2] as UnityEngine.UI.Text;
			#endregion (禁止编辑)自动生成绑定
        }
    }
} 