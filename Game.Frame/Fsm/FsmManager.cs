using Game.Base.LifeCycle;
using Game.Base.Module;
using System;
using System.Collections.Generic;

namespace Game.Frame.Fsm
{
    public class FsmManager : GameModuleBase, IUpdate, IShutdown
    {
        private readonly Dictionary<(int, Type), FsmBase> fsmDic = new Dictionary<(int, Type), FsmBase>();
        private int instanceId = 0;

        /// <summary>
        /// 有限状态机管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void OnUpdate()
        {
            if (fsmDic.Count <= 0)
            {
                return;
            }

            foreach (var fsm in fsmDic.Values)
            {
                if (fsm.IsRunning)
                {
                    continue;
                }
                fsm.OnUpdate();
            }
        }

        /// <summary>
        /// 关闭并清理有限状态机管理器。
        /// </summary>
        public void Shutdown()
        {
            foreach (var fsm in fsmDic.Values)
            {
                fsm.Shutdown();
            }

            fsmDic.Clear();
        }

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        public IFsm<T> CreateFsm<T>(T owner, params FsmStateBase<T>[] states) where T : class
        {
            return CreateFsm<T>(owner, new List<FsmStateBase<T>>(states));
        }

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        public IFsm<T> CreateFsm<T>(T owner, List<FsmStateBase<T>> states) where T : class
        {
            int id = instanceId++;
            Fsm<T> fsm = Fsm<T>.Make(id, owner, states);
            fsmDic.Add((id, typeof(T)), fsm);
            return fsm;
        }

        /// <summary>
        /// 获取有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <returns>要获取的有限状态机。</returns>
        public IFsm<T> GetFsm<T>(int id) where T : class
        {
            return (IFsm<T>)GetFsm(typeof(T), id);
        }

        /// <summary>
        /// 获取有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <param name="name">有限状态机名称。</param>
        /// <returns>要获取的有限状态机。</returns>
        public FsmBase GetFsm(Type ownerType, int id)
        {
            if (!fsmDic.TryGetValue((id, ownerType), out var fsm))
            {
                fsm = null;
            }
            return fsm;
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        public bool DestroyFsm<T>(IFsm<T> fsm) where T : class
        {
            return DestroyFsm(typeof(T), fsm.Id);
        }

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        public bool DestroyFsm(FsmBase fsm)
        {
            return DestroyFsm(fsm.OwnerType, fsm.Id);
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        public bool DestroyFsm(Type ownerType, int id)
        {
            var key = (id, ownerType);
            if (fsmDic.TryGetValue(key, out FsmBase fsm))
            {
                fsm.Shutdown();
                return fsmDic.Remove(key);
            }
            return false;
        }
    }
}
