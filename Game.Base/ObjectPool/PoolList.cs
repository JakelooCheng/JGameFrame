using Game.Base.Logs;
using System;
using System.Collections.Generic;

namespace Game.Base.ObjectPool
{
    /// <summary>
    /// �ػ� List ����
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
                Logs.Log.Error("����Ļ����˷�ʹ���е� PoolList");
            }
        }
    }
}