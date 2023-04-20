using System;
using System.Collections.Generic;

namespace Game.Frame.Fsm
{
    public interface IFsmManager
    {
        /// <summary>
        /// 创建有限状态机
        /// </summary>
        IFsm<T> CreateFsm<T>(T owner, params FsmStateBase<T>[] states) where T : class;

        /// <summary>
        /// 创建有限状态机
        /// </summary>
        IFsm<T> CreateFsm<T>(T owner, List<FsmStateBase<T>> states) where T : class;

        /// <summary>
        /// 获取有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型</typeparam>
        /// <returns>要获取的有限状态机</returns>
        IFsm<T> GetFsm<T>(int id) where T : class;

        /// <summary>
        /// 获取有限状态机。
        /// </summary>
        FsmBase GetFsm(Type ownerType, int id);

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <returns>是否销毁有限状态机成功。</returns>
        bool DestroyFsm<T>() where T : class;

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        bool DestroyFsm<T>(IFsm<T> fsm) where T : class;

        /// <summary>
        /// 销毁有限状态机
        bool DestroyFsm(FsmBase fsm);

        /// <summary>
        /// 销毁有限状态机
        /// </summary>
        bool DestroyFsm(Type ownerType, int id);
    }
}
