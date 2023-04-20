using System;

namespace Game.Base.Logs
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(string format, params object[] args)
        {
            Console.WriteLine("[DEBUG]:" + format, args);
        }

        public void Debug(string format)
        {
            Console.WriteLine("[DEBUG]:" + format);
        }

        public void Error(string format, params object[] args)
        {
            Console.WriteLine("[ERROR]:" + format, args);
        }

        public void Error(string format)
        {
            Console.WriteLine("[ERROR]:" + format);
        }

        public void Exception(Exception e)
        {
            Console.WriteLine("[EXCEPTION]:" + e.ToString());
        }

        public void Important(string format, params object[] args)
        {
            Console.WriteLine("[Important]:" + format, args);
        }

        public void Important(string format)
        {
            Console.WriteLine("[Important]:" + format);
        }

        public void Info(string format, params object[] args)
        {
            Console.WriteLine("[INFO]:" + format, args);
        }

        public void Warning(string format, params object[] args)
        {
            Console.WriteLine("[WARN]:" + format, args);
        }

        public void Info(string format)
        {
            Console.WriteLine("[INFO]:" + format);
        }

        public void Warning(string format)
        {
            Console.WriteLine("[WARN]:" + format);
        }

        public void DebugStepTimeCost(string label, string name)
        {

        }
    }
}
