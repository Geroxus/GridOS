using System;

namespace IngameScript
{
    public static class LOGGER
    {
        private static Func<string, Action<string>> _output;
        private static bool _info = false;
        private static bool _error = true;

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

        // It's been a while. But this looks more like a crutch not meant for permanent use. Therefore, consider this as Deprecated
        public static Action<string> Always => _output("");

        public static Action<string> Error
        {
            get
            {
                return _error ? _output("[[ERROR]] -- ") : s => { };
            }
            private set {  }
        }
        
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