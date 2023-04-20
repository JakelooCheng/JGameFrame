using System;

namespace Game.Frame.Fsm
{
    /// <summary>
    /// 有限状态机状态基类，基于 GameFramework
    /// </summary>
    public abstract class FsmStateBase<T> where T : class
    {
        /// <summary>
        /// 有限状态机状态初始化时调用
        /// </summary>
        public virtual void OnInit(IFsm<T> fsm)
        {
        }

        /// <summary>
        /// 有限状态机状态进入时调用
        /// </summary>
        public virtual void OnEnter(IFsm<T> fsm)
        {
        }

        /// <summary>
        /// 有限状态机状态轮询时调用。
        /// </summary>
        public virtual void OnUpdate(IFsm<T> fsm)
        {
        }

        /// <summary>
        /// 有限状态机状态离开时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        /// <param name="isShutdown">是否是关闭有限状态机时触发。</param>
        public virtual void OnLeave(IFsm<T> fsm, bool isShutdown)
        {
        }

        /// <summary>
        /// 有限状态机状态销毁时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        public virtual void OnDestroy(IFsm<T> fsm)
        {
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要切换到的有限状态机状态类型。</typeparam>
        /// <param name="fsm">有限状态机引用。</param>
        public void ChangeState<TState>(IFsm<T> fsm) where TState : FsmStateBase<T>
        {
            ChangeState(fsm, typeof(TState));
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        /// <param name="stateType">要切换到的有限状态机状态类型。</param>
        public void ChangeState(IFsm<T> fsm, Type stateType)
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            fsmImplement.ChangeState(stateType);
        }
    }
}
