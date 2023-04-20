using Game.Frame;
using Game.Frame.Timer;
using GameApp;
using UnityEngine;
using UnityEngine.UI;
using TableData;

namespace GameApp.UI.System
{
    public partial class SceneLoadingWindow
    {
        private Vector3 tempVector;
        private ITimer timer;
        private ITimer UpdateTimer
        {
            set
            {
                if (timer != null)
                {
                    AppFacade.Timer.Cancel(ref timer);
                    timer = null;
                }
                timer = value;
            }
        }

        protected override void OnCreate()
        {
            InitComponents();
        }

        protected override void OnShow()
        {
            tempVector = Vector3.zero;
            trLoadingIcon.localRotation = Quaternion.identity;
            UpdateTimer = AppFacade.Timer.Run(0, 0, UpdateLoadingImage);
            FlushTips();
        }

        protected override void OnHide()
        {
            UpdateTimer = null;
        }

        private void FlushTips()
        {
            var tipsArray = LoadingTipsConfigManager.Instance.ItemArray.Items;
            string tips = tipsArray[Random.Range(0, tipsArray.Count - 1)].Tips;
            txtGameTips.text = tips;
        }

        private void UpdateLoadingImage()
        {
            tempVector.z = UnityEngine.Time.deltaTime * -100;
            trLoadingIcon.Rotate(tempVector);
        }
    }
}