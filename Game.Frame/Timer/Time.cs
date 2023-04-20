using UnityEngine;

namespace Game.Frame.Timer
{
    public class Time : ITime
    {
        public float DeltaMS => UnityEngine.Time.deltaTime * 1000;

        public float RealDeltaMS => UnityEngine.Time.unscaledDeltaTime * 1000;

        public float StartDurationMS => UnityEngine.Time.realtimeSinceStartup * 1000;

        public float TimeScale => UnityEngine.Time.timeScale;

        public int FrameCount => UnityEngine.Time.frameCount;
    }
}