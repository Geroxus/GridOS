using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class GridOs
    {
        private Dictionary<ProcessId, IGridOsProcess> Processes { get; } = new Dictionary<ProcessId, IGridOsProcess>();
        private OsProcessBridge ProcessBridge { get; } = OsProcessBridge.Instance;

        private static readonly DateTime  BuildDate = new DateTime(2025, 11, 27);
        public static string Version { get; } = $"0.1-beta-{BuildDate.Date.ToShortDateString()}";
        public string VersionString => Version;

        public static GridOs BootStrap(
            IMyGridTerminalSystem gridTerminalSystem
            )
        {
            // configure and setup bridges
            OsGridAccessBridge.Instance.RegisterGridTerminalSystem(gridTerminalSystem);

            return new GridOs();
        }

        private GridOs()
        {
            OsProcessBridge.Instance.RegisterProcessLists(Processes);
            
            ProcessBridge.Register(ProgramFactory.Create<BootService>());
        }

        public void Operate()
        {
            LOGGER.Info($"Operating v{Version}");
            foreach (IGridOsProcess process in Processes.Values)
            {
                LOGGER.Info($"Process: {process.GetType().Name} : {process.Name}");
                process.Run();
            }
            LOGGER.Info("Cleanup Operator");
            ProcessBridge.Run();
        }
    }
}