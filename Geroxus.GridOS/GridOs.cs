using System.Collections.Generic;

namespace IngameScript
{
    public class GridOs
    {
        private List<IGridDriver> Drivers { get; } = new List<IGridDriver>();
        private List<IGridService> Services { get; } = new List<IGridService>();

        public GridOs()
        {
            OsProcessBridge.Instance.RegisterProcessLists(Drivers, Services);
            RegisterService(ServiceFactory.GetBootService());
        }

        public GridOs RegisterDriver(IGridDriver driver)
        {
            Drivers.Add(driver);
            return this;
        }

        public GridOs RegisterService(IGridService service)
        {
            Services.Add(service);
            return this;
        }

        public void Operate()
        {
            foreach (IGridDriver driver in Drivers)
            {
                driver.Update();
            }

            foreach (IGridService service in Services)
            {
                service.Run();
            }
        }
    }
}