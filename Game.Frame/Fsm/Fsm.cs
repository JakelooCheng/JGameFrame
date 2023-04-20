using Game.Base.LifeCycle;
using Game.Base.Logs;
using Game.Base.ObjectPool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

namespace Game.Frame.Fsm
{
    /// <summary>
    /// 有限状态机，基于 GameFramework
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型。</typeparam>
    internal sealed class Fsm<T> : FsmBase, IFsm<T> where T : class
    {
        private readonly Dictionary<Type, FsmStateBase<T>> statesDic = new Dictionary<Type, FsmStateBase<T>>();

        public override Type OwnerType => typeof(T);

        public T Owner { get; private set; }
        public FsmStateBase<T> CurrentState { get; private set; }

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        public static Fsm<T> Make(int id, T owner, List<FsmStateBase<T>> states)
        {
            Fsm<T> fsm = GlobalObjectPool<Fsm<T>>.Get();
            fsm.Id = id;
            fsm.Owner = owner;
            foreach (FsmStateBase<T> state in states)
            {
                Type stateType = state.GetType();
                if (fsm.statesDic.ContainsKey(stateType))
                {
                    Log.Error($"Fsm Error 重复添加状态 {stateType}");
                }

                fsm.statesDic.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        /// <summary>
        /// 开始有限状态机。
        /// </summary>
        /// <typeparam name="TState">要开始的有限状态机状态类型。</typeparam>
        public void Start<TState>() where TState : FsmStateBase<T>
        {
            Start(typeof(TState));
        }

        /// <summary>
        /// 开始有限状态机。
        /// </summary>
        /// <param name="stateType">要开始的有限状态机状态类型。</param>
        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                Log.Error($"Fsm Error 状态机不可重复运行！");
                return;
            }

            FsmStateBase<T> state = GetState(stateType);

            CurrentState = state;
            CurrentState.OnEnter(this);
        }

        /// <summary>
        /// 清理有限状态机。
        /// </summary>
        public void Clear()
        {
            IsRunning = false;
            if (CurrentState != null)
            {
                CurrentState.OnLeave(this, true);
                CurrentState = null;
            }

            foreach (var state in statesDic.Values)
            {
                state.OnDestroy(this);
            }

            statesDic.Clear();
        }

        public override void Shutdown()
        {
            Clear();
            GlobalObjectPool<Fsm<T>>.Release(this);
        }

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要获取的有限状态机状态类型。</typeparam>
        /// <returns>要获取的有限状态机状态。</returns>
        public TState GetState<TState>() where TState : FsmStateBase<T>
        {
            return GetState(typeof(TState)) as TState;
        }

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        /// <param name="stateType">要获取的有限状态机状态类型。</param>
        /// <returns>要获取的有限状态机状态。</returns>
        public FsmStateBase<T> GetState(Type stateType)
        {
            if (statesDic.TryGetValue(stateType, out FsmStateBase<T> state))
            {
                return state;
            }
            else
            {
                Log.Error($"Fsm Error 状态机不存在状态 {stateType}！");
            }
            return null;
        }

        /// <summary>
        /// 有限状态机轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public override void OnUpdate()
        {
            if (CurrentState == null)
            {
                return;
            }

            CurrentState.OnUpdate(this);
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要切换到的有限状态机状态类型。</typeparam>
        internal void ChangeState<TState>() where TState : FsmStateBase<T>
        {
            ChangeState(typeof(TState));
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <param name="stateType">要切换到的有限状态机状态类型。</param>
        internal void ChangeState(Type stateType)
        {
            if (CurrentState == null)
            {
                Log.Error($"Fsm Error 状态机未运行！");
            }

            FsmStateBase<T> state = GetState(stateType);
            CurrentState.OnLeave(this, false);
            CurrentState = state;
            CurrentState.OnEnter(this);
        }
    }
}
