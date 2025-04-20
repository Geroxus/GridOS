using System;

namespace IngameScript
{
    public class FlightCapabilityFactory : IProcessFactory<FlightCapability>
    {
        public Func<string, FlightCapability> CreateProcess(ProcessIdProvider idProvider) => (name) => new FlightCapability(idProvider.Next(typeof(IGridService)), name);
    }
}