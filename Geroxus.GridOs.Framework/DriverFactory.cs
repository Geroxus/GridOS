using System;
using System.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public static class DriverFactory
    {
        private static readonly ProcessIdProvider ProcessIdProvider = new ProcessIdProvider((new ProcessId(90000)));

        public static IGridDriver Get<T>(IEnrichedComponent<T> enrichedComponent)
        {
            if (enrichedComponent.Component is IMyTextSurface)
            {
                IMyTextSurface textSurface = enrichedComponent.Component as IMyTextSurface;
                if (!OsProcessBridge.Instance.GetDrivers(typeof(DisplayDriver))
                        .Any(d => d.Name.Contains(enrichedComponent.Name)))
                {
                    return new DisplayDriver(textSurface, ProcessIdProvider.Next(),
                        $"DisplayDriver[[{enrichedComponent.Name}]]");
                }

                return (DisplayDriver)null;
            }

            throw new Exception("not implemented");
        }
    }
}