using Game.Base.Logs;
using System;
using System.Collections.Generic;

namespace Game.Base.ObjectPool
{
    /// <summary>
    /// �ػ� HashSet ����
    /// </summary>
    public class PoolHashSet<T> : HashSet<T>, IDisposable
    {
        private bool isUsing;

        public static PoolHashSet<T> Get()
        {
            var result = GlobalObjectPool<PoolHashSet<T>>.Get();

            result.isUsing = true;
            return result;
        }

        public void Dispose()
        {
            Clear();

            if (isUsing)
            {
                GlobalObjectPool<PoolHashSet<T>>.Release(this);
                isUsing = false;
            }
            else
            {
                Logs.Log.Error("����Ļ����˷�ʹ���е� PoolHashSet");
            }
        }
    }
}