using System;

namespace IngameScript
{
    public abstract class GridProgramBase :  IGridProgram
    {
        public void Dispose()
        {
        }
        public string Name { get; private set; }
        public GridProgram Title { get; private set; }
        public ProcessId ProcessId { get; private set; }
        protected GridUiHandler Ui => OsUiBridge.Instance.GetUiComponent(this);
        public abstract void Run();
        public abstract void SetUp();
        public void Initialize(ProcessIdProvider processIdProvider, string name)
        {
            ProcessId = processIdProvider.Next(typeof(IGridProgram));
            Name = name;
            
            GridProgram title;
            Enum.TryParse<GridProgram>(name, out title);
            Title = title;
        }
    }
}