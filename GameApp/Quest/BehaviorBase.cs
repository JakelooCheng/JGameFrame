using Game.Base.Logs;
using System;
using System.Collections;
using System.Collections.Generic;
using TableData;

namespace GameApp.Quest
{
    public class BehaviorBase
    {
        protected BehaviorConfig BehaviorConfig;

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinish { get; protected set; }


        /// <summary>
        /// 构建行为
        /// </summary>
        public static BehaviorBase Make(int behaviorConfigId)
        {
            var behaviorConfig = BehaviorConfigManager.Instance.GetItem(behaviorConfigId);
            if (behaviorConfig == null)
            {
                Log.Error($"QuestManager Error 不存在 BehaviorConfig ID:{behaviorConfigId}");
            }
            var behaviorType = QuestHelper.GetBehaviorType(behaviorConfig.BehaviorType);
            var behavior = Activator.CreateInstance(behaviorType) as BehaviorBase;
            behavior.BehaviorConfig = behaviorConfig;
            behavior.Excute();
            return behavior;
        }

        /// <summary>
        /// 执行时
        /// </summary>
        public virtual void Excute()
        {

        }

        public virtual void Finish()
        {
            if (IsFinish) { return; }

            IsFinish = true;
        }
    }
}