using GameApp;
using GameApp.Cache;
using GameApp.Entitys;
using GameApp.Utils;
using System;
using TableData;
using UnityEngine.UI;

namespace GameApp.UI.System
{
    public class RewardWindowArgs
    {
        public int ItemId;
        public int Count;
        public Action CloseCallback;
    }

    public partial class RewardWindow
    {
        private RewardWindowArgs CurArgs;

        private ItemConfig itemConfig;

        protected override void OnCreate()
        {
            InitComponents();

            btnClose.onClick.AddListener(CloseWindow);
        }

        protected override void OnShow()
        {
            CurArgs = Data as RewardWindowArgs;

            itemConfig = ItemConfigManager.Instance.GetItem(CurArgs.ItemId);

            CommonUtility.StopCamera();
            CommonUtility.StopPlayer();
        }

        protected override void OnHide()
        {
            CommonUtility.Toast("获得物品", $"{itemConfig.Name} × {CurArgs.Count}");
            CommonUtility.RunPlayer();
            CommonUtility.RunCamera();

            CurArgs.CloseCallback?.Invoke();
        }
    }
}