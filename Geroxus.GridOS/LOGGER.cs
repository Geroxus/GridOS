using System;
using System.Collections.Generic;
using System.Text;

namespace IngameScript
{
    public static class LOGGER
    {
        private static Func<string, Action<string>> _output;
        private static Action<string> _echo;
        private static DateTime OsBootTime;
        private static TimeSpan OsActiveTime => DateTime.Now.Subtract(OsBootTime);
        private static bool _info = false;
        private static bool _error = true;
        private static List<Log> _logs = new List<Log>();

        public static void RegisterOutput(Action<string> action)
        {
            _echo = action;
            _output = outer => inner => action($"{outer}{inner}");
        }

        public static void RegisterBootTime(DateTime bootTime)
        {
            OsBootTime = bootTime;
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
        public static Action<string> DisplayOnly => _output("");

        public static Action<string> Error
        {
            get
            {
                if (_error)
                    return s => _logs.Add(new Log(OsActiveTime, "ERROR", s));
                return s => { };
            }
            private set { }
        }

        public static Action<string> Info
        {
            get
            {
                if (_info)
                    return s => _logs.Add(new Log(OsActiveTime, "INFO", s));
                return s => { };
            }
            private set { }
        }

        public static void PrintLogs(int maxListings = 15)
        {
            _echo(GetLog(maxListings));
        }

        public static string GetLog(int maxListings = 15)
        {
            StringBuilder builder = new StringBuilder();
            int logsCount = maxListings > _logs.Count ? _logs.Count : maxListings;
            for (var i = 0; i < logsCount; i++)
            {
                var log = _logs[_logs.Count - i - 1];
                builder.AppendLine(log.ToString());
            }

            return builder.ToString();
        }
    }

    internal struct Log
    {
        private TimeSpan UptimeStamp;
        private string Level;
        private string Message;

        public Log(TimeSpan uptimeStamp, string level, string message)
        {
            UptimeStamp = uptimeStamp;
            Level = level;
            Message = message;
        }

        public override string ToString() =>
            $"[[{UptimeStamp.Minutes}:{UptimeStamp.Seconds}.{UptimeStamp.Milliseconds}]][[{Level}]]: {Message}";
    }
}