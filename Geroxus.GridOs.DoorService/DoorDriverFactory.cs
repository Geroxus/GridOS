using System;

namespace IngameScript
{
    public class DoorDriverFactory : IProcessFactory<IGridOsProcess>
    {
        public Func<string, IGridOsProcess> CreationStrategy(ProcessIdProvider idProvider) => name => new DoorDriver(idProvider.Next(typeof(IGridDriver)), name);

        public string ProcessName { get; }
    }
}