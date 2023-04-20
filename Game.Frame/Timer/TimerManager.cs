using Game.Base.Module;
using Game.Base.LifeCycle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Base.Async;

namespace Game.Frame.Timer
{
    /// <summary>
    /// 计时器，提供定时、Update、延后执行等。
    /// </summary>
    public class TimerManager : GameModuleBase, IInit, IUpdate, IClear, IShutdown
    {
        private readonly List<Timer> timers = new List<Timer>();
        private readonly List<Timer> timerWillAdd = new List<Timer>();
        private readonly List<ITimer> timerWillRemove = new List<ITimer>();
        private bool isOnUpdate = false;

        public Time Time { get; private set; }

        public void Init()
        {
            Time = new Time();
        }

        public void OnUpdate()
        {
            // 添加帧后添加的
            if (timerWillAdd.Count > 0)
            {
                timers.AddRange(timerWillAdd);
                timerWillAdd.Clear();
            }
            isOnUpdate = true;

            for (var index = timers.Count - 1; index >= 0; index--)
            {
                var timer = timers[index];

                if (timer.IsInvalid)
                {
                    timer.OnUpdate(Time);
                }

                if (!isOnUpdate)
                {
                    return; // Update中被Shutdown
                }

                if (!timer.IsInvalid || timer.LoopedTime >= timer.LoopTime)
                {
                    timer.Release();
                    timers.RemoveAt(index);
                }
            }

            isOnUpdate = false;

            // 移除帧内移除的
            foreach (var remove in timerWillRemove)
            {
                Remove(remove);
            }
            timerWillRemove.Clear();
        }

        public void Clear()
        {
            isOnUpdate = false;
            foreach (var timer in timers)
            {
                timer.Release();
            }
            foreach (var timer in timerWillAdd)
            {
                timer.Release();
            }
            timers.Clear();
            timerWillAdd.Clear();
            timerWillAdd.Clear();
        }

        public void Shutdown()
        {
            Clear();
            Time = null;
        }

        #region Private Functions
        private ITimer Add(int intervalMS, int loopTime, bool isScaled, Action act)
        {
            return Add(intervalMS, loopTime, isScaled, timer => act?.Invoke());
        }

        private ITimer Add(int intervalMS, int loopTime, bool isScaled, Action<ITimer> act)
        {
            var timer = Timer.Make(Time, intervalMS, loopTime, isScaled, act);

            if (isOnUpdate)
            {
                timerWillAdd.Add(timer);
            }
            else
            {
                timers.Add(timer);
            }
            return timer;
        }


        /// <summary>
        /// 用 ID 查找 Timer
        /// </summary>
        /// <returns></returns>
        private int Find(long id)
        {
            for (var index = 0; index < timers.Count; index++)
            {
                if (timers[index].Id == id)
                {
                    return index;
                }
            }
            return -1;
        }

