using System;

namespace IngameScript
{
    public class ServiceFactory
    {
        private static readonly ProcessIdProvider ProcessIdProvider = new ProcessIdProvider(40000);

        public static BootService GetBootService()
        {
            return new BootService(ProcessIdProvider.Next());
        }
    }
}