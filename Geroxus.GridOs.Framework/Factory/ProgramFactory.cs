namespace IngameScript
{
    public static class ProgramFactory
    {
        private static readonly ProcessIdProvider ProcessIdProvider = new ProcessIdProvider(new ProcessId(1000));
        public static GridUI GetGridUi()
        {
            var factory = new GridUiFactory();
            return factory.CreateProcess().Invoke(ProcessIdProvider.Next(), "GridUI");
        }

        public static FlightCapability GetFlightCapability()
        {
            var factory = new FlightCapabilityFactory();
            return factory.CreateProcess().Invoke(ProcessIdProvider.Next(), "FlightCapability");
        }
    }
}