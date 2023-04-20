using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Base.ObjectPool
{
    /// <summary>
    /// 清理全局对象池
    /// </summary>
    public static class GlobalObjectPoolCleaner
    {
        private static Dictionary<Type, Action> needClearPools = new Dictionary<Type, Action>();

        /// <summary>
        /// 已注册的池子数量
        /// </summary>
        public static int RegisteredPoolCount => needClearPools.Count;

        public static void ClearAllGobalPool()
        {
            foreach (var pool in needClearPools)
            {
                pool.Value?.Invoke();
            }
        }

        public static void RegisterPoolCleaner(Type poolType, Action clearAction)
        {
            needClearPools.Add(poolType, clearAction);
        }
    }
}