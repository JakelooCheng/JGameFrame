using System;
using System.Collections.Generic;
using Game.Base.Utility;

namespace Game.Base.Logs
{
    public class U3DLogger : ILogger
    {
        Dictionary<string, (float start, float last, string name)> stepTimestamps;

        public void Debug(string format, params object[] args)
        {
            UnityEngine.Debug.Log(Text.Format("<color=#888888>{0}</color>", Text.Format(format, args)));
        }

        public void Debug(string format)
        {
            UnityEngine.Debug.Log(Text.Format("<color=#888888>{0}</color>", format));
        }

        public void Info(string format)
        {
            UnityEngine.Debug.Log(format);
        }

        public void Info(string format, params object[] args)
        {
            UnityEngine.Debug.Log(Text.Format(format, args));
        }

        public void Warning(string format)
        {
            UnityEngine.Debug.LogWarning(format);
        }

        public void Warning(string format, params object[] args)
        {
            UnityEngine.Debug.LogWarning(Text.Format(format, args));
        }

        public void Error(string format, params object[] args)
        {
            UnityEngine.Debug.LogError(Text.Format(format, args));
        }

        public void Error(string format)
        {
            UnityEngine.Debug.LogError(format);
        }

        public void Exception(Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }

        public void Important(string format, params object[] args)
        {
            UnityEngine.Debug.LogError(Text.Format(format, args));
        }

        public void Important(string format)
        {
            UnityEngine.Debug.LogError(format);
        }

        public void DebugStepTimeCost(string label, string name)
        {
            stepTimestamps ??= new Dictionary<string, (float start, float last, string name)>();
            float current = UnityEngine.Time.realtimeSinceStartup;
            if (stepTimestamps.TryGetValue(label, out var timestamp))
            {
                UnityEngine.Debug.Log(
                    $"StepTimeCost {label} {name} cost time: {current - timestamp.start}; last step {timestamp.name} cost time: {current - timestamp.last}");
                stepTimestamps.Remove(label);
            }
            else
            {
                UnityEngine.Debug.Log(
                    $"StepTimeCost {label} start count time {name}");
                stepTimestamps.Add(label, (current, current, name));
            }
        }
    }
}
