using Game.Frame;
using GameApp;
using UnityEngine.UI;

namespace GameApp.UI.Map
{
    public partial class MapWindow
    {
        protected override void OnCreate()
        {
            InitComponents();

            btnSetting.onClick.AddListener(OnSettingBtnClick);
        }

        protected override void OnShow()
        {

        }

        private void OnSettingBtnClick()
        {
            AppFacade.UI.OpenWindow<MapSettingWindow>();
        }
    }
}