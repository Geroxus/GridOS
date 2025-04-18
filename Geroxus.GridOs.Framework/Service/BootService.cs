using System;
using System.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class BootService : IGridService
    {
        public string Name { get; }
        public ProcessId ProcessId { get; }
        private OsProcessBridge Processes { get; } = OsProcessBridge.Instance;
        private OsGridAccessBridge Grid { get; } = OsGridAccessBridge.Instance;

        public BootService(ProcessId processId)
        {
            Name = "GridOS Boot Service";
            ProcessId = processId;
        }

        public void Run()
        {
            Grid.Get<IMyTextSurface>(DriverFactory.Get).ForEach(Processes.Register);
            LOGGER.Write("Boot registered displays");

            Processes.Register(ProgramFactory.GetGridUi());

            // displaying stuff
            LOGGER.Write("Boot finished, Stop Booting");
            Processes.RegisterStop(ProcessId);
        }

        public void Dispose()
        {
        }
    }

    public static class ProgramFactory
    {
        private static readonly ProcessIdProvider ProcessIdProvider = new ProcessIdProvider(new ProcessId(1000));
        public static GridUI GetGridUi()
        {
            var factory = new GridUiFactory();
            return factory.CreateProcess().Invoke(ProcessIdProvider.Next(), "GridUI");
        }
    }
}