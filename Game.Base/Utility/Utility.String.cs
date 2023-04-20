using System;
using System.Text;

namespace Game.Base.Utility
{
    public static class Text
    {
        private static StringBuilder cachedStringBuilder { get; } = new StringBuilder();

        public static string Format(string format, params object[] args)
        {
            cachedStringBuilder.Clear();
            if (format != null)
            {
                cachedStringBuilder.AppendFormat(format, args);
            }

            return cachedStringBuilder.ToString();
        }
    }
}
