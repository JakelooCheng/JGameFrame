using GameApp;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Frame.PlayerData;
using Game.Frame;
using Game.Frame.UI;

namespace GameApp.UI.System
{
    public partial class PlayerDataWindow
    {
        private List<PlayerDataInfo> playerDatas = new List<PlayerDataInfo>();

        protected override void OnCreate()
        {
            InitComponents();

            btnClose.onClick.AddListener(CloseWindow);
            btnCreateNew.onClick.AddListener(ShowCreateNewPanel);
            btnClosePanel.onClick.AddListener(CloseCreateNewPanel);
            btnConfirm.onClick.AddListener(ConfirmNewData);
        }

        protected override void OnShow()
        {
            CloseCreateNewPanel();
            FlushPlayerData();
        }

        private void FlushPlayerData()
        {
            playerDatas.Clear();
            AppFacade.PlayerData.GetAllInfos(playerDatas);
            playerDataCells.ReleaseAll();
            foreach (var data in playerDatas)
            {
                playerDataCells.GetCell().SetData(data);
            }
        }

        private void ShowCreateNewPanel()
        {
            trCreateNewPanel.SetUIActive(true);
            btnClose.SetUIActive(false);
        }

        private void CloseCreateNewPanel()
        {
            trCreateNewPanel.SetUIActive(false);
            compInput.text = string.Empty;
            btnClose.SetUIActive(true);
        }

        private void ConfirmNewData()
        {
            if (compInput.text == string.Empty)
            {
                return; //TODO 需要个 Toast 系统
            }

            AppFacade.PlayerData.Create(compInput.text);

            CloseCreateNewPanel();
            FlushPlayerData();
        }
    }
}