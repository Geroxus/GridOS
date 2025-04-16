using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace IngameScript
{
    public class OsProcessBridge : OsBridge<OsProcessBridge>
    {
        private List<IGridDriver> _drivers;
        private List<IGridService> _services;

        public void RegisterProcessLists(List<IGridDriver> drivers, List<IGridService> services)
        {
            _drivers = drivers;
            _services = services;
        }

        public ImmutableList<IGridDriver> GetDrivers(Type type)
        {
            return _drivers.Where(d => d.GetType() == type).ToImmutableList();
        }

        public ImmutableList<IGridOsProcess> GetAllProcesses()
        {
            return _drivers.Select(it => it as IGridOsProcess).Union(_services.Select(it => it as IGridOsProcess)).ToImmutableList();
        }

        public void RegisterDriver(IGridDriver driver)
        {
            // TODO no duplicates
            _drivers.Add(driver);
        }
    }
}