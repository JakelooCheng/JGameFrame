using GameApp.Cache;

namespace GameApp.Quest
{
    /// <summary>
    /// 是否有物品条件判断
    /// </summary>
    public class HasItemCondition : ConditionBase
    {
        public override int Check()
        {
            /// 直接判断大于指定值
            if (ConditionConfig.JumpEdges.Count > 1)
            {
                int compare = ConditionConfig.IntArg2;
                /// 判断有无
                if (compare != 0)
                {
                    return ConditionConfig.JumpEdges[GameCache.Bag.HasItem(ConditionConfig.IntArg1) ? 0 : 1];
                }
                /// 判断数量
                else
                {
                    return ConditionConfig.JumpEdges[GameCache.Bag.GetCount(ConditionConfig.IntArg1) >= compare ? 0 : 1];
                }
            }
            /// 等待条件满足
            else
            {
                if (GameCache.Bag.HasItem(ConditionConfig.IntArg1))
                {
                    return ConditionConfig.JumpEdges[0];
                }
            }
            return 0;
        }
    }
}