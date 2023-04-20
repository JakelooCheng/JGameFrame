using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Base.ObjectPool
{
    /// <summary>
    /// ����ȫ�ֶ����
    /// </summary>
    public static class GlobalObjectPoolCleaner
    {
        private static Dictionary<Type, Action> needClearPools = new Dictionary<Type, Action>();

        /// <summary>
        /// ��ע��ĳ�������
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