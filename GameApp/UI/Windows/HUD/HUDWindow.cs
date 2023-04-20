using Game.Base.Logs;
using Game.Frame;
using Game.Frame.Event;
using GameApp;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Base.EC;
using Game.Frame.Timer;
using UnityEngine;
using GameApp.Cache;
using GameApp.Entitys;
using Game.Frame.UI;
using GameApp.Utils;

namespace GameApp.UI.HUD
{
    public partial class HUDWindow
    {
        private LinkedList<DialogData> dialogList = new LinkedList<DialogData>();

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

        private bool isInteraction = false;
        private bool IsInteraction
        {
            set
            {
                if (value != isInteraction)
                {
                    trDialogCell.SetUIActive(!isInteraction);
                }
                isInteraction = value;
            }
        }


        protected override void OnCreate()
        {
            InitComponents();

            AppFacade.Event.Subscribe(EntityMeasureEvent.EventId, OnEntityMeasureEvent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            AppFacade.Event.Unsubscribe(EntityMeasureEvent.EventId, OnEntityMeasureEvent);
        }

        protected override void OnShow()
        {
            UpdateTimer = AppFacade.Timer.Run(0, 0, OnUpdate);
        }

        protected override void OnHide()
        {
            UpdateTimer = null;
        }

        private void OnUpdate()
        {
            if (dialogList.Count <= 0)
            {
                IsInteraction = false;
                return;
            }

            var gameObjectComp = GameCache.Player.PlayerEntity.GetComp<GameObjectComp>();
            if (gameObjectComp == null)
            {
                IsInteraction = false;
                return;
            }

            var playerComp = GameCache.Player.PlayerEntity.GetComp<PlayerComp>();
            if (playerComp == null || !playerComp.IsRunning)
            {
                IsInteraction = false;
                return;
            }

            float scale = 1 - GameCache.Player.CameraMovement.Distance / GameCache.Player.CameraMovement.DistanceRange.y / 2;
            var localPosition = MathUtility.GetLocalPosition(gameObjectComp.Transform.position, RectTransform);
            trDialogCell.localPosition = localPosition + scale * ConstValue.InteractionOffset;
            trDialogCell.localScale = Vector3.one * scale;
            txtDialogCell.text = dialogList.First.Value.Desc;
            IsInteraction = true;

            // 做交互
            if (Input.GetKeyDown(KeyCode.E))
            {
                DoInteraction();

                /// 交互后强制刷新一遍描述
                if (dialogList.Count > 0)
                {
                    var data = dialogList.First.Value;
                    data.InitDesc();
                    dialogList.RemoveFirst();
                    dialogList.AddFirst(data);
                }
            }
        }

        private void DoInteraction()
        {
            GameCache.Interaction.DoInteraction(dialogList.First.Value);
        }

        private void OnEntityMeasureEvent(object from, EventArgsBase e)
        {
            var args = e as EntityMeasureEvent;
            if (args == null || args.Entity == null) return;

            if (args.InteractionType == InteractionType.Default)
            {
                return;
            }

            // 进入范围
            if (args.IsEnterRange)
            {
                var data = new DialogData
                {
                    InteractionType = args.Entity.GetComp<MeasureComp>().InteractionType,
                    Entity = args.Entity
                };
                data.InitDesc();
                dialogList.AddFirst(data);
            }
            else
            {
                foreach (var data in dialogList)
                {
                    if (data.Entity.Id == args.Entity.Id)
                    {
                        dialogList.Remove(data);
                        return;
                    }
                }
            }
        }
    }
}