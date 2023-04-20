using Game.Base.LifeCycle;
using System;

namespace Game.Frame.Fsm
{
    public abstract class FsmBase : IUpdate, IShutdown
    {
        public int Id { get; protected set; }

        /// <summary>
        /// 获取有限状态机持有者类型。
        /// </summary>
        public abstract Type OwnerType { get; }

        public bool IsRunning { get; protected set; }

        public abstract void OnUpdate();

        public abstract void Shutdown();
    }
}
