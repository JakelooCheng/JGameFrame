using System;

namespace Game.Frame.Fsm
{
    /// <summary>
    /// 有限状态机接口，基于 GameFramework
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型。</typeparam>
    public interface IFsm<T> where T : class
    {
        /// <summary>
        /// 有限状态机 Id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 获取有限状态机持有者
        /// </summary>
        T Owner { get; }

        /// <summary>
        /// 获取当前有限状态机状态
        /// </summary>
        FsmStateBase<T> CurrentState { get; }

        /// <summary>
        /// 是否正在运行
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 开始有限状态机
        /// </summary>
        void Start<TState>() where TState : FsmStateBase<T>;

        /// <summary>
        /// 开始有限状态机 stateType : FsmState<T>
        /// </summary>
        void Start(Type stateType);

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要获取的有限状态机状态类型。</typeparam>
        /// <returns>要获取的有限状态机状态。</returns>
        TState GetState<TState>() where TState : FsmStateBase<T>;

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        /// <param name="stateType">要获取的有限状态机状态类型。</param>
        /// <returns>要获取的有限状态机状态。</returns>
        FsmStateBase<T> GetState(Type stateType);
    }
}
