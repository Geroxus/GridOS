using System;

namespace IngameScript
{
    public static class LOGGER
    {
        private static Func<string, Action<string>> _output;
        private static bool _info = false;

        public static void RegisterOutput(Action<string> action)
        {
            _output = outer => inner => action($"{outer}{inner}");
        }

        public static void SetLogLevelInfo()
        {
            _info = true;
        }

        public static void DisableLogging()
        {
            _info = false;
        }

        public static Action<string> Always => _output("");

        public static Action<string> Info
        {
            get
            {
                return _info ? _output("[[Info]] -- ") : s => { };
            }
            private set {  }
        }
    }
}