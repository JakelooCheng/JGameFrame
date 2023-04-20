using Game.Frame.Timer;
using UnityEngine;

namespace Game.Frame.Coroutine
{
    public struct AwaitFrame : ICoroutineAwaiter
    {
        private readonly int targetFrame;

        private ITime time;

        public AwaitFrame(int frame) 
        {
            time = AppFacade.Timer.Time;
            targetFrame = time.FrameCount + frame;
        }

        public bool IsDone => time.FrameCount >= targetFrame;
    }
}
