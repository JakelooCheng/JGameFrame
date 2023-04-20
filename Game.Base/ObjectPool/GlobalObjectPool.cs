using System.Collections;
using System.Collections.Generic;

namespace Game.Base.ObjectPool
{
    /// <summary>
    /// 全局对象池
    /// </summary>
    /// <typeparam name="T">池化对象</typeparam>
    public static class GlobalObjectPool<T> where T : new()
    {
        private static readonly Queue<T> objectQueue = new Queue<T>();

        private static int usedCount = 0;

        /// <summary>
        /// 使用实例数
        /// </summary>
        public static int UsedCount => usedCount;

        /// <summary>
        /// 池内剩余数量
        /// </summary>
        private static int PoolCount => objectQueue.Count;

        static GlobalObjectPool()
        {
            GlobalObjectPoolCleaner.RegisterPoolCleaner(typeof(T), Clear);
        }

        public static void Init(int initCount)
        {
            for (int i = 0; i < initCount; i++)
            {
                objectQueue.Enqueue(new T());
            }
        }

        public static T Get()
        {
            usedCount++;
            if (objectQueue.Count == 0)
            {
                return new T();
            }
            else
            {
                return objectQueue.Dequeue();
            }
        }

        public static void Release(T obj)
        {
            usedCount--;
            objectQueue.Enqueue(obj);
        }

        public static void Clear()
        {
            objectQueue.Clear();
        }
    }
}