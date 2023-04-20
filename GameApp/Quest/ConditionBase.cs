using TableData;
using Game.Base.Logs;
using System;

namespace GameApp.Quest
{
    public abstract class ConditionBase
    {
        protected ConditionConfig ConditionConfig;

        public static ConditionBase Make(int conditionId)
        {
            if (conditionId == 0) // 空条件返回
            {
                return null;
            }

            var conditionConfig = ConditionConfigManager.Instance.GetItem(conditionId);
            if (conditionConfig == null)
            {
                Log.Error($"QuestManager Error 不存在 ConditionConfig ID:{conditionId}");
            }
            var conditionType = QuestHelper.GetConditionType(conditionConfig.ConditionType);
            var condition = Activator.CreateInstance(conditionType) as ConditionBase;
            condition.ConditionConfig = conditionConfig;
            return condition;
        }

        /// <summary>
        /// 返回判断成功的边 ID
        /// </summary>
        public abstract int Check();
    }
}
