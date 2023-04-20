using GameApp;
using Game.Frame.UI;
using Game.Frame.UI.Core;
using Game.Extern.QuickUI;
using UnityEngine.UI;
using UnityEngine;
using Game.Frame.PlayerData;
using Game.Frame;

namespace GameApp.UI.System
{
    [UIWindow("UI/Prefabs/System/PlayerData/PlayerDataWindow", LayerType.Form, WindowMode.Inclusive, ClearMode.Default)]
    public partial class PlayerDataWindow : UIWindow
    {
        #region (禁止编辑)自动生成变量
		// HashCode:419
		private UnityEngine.UI.Button btnCreateNew;
		private UnityEngine.UI.ScrollRect scrollPlayerDataList;
		private UnityEngine.RectTransform trContent;
		private UnityEngine.UI.Button btnClose;
		private UnityEngine.RectTransform trCreateNewPanel;
		private UnityEngine.UI.InputField compInput;
		private UnityEngine.UI.Button btnConfirm;
		private UnityEngine.UI.Button btnClosePanel;
		#endregion (禁止编辑)自动生成变量

		private UICells<PlayerDataCell> playerDataCells = new UICells<PlayerDataCell>();

        private void InitComponents()
        {
            QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();
            /// 在这里初始化组件

            #region (禁止编辑)自动生成绑定
			btnCreateNew = quickUI.UIObjectsArray[0] as UnityEngine.UI.Button;
			scrollPlayerDataList = quickUI.UIObjectsArray[1] as UnityEngine.UI.ScrollRect;
			trContent = quickUI.UIObjectsArray[2] as UnityEngine.RectTransform;
			btnClose = quickUI.UIObjectsArray[3] as UnityEngine.UI.Button;
			trCreateNewPanel = quickUI.UIObjectsArray[4] as UnityEngine.RectTransform;
			compInput = quickUI.UIObjectsArray[5] as UnityEngine.UI.InputField;
			btnConfirm = quickUI.UIObjectsArray[6] as UnityEngine.UI.Button;
			btnClosePanel = quickUI.UIObjectsArray[7] as UnityEngine.UI.Button;
			#endregion (禁止编辑)自动生成绑定

			playerDataCells.Init(trContent);
        }

		private class PlayerDataCell : UICell
		{
			private Text title;
			private Text time;

			private PlayerDataInfo dataInfo;

            public override void Init(Transform root)
			{
				title = root.GetChild(0).GetComponent<Text>();
                time = root.GetChild(1).GetComponent<Text>();
				root.GetComponent<Button>().onClick.AddListener(StartGame);
            }

			public void SetData(PlayerDataInfo dataInfo)
			{
				this.dataInfo = dataInfo;
                this.title.text = dataInfo.Name;
				this.time.text = dataInfo.SavedTime.ToString("D");
            }

            public void StartGame()
            {
				AppFacade.UI.CloseWindow<PlayerDataWindow>();
				AppFacade.PlayerData.StartGame(dataInfo);
            }
        }
	}
} 