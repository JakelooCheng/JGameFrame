using Game.Base.Logs;
using System;
using System.Collections.Generic;

namespace Game.Base.ObjectPool
{
    /// <summary>
    /// 池化 List 对象
    /// </summary>
    public class PoolList<T> : List<T>, IDisposable
    {
        private bool isUsing;

        public static PoolList<T> Get()
        {
            var result = GlobalObjectPool<PoolList<T>>.Get();

            result.isUsing = true;
            return result;
        }

        public void Dispose()
        {
            Clear();

            if (isUsing)
            {
                GlobalObjectPool<PoolList<T>>.Release(this);
                isUsing = false;
            }
            else
            {
                Logs.Log.Error("错误的回收了非使用中的 PoolList");
            }
        }
    }
}