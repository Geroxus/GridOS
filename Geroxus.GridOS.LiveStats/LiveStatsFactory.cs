using System;

namespace IngameScript
{
    public class LiveStatsFactory : IProcessFactory<LiveStats>
    {
        public Func<string, LiveStats> CreationStrategy(ProcessIdProvider idProvider) => (name) => new LiveStats(idProvider.Next(typeof(IGridProgram)), name);

        public string ProcessName { get; } = typeof(LiveStats).ToString();
    }
}