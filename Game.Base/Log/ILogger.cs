using System;

namespace Game.Base.Logs
{
    public interface ILogger
    {
        void Debug(string format);

        void Debug(string format, params object[] args);

        void Info(string format);

        void Info(string format, params object[] args);

        void Warning(string format);

        void Warning(string format, params object[] args);

        void Important(string format);

        void Important(string format, params object[] args);

        void Error(string format);

        void Error(string format, params object[] args);

        void Exception(Exception e);

        void DebugStepTimeCost(string label, string name);
    }
}
