using Game.Frame.Timer;
using UnityEngine;

namespace Game.Frame.Coroutine
{
    public struct AwaitTime : ICoroutineAwaiter
    {
        private readonly float targetTime;
        private ITime time;

        public AwaitTime(int waitTimeByMS)
        {
            time = AppFacade.Timer.Time;
            targetTime = time.StartDurationMS + waitTimeByMS;
        }

        public bool IsDone => time.StartDurationMS >= targetTime;
    }
}
