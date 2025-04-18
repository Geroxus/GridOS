using System.Collections.Generic;

namespace IngameScript
{
    public class GridOs
    {
        private Dictionary<ProcessId, IGridDriver> Drivers { get; } = new Dictionary<ProcessId, IGridDriver>();
        private Dictionary<ProcessId, IGridService> Services { get; } = new Dictionary<ProcessId, IGridService>();
        private OsProcessBridge ProcessBridge { get; } = OsProcessBridge.Instance;

        public GridOs()
        {
            OsProcessBridge.Instance.RegisterProcessLists(Drivers, Services);
            ProcessBridge.Register(ServiceFactory.GetBootService());
        }

        public void Operate()
        {
            LOGGER.Write("Operating");
            foreach (IGridDriver driver in Drivers.Values)
            {
                LOGGER.Write("Driver: " + driver.GetType().Name);
                driver.Update();
            }

            foreach (IGridService service in Services.Values)
            {
                LOGGER.Write("Services: " + service.GetType().Name);
                service.Run();
            }
            LOGGER.Write("Cleanup Operator");
            ProcessBridge.StopNext();
        }
    }
}