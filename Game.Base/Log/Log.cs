using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Game.Base.Logs
{
    public static class Log
    {
        public static ILogger Logger { get; set; }


        [Conditional("LOG_DEBUG")]
        public static void Debug(string format, params object[] args)
        {
            if (Logger != null)
            {
                Logger.Debug(format, args);
            }
        }

        [Conditional("LOG_DEBUG")]
        public static void Debug(string format)
        {
            if (Logger != null)
            {
                Logger.Debug(format);
            }
        }

        [Conditional("LOG_DEBUG")]
        [Conditional("LOG_INFO")]
        public static void Info(string format)
        {
            if (Logger != null)
            {
                Logger.Info(format);
            }
        }

        [Conditional("LOG_DEBUG")]
        [Conditional("LOG_INFO")]
        public static void Info(string format, params object[] args)
        {
            if (Logger != null)
            {
                Logger.Info(format, args);
            }
        }

        [Conditional("LOG_DEBUG")]
        [Conditional("LOG_INFO")]
        [Conditional("LOG_WARINING")]
        public static void Warning(string format, params object[] args)
        {
            if (Logger != null)
            {
                Logger.Warning(format, args);
            }
        }

        [Conditional("LOG_DEBUG")]
        [Conditional("LOG_INFO")]
        [Conditional("LOG_WARINING")]
        [Conditional("LOG_ERROR")]
        public static void Important(string format, params object[] args)
        {
            if (Logger != null)
            {
                Logger.Important(format, args);
            }
        }

        [Conditional("LOG_DEBUG")]
        [Conditional("LOG_INFO")]
        [Conditional("LOG_WARINING")]
        [Conditional("LOG_ERROR")]
        public static void Important(string format)
        {
            if (Logger != null)
            {
                Logger.Important(format);
            }
        }

        [Conditional("LOG_DEBUG")]
        [Conditional("LOG_INFO")]
        [Conditional("LOG_WARINING")]
        [Conditional("LOG_ERROR")]
        public static void Error(string format, params object[] args)
        {
            if (Logger != null)
            {
                Logger.Error(format, args);
            }
        }

        [Conditional("LOG_DEBUG")]
        [Conditional("LOG_INFO")]
        [Conditional("LOG_WARINING")]
        [Conditional("LOG_ERROR")]
        public static void Error(string format)
        {
            if (Logger != null)
            {
                Logger.Error(format);
            }
        }

        [Conditional("LOG_DEBUG")]
        [Conditional("LOG_INFO")]
        [Conditional("LOG_WARINING")]
        [Conditional("LOG_ERROR")]
        [Conditional("LOG_EXCEPTION")]
        public static void Exception(Exception e)
        {
            if (Logger != null)
            {
                Logger.Exception(e);
            }
        }

        [Conditional("LOG_DEBUGSTEPTIMECOST")]
        public static void DebugStepTimeCost(string label, string step)
        {
            if (Logger != null)
            {
                Logger.DebugStepTimeCost(label, step);
            }
        }
    }
}
