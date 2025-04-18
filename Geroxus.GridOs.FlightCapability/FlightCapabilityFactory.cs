using System;

namespace IngameScript
{
    public class FlightCapabilityFactory : IProcessFactory<FlightCapability>
    {
        public Func<ProcessId, string, FlightCapability> CreateProcess() => (id, name) => new FlightCapability(id, name);
    }
}