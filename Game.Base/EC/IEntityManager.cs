using Game.Base.EC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Base.EC
{
    public interface IEntityManager
    {
        /// <summary>
        /// 正在运行
        /// </summary>
        bool Running { get; }

        /// <summary>
        /// 加入场景
        /// </summary>
        void Add(Entity entity);

        /// <summary>
        /// 移除出场景
        /// </summary>
        void Remove(Entity entity);
    }
}