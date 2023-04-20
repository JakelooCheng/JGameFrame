using Game.Frame;
using GameApp;
using GameApp.UI.System;
using UnityEngine.UI;

namespace GameApp.UI.Start
{
    public partial class StartWindow
    {
        protected override void OnCreate()
        {
            InitComponents();

            btnStart.onClick.AddListener(OpenPlayerDataWindow);
            btnOpenSetting.onClick.AddListener(OpenSettingWindow);
        }

        protected override void OnShow()
        {

        }

        private void OpenPlayerDataWindow()
        {
            AppFacade.UI.OpenWindow<PlayerDataWindow>();
        }

        private void OpenSettingWindow()
        {
            // AppFacade.UI.OpenWindow<SettingWindow>();
        }
    }
}