using Game.Base.Logs;
using System;
using System.Collections.Generic;

namespace Game.Base.ObjectPool
{
    /// <summary>
    /// 池化 Dictionary 对象
    /// </summary>
    public class PoolDictionary<K, V> : Dictionary<K, V>, IDisposable
    {
        private bool isUsing;

        public static PoolDictionary<K, V> Get()
        {
            var result = GlobalObjectPool<PoolDictionary<K, V>>.Get();

            result.isUsing = true;
            return result;
        }

        public void Dispose()
        {
            Clear();

            if (isUsing)
            {
                GlobalObjectPool<PoolDictionary<K, V>>.Release(this);
                isUsing = false;
            }
            else
            {
                Logs.Log.Error("错误的回收了非使用中的 PoolDictionary");
            }
        }
    }
}