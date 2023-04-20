using Game.Base.LifeCycle;
using System;

namespace Game.Base.EC
{
    /// <summary>
    /// Entity 接口，支持 LifeCycle
    /// </summary>
    public interface IEntity : IInit, IUpdate, IShutdown
    {
        IEntityManager Manager { get; set; }

        int Id { get; set; }

        string Name { get; set; }

        bool IsActive { get; set; }

        void Start();

        /// <summary>
        /// 添加组件
        /// </summary>
        T AddComp<T>() where T : IComponent, new();

        /// <summary>
        /// 获取某个T类型的组件
        /// </summary>
        T GetComp<T>() where T : IComponent;

        /// <summary>
        /// 获取某个组件
        /// </summary>
        IComponent GetComp(Type name);

        /// <summary>
        /// 移除一个组件
        /// </summary>
        bool RemoveComp<T>() where T : IComponent;
    }
}
