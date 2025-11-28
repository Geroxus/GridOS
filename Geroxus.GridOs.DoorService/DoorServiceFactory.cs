using System;

namespace IngameScript
{
    public class DoorServiceFactory : IProcessFactory<DoorService>
    {
        public Func<string, DoorService> CreationStrategy(ProcessIdProvider idProvider) => name => new DoorService(idProvider.Next(typeof(IGridService)), "Door Service");

        public string ProcessName { get; } = typeof(DoorService).ToString();
    }
}