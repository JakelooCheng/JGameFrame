using Game.Base.ObjectPool;
using Game.Base.LifeCycle;
using System;
using Game.Base.Logs;
using UnityEngine;

namespace Game.Frame.Timer
{
    public class Timer : ITimer
    {
        private static long instanceId = 0;
        private static long InstanceId => instanceId++;

        public long Id { get; private set; }

        public bool IsInvalid { get; private set; }

        public bool Pause { get; set; }

        public long LoopTime { get; private set; }

        public long LoopedTime { get; private set; }

        public int IntervalMS { get; private set; }

        public float RunDurationMS { get; private set; }

        public float StartMS { get; private set; }

        public bool IsScaled { get; private set; }

        /// <summary>
        /// 触发时回调
        /// </summary>
        private Action<ITimer> callback;

        /// <summary>
        /// 拿一个 Timer
        /// </summary>
        /// <param name="startMS">开始时间</param>
        /// <returns></returns>
        public static Timer Make(ITime time, int intervalMS, int loopTime, bool isScaled, Action<ITimer> callback)
        {
            var timer = GlobalObjectPool<Timer>.Get();
            timer.Id = InstanceId;
            timer.LoopTime = loopTime > 0 ? loopTime : long.MaxValue;
            timer.LoopedTime = 0;
            timer.IntervalMS = intervalMS;
            timer.RunDurationMS = 0;
            timer.StartMS = time.StartDurationMS;
            timer.Pause = false;
            timer.callback = callback;
            timer.IsInvalid = true;
            timer.IsScaled = isScaled;
            return timer;
        }

        public void Release()
        {
            IsInvalid = false;
            RunDurationMS = 0;
            callback = null;
        }

        public void OnUpdate(Time time)
        {
            RunDurationMS += IsScaled ? time.DeltaMS : time.RealDeltaMS;

            if (RunDurationMS >= (LoopedTime + 1) * IntervalMS && LoopedTime < LoopTime)
            {
                Execute();
            }
        }

        private void Execute()
        {
            LoopedTime++;

            try
            {
                callback?.Invoke(this);
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }
    }
}