        private bool Remove(ITimer timer)
        {
            var index = Find(timer.Id);
            if (index >= 0)
            {
                if (isOnUpdate)
                {
                    timerWillRemove.Add(timer);
                }
                else
                {
                    timers.RemoveAt(index);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// 运行一个一次性的计时器
        /// </summary>
        /// <param name="intervalMS">延迟时间：毫秒</param>
        /// <param name="act">回调</param>
        /// <returns></returns>
        public ITimer Wait(int intervalMS, Action act)
        {
            return Add(intervalMS, 1, false, act);
        }

        /// <summary>
        /// 运行一个一次性的计时器
        /// </summary>
        /// <param name="intervalMS">延迟时间：毫秒</param>
        /// <param name="isScaled">是否被时间缩放影响</param>
        /// <param name="act">回调</param>
        /// <returns></returns>
        public ITimer Wait(int intervalMS, bool isScaled, Action act)
        {
            return Add(intervalMS, 1, isScaled, act);
        }

        /// <summary>
        /// 运行一个一次性的计时器
        /// </summary>
        /// <param name="intervalMS">延迟时间：毫秒</param>
        /// <param name="act">回调，会接收这个计时器对象，用于查询计时器信息</param>
        /// <returns></returns>
        public ITimer Wait(int intervalMS, Action<ITimer> act)
        {
            return Add(intervalMS, 1, false, act);
        }

        /// <summary>
        /// 运行一个一次性的计时器
        /// </summary>
        /// <param name="intervalMS">延迟时间：毫秒</param>
        /// <param name="isScaled">是否被时间缩放影响</param>
        /// <param name="act">回调，会接收这个计时器对象，用于查询计时器信息</param>
        /// <returns></returns>
        public ITimer Wait(int intervalMS, bool isScaled, Action<ITimer> act)
        {
            return Add(intervalMS, 1, isScaled, act);
        }

        /// <summary>
        /// 运行一个一次性的计时器，并阻塞线程
        /// </summary>
        /// <param name="intervalMS">延迟时间：毫秒</param>
        /// <param name="isScaled">是否被时间缩放影响</param>
        /// <returns></returns>
        public async SlimTask Wait(int intervalMS, bool isScaled)
        {
            var task = SlimTask.Create();
            Wait(intervalMS, isScaled, () => SlimTask.Release(task));
            await task;
        }

        /// <summary>
        /// 运行一个可以设置次数和间隔的计时器
        /// </summary>
        /// <param name="intervalMS">计时器执行回调的时间间隔：毫秒</param>
        /// <param name="loopTime">循环次数，当小于等于0表示无限循环</param>
        /// <param name="act">回调</param>
        /// <returns></returns>
        public ITimer Run(int intervalMS, int loopTime, Action act)
        {
            return Add(intervalMS, loopTime, false, act);
        }

        /// <summary>
        /// 运行一个可以设置次数和间隔的计时器
        /// </summary>
        /// <param name="intervalMS">计时器执行回调的时间间隔：毫秒</param>
        /// <param name="loopTime">循环次数，当小于等于0表示无限循环</param>
        /// <param name="isScaled">是否被时间缩放影响</param>
        /// <param name="act">回调</param>
        /// <returns></returns>
        public ITimer Run(int intervalMS, int loopTime, bool isScaled, Action act)
        {
            return Add(intervalMS, loopTime, isScaled, act);
        }

        /// <summary>
        /// 运行一个可以设置次数和间隔的计时器
        /// </summary>
        /// <param name="intervalMS">计时器执行回调的时间间隔：毫秒</param>
        /// <param name="loopTime">循环次数，当小于等于0表示无限循环</param>
        /// <param name="act">回调，会接收这个计时器对象，用于查询计时器信息</param>
        /// <returns></returns>
        public ITimer Run(int intervalMS, int loopTime, Action<ITimer> act)
        {
            return Add(intervalMS, loopTime, false, act);
        }

        /// <summary>
        /// 运行一个可以设置次数和间隔的计时器
        /// </summary>
        /// <param name="intervalMS">计时器执行回调的时间间隔：毫秒</param>
        /// <param name="loopTime">循环次数，当小于等于0表示无限循环</param>
        /// <param name="isScaled">是否被时间缩放影响</param>
        /// <param name="act">回调，会接收这个计时器对象，用于查询计时器信息</param>
        /// <returns></returns>
        public ITimer Run(int intervalMS, int loopTime, bool isScaled, Action<ITimer> act)
        {
            return Add(intervalMS, loopTime, isScaled, act);
        }

        /// <summary>
        /// 运行一个每帧运行的计时器，除非主动取消，否则一直执行下去
        /// </summary>
        /// <param name="act">回调，会接收这个计时器对象，用于查询计时器信息</param>
        /// <returns></returns>
        public ITimer Frame(Action act, bool isScaled = true)
        {
            return Add(0, 0, isScaled, act);
        }

        /// <summary>
        /// 运行一个每帧运行的计时器，除非主动取消，否则一直执行下去
        /// </summary>
        /// <param name="act">回调，会接收这个计时器对象，用于查询计时器信息</param>
        /// <returns></returns>
        public ITimer Frame(Action<ITimer> act, bool isScaled = true)
        {
            return Add(0, 0, isScaled, act);
        }

        /// <summary>
        /// 取消 Timer，并清空引用
        /// </summary>
        /// <param name="timer"></param>
        public void Cancel(ref ITimer timer)
        {
            Remove(timer);
            timer = null;
        }

        /// <summary>
        /// 获取毫秒数计时器
        /// </summary>
        public ITime GetTime()
        {
            return Time;
        }
        #endregion
    }
}