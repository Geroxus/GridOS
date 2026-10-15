using System;
using System.Collections.Generic;

namespace IngameScript
{
    public static class ProgramFactory
    {
        private static readonly ProcessIdProvider ProcessIdProvider = new ProcessIdProvider();
        public static IGridOsProcess Create<T>() where T : IGridOsProcess, new()
        {
            if (typeof(T) == typeof(IGridDriver))
                throw new Exception("Cannot create a driver from ProgramFactory. Use DriverFactory instead.");
            T  process = new T();
            string name = typeof(T).ToString();
            process.Initialize(ProcessIdProvider, name);
            return process;
        }
    }
}