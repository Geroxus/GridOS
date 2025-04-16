using System;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public static class DriverFactory
    {
        private static readonly ProcessIdProvider ProcessIdProvider = new ProcessIdProvider((new ProcessId(90000)));

        public static IGridDriver Get<T>(T component)
        {
            if (component is IMyTextSurface)
            {
                return new DisplayDriver(component as IMyTextSurface, ProcessIdProvider.Next());
            }
            throw new Exception("not implemented");
        }
    }
}