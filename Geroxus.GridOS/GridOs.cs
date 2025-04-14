using System.Collections.Generic;

namespace IngameScript
{
    public class GridOs
    {
        private List<IGridDriver> Drivers { get; } = new List<IGridDriver>();
        private List<GridService> Services { get; } = new List<GridService>();

        public GridOs()
        {
            OsProcessBridge.Instance.RegisterProcessLists(Drivers, Services);
        }

        public GridOs RegisterDriver(IGridDriver driver)
        {
            Drivers.Add(driver);
            return this;
        }

        public GridOs RegisterService(GridService service)
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

            foreach (GridService service in Services)
            {
                service.Run();
            }
        }
    }
}