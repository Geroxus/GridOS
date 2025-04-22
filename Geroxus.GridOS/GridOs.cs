using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class GridOs
    {
        private Dictionary<ProcessId, IGridOsProcess> Processes { get; } = new Dictionary<ProcessId, IGridOsProcess>();
        private OsProcessBridge ProcessBridge { get; } = OsProcessBridge.Instance;

        public string Version { get; set; }

        public static GridOs BootStrap(
            IMyGridTerminalSystem gridTerminalSystem,
            IProcessFactory<BootService> bootServiceFactory = null,
            IProcessFactory<IGridUi> gridUiFactory = null
            )
        {
            // configure and setup bridges
            OsGridAccessBridge.Instance.RegisterGridTerminalSystem(gridTerminalSystem);

            // register services
            if(bootServiceFactory == null) bootServiceFactory = new BootServiceFactory();
            ProgramFactory.Register(bootServiceFactory);
            
            if(gridUiFactory == null) gridUiFactory = new GridUiFactory();
            ProgramFactory.Register(gridUiFactory);

            return new GridOs();
        }

        private GridOs()
        {
            OsProcessBridge.Instance.RegisterProcessLists(Processes);
            
            ProcessBridge.Register(ProgramFactory.Get<BootService>());
        }

        public void Operate()
        {
            LOGGER.Write($"Operating v{Version}");
            foreach (IGridOsProcess process in Processes.Values)
            {
                LOGGER.Write($"Process: {process.GetType().Name} : {process.Name}");
                process.Run();
            }
            LOGGER.Write("Cleanup Operator");
            ProcessBridge.Run();
        }
    }
}