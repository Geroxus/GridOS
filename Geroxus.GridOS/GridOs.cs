using System.Collections.Generic;

namespace IngameScript
{
    public class GridOs
    {
        private Dictionary<ProcessId, IGridOsProcess> Processes { get; } = new Dictionary<ProcessId, IGridOsProcess>();
        private OsProcessBridge ProcessBridge { get; } = OsProcessBridge.Instance;

        public GridOs()
        {
            OsProcessBridge.Instance.RegisterProcessLists(Processes);
            ProcessBridge.Register(ServiceFactory.GetBootService());
        }

        public void Operate()
        {
            LOGGER.Write("Operating");
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