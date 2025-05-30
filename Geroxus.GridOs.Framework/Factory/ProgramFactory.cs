using System;
using System.Collections.Generic;

namespace IngameScript
{
    public static class ProgramFactory
    {
        private static readonly Dictionary<string, IProcessFactory<IGridOsProcess>> Factories =
            new Dictionary<string, IProcessFactory<IGridOsProcess>>();

        private static readonly ProcessIdProvider ProcessIdProvider = new ProcessIdProvider();

        public static IGridOsProcess Get<T>() where T : IGridOsProcess
        {
            string programName = typeof(T).ToString();
            IProcessFactory<IGridOsProcess> factory;
            if (Factories.TryGetValue(programName, out factory) && factory != null)
                return factory.CreationStrategy(ProcessIdProvider).Invoke(programName);
            throw new Exception($"No factory registered for type {typeof(T).Name}");
        }

        public static void Register(IProcessFactory<IGridOsProcess> processFactory)
        {
            Factories.Add(processFactory.ProcessName, processFactory);
        }
    }
}