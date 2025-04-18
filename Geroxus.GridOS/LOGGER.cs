using System;

namespace IngameScript
{
    public static class LOGGER
    {
        public static void RegisterOutput(Action<string> action)
        {
            Write = s => action($"[[Info]] {s}");
        }

        public static Action<string> Write { get; set; }
    }
}