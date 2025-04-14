using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace IngameScript
{
    public abstract class GridService
    {
        protected readonly OsProcessBridge Os = OsProcessBridge.Instance;

        public abstract void Run();
    }

    public class OsProcessBridge
    {
        private List<IGridDriver> _drivers;
        private List<GridService> _services;
        public static OsProcessBridge Instance { get; } = new OsProcessBridge();

        public void RegisterProcessLists(List<IGridDriver> drivers, List<GridService> services)
        {
            _drivers = drivers;
            _services = services;
        }

        public ImmutableList<IGridDriver> GetDrivers(Type type)
        {
            return _drivers.Where(d => d.GetType() == type).ToImmutableList();
        }
    }
}