using System;

namespace IngameScript
{
    public class BootServiceFactory : IProcessFactory<BootService>
    {
        public Func<string, BootService> CreationStrategy(ProcessIdProvider idProvider) =>
            (name) => new BootService(idProvider.Next(typeof(IGridService)), "GridOS Boot Service");

        public string ProcessName { get; } = typeof(BootService).ToString();
    }
}