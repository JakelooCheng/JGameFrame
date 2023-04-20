using UnityEngine;
using System.Collections.Generic;
using Game.Base.ObjectPool;

namespace Game.Base.Data
{
    public enum DataFrom
    {
        Default = 0,
        UI = 1
    }

    /// <summary>
    /// 挂在 GameObject 上的
    /// </summary>
    public class GameObjectDataMono : MonoBehaviour
    {
        private PoolDictionary<int, Object> dataDic;

        public void SetData(DataFrom from, Object data)
        {
            if (dataDic == null)
            {
                dataDic = PoolDictionary<int, Object>.Get();
            }

            if (data == null) // 为空时默认是在删除的
            {
                DeleteData(from);
            }
            else
            {
                dataDic[(int)from] = data;
            }
        }

        public void DeleteData(DataFrom from)
        {
            dataDic.Remove((int)from);
            if (dataDic.Count == 0)
            {
                dataDic.Dispose();
                dataDic = null;
            }
        }

        public T GetData<T>(DataFrom from) where T : class
        {
            return GetData(from) as T;
        }

        public Object GetData(DataFrom from)
        {
            if (dataDic == null)
            {
                return null;
            }

            return dataDic[(int)from];
        }

        private void OnDestroy()
        {
            dataDic.Dispose();
        }
    }
}
