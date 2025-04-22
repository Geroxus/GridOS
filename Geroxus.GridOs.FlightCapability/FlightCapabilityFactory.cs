using System;

namespace IngameScript
{
    public class FlightCapabilityFactory : IProcessFactory<FlightCapability>
    {
        public Func<string, FlightCapability> CreationStrategy(ProcessIdProvider idProvider) => (name) => new FlightCapability(idProvider.Next(typeof(IGridService)), name);
        public string ProcessName { get; } =  typeof(FlightCapability).ToString();
    }
}