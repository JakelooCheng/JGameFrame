using System.Collections;
using System.Collections.Generic;

namespace Game.Base.ObjectPool
{
    /// <summary>
    /// ȫ�ֶ����
    /// </summary>
    /// <typeparam name="T">�ػ�����</typeparam>
    public static class GlobalObjectPool<T> where T : new()
    {
        private static readonly Queue<T> objectQueue = new Queue<T>();

        private static int usedCount = 0;

        /// <summary>
        /// ʹ��ʵ����
        /// </summary>
        public static int UsedCount => usedCount;

        /// <summary>
        /// ����ʣ������
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