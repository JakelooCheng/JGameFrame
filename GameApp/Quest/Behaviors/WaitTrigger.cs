using Game.Frame;
using Game.Frame.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameApp.Quest
{
    /// <summary>
    /// 等待触发
    /// </summary>
    public class WaitTrigger : BehaviorBase
    {
        private int WaitingId;

        private QuestTriggerEvent.TriggerType WaitingType;

        public override void Excute()
        {
            WaitingType = (QuestTriggerEvent.TriggerType)BehaviorConfig.IntArg1;
            WaitingId = BehaviorConfig.IntArg2;
            AppFacade.Event.Subscribe(QuestTriggerEvent.EventId, OnMessage);
        }

        private void OnMessage(object sender, EventArgsBase e)
        {
            var arg = e as QuestTriggerEvent;
            if (arg != null)
            {
                if (arg.Target == WaitingId)
                {
                    Finish();
                    AppFacade.Timer.Wait(0, () =>
                    {
                        AppFacade.Event.Unsubscribe(QuestTriggerEvent.EventId, OnMessage);
                    });
                }
            }
        }
    }
}