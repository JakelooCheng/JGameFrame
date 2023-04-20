using Game.Frame;
using Game.Frame.Timer;
using Game.Frame.UI;
using GameApp;
using GameApp.Cache;
using GameApp.Entitys;
using GameApp.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameApp.UI.HUD
{
    public class DialogArgs
    {
        public string Dialog;

        public MapUnitComp TargetComp;

        public Action CloseCallback;
    }

    public partial class DialogWindow
    {
        private DialogArgs curDialogArgs;

        private float curDialogOffset;

        protected override void OnCreate()
        {
            InitComponents();

            btnClose.onClick.AddListener(CloseWindow);
        }

        protected override void OnShow()
        {
            curDialogArgs = Data as DialogArgs;
            txtDialogCell.text = curDialogArgs.Dialog;

            LayoutRebuilder.ForceRebuildLayoutImmediate(trDialogCell);

            if (curDialogArgs.TargetComp is IDialogAble dialogAble)
            {
                dialogAble.OnDialog();
                curDialogOffset = dialogAble.DialogOffset;
            }
            else
            {
                curDialogOffset = ConstValue.DialogOffset;
            }

            CommonUtility.StopPlayer();
            CommonUtility.StopCamera();

            UpdateTimer = AppFacade.Timer.Run(0, 0, OnUpdateLocation);
            trDialogCell.SetUIActive(true);
        }

        /// <summary>
        /// 更新对话框位置
        /// </summary>
        private void OnUpdateLocation()
        {
            var gameObjectComp = curDialogArgs.TargetComp.GetComp<GameObjectComp>();
            if (gameObjectComp == null)
            {
                CloseWindow();
                return;
            }

            //// 按 E 也可以结束交互
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    CloseWindow();
            //}

            var localPosition = MathUtility.GetLocalPosition(gameObjectComp.Transform.position + new Vector3(0, curDialogOffset, 0), RectTransform);
            trDialogCell.localPosition = localPosition;
        }

        protected override void OnHide()
        {
            trDialogCell.SetUIActive(false);
            CommonUtility.RunPlayer();
            CommonUtility.RunCamera();
            UpdateTimer = null;
            curDialogArgs.CloseCallback?.Invoke();
        }
    }
}