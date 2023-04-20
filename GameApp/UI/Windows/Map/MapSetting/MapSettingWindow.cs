using Game.Frame;
using Game.Frame.PlayerData;
using GameApp;
using GameApp.Cache;
using GameApp.Entitys;
using GameApp.Quest;
using GameApp.UI.HUD;
using GameApp.Utils;
using UnityEngine.UI;

namespace GameApp.UI.Map
{
    public partial class MapSettingWindow
    {
        protected override void OnCreate()
        {
            InitComponents();

            btnContinue.onClick.AddListener(CloseWindow);
            btnSave.onClick.AddListener(OnSaving);
            btnSetting.onClick.AddListener(OnSetting);
            btnQuit.onClick.AddListener(OnQuit);
            btnShutdown.onClick.AddListener(OnShutdownBtn);
        }

        protected override void OnShow()
        {
            CommonUtility.StopPlayer();
        }

        protected override void OnHide()
        {
            CommonUtility.RunPlayer();
        }

        private void OnSaving()
        {
            // 通过设置玩家数据
            var playerData = AppFacade.PlayerData.Player;
            var player = GameCache.Player.PlayerEntity;
            playerData.PlayerPos = player.GetComp<GameObjectComp>().Transform.position;
            playerData.IsDirty = true;

            CommonUtility.Toast("保存成功", $"存档 {playerData.PlayerName}");
        }

        private void OnSetting()
        {
            QuestManager.Instance.StartQuest(100);
        }

        private void OnQuit()
        {
            GameCache.GameState.CurMapScene.LoadState = GameApp.Map.LoadState.Unload;
            AppFacade.PlayerData.QuitGame();
        }

        private void OnShutdownBtn()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}