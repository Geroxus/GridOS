using System;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public abstract class GridDriverBase<T> : IGridDriver
    {
        public void Dispose()
        {
        }
        
        public string Name { get; private set; }
        public ProcessId ProcessId { get; private set; }
        protected T Component { get; private set; }
        public abstract void Run();
        public abstract void SetUp();
        public void Initialize(ProcessIdProvider processIdProvider, string name)
        {
            ProcessId = processIdProvider.Next(typeof(IGridDriver));
            Name = name;
        }

        public void SetComponent(T component)
        {
            if (Component == null)
                Component = component;
            else
                throw new Exception("Component already set");
        }
    }
}