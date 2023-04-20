using Game.Base.Logs;
using System;
using System.Collections;
using System.Collections.Generic;
using TableData;

namespace GameApp.Quest
{
    public static class QuestHelper
    {
        private static Dictionary<QuestBehaviorType, Type> behaviorTypeCache
            = new Dictionary<QuestBehaviorType, Type>();

        private static Dictionary<QuestConditionType, Type> conditionTypeCache
            = new Dictionary<QuestConditionType, Type>();

        public static void Init()
        {
            if (behaviorTypeCache.Count > 0)
            {
                return;
            }    
            behaviorTypeCache.Add(QuestBehaviorType.Dialog, typeof(DialogBehavior));
            behaviorTypeCache.Add(QuestBehaviorType.WaitTrigger, typeof(WaitTrigger));
            behaviorTypeCache.Add(QuestBehaviorType.Drop, typeof(DropBehavior));
            behaviorTypeCache.Add(QuestBehaviorType.ShowHideEntity, typeof(ShowOrHideEntity));
        }

        /// <summary>
        /// 获取行为实例类型
        /// </summary>
        public static Type GetBehaviorType(QuestBehaviorType type)
        {
            return behaviorTypeCache[type];
        }

        /// <summary>
        /// 获取条件实例类型
        /// </summary>
        public static Type GetConditionType(QuestConditionType type)
        {
            return conditionTypeCache[type];
        }
    }
}