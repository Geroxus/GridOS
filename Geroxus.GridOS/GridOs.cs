using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class GridOs
    {
        private static readonly GridOs _instance;
        public static GridOs Instance { get; } = _instance ?? (_instance = new GridOs());
        private Dictionary<ProcessId, IGridOsProcess> Processes { get; } = new Dictionary<ProcessId, IGridOsProcess>();
        private OsProcessBridge ProcessBridge { get; } = OsProcessBridge.Instance;

        private static readonly DateTime  BuildDate = new DateTime(2025, 12, 02);
        public static string Version { get; } = $"0.1-beta-{BuildDate.Date.ToShortDateString()}";
        public string VersionString => Version;

        public static GridOs BootStrap(
            IMyGridTerminalSystem gridTerminalSystem
            )
        {
            LOGGER.Always($"Welcome to Grid Os Version v{Version}!");
            // configure and setup bridges
            OsGridAccessBridge.Instance.RegisterGridTerminalSystem(gridTerminalSystem);
            
            return Instance;
        }

        public DateTime BootTime { get;}
        public TimeSpan ActiveTime => DateTime.Now.Subtract(BootTime);

        private GridOs()
        {
            BootTime = DateTime.Now;
            OsProcessBridge.Instance.RegisterProcessLists(Processes);
            
            ProcessBridge.Register(ProgramFactory.Create<BootService>());
        }

        public void Operate()
        {
            // This shouldn't be logged like this and is actively harming permanent logs. I'll leave it here for future reference though
            // LOGGER.Always($"Operating for {ActiveTime.ToString()}");
            foreach (IGridOsProcess process in Processes.Values)
            {
                LOGGER.Info($"Process: {process.GetType().Name} : {process.Name}");
                try
                {
                    process.Run();
                }
                catch (Exception e)
                {
                    LOGGER.Error(e.ToString());
                    ProcessBridge.RegisterStop(process.ProcessId);
                }
            }
            LOGGER.Info("Cleanup Operator");
            ProcessBridge.Run();
        }
    }
}