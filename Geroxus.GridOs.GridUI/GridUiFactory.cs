using System;

namespace IngameScript
{
    public class GridUiFactory : IProcessFactory<IGridUi>
    {
        public Func<string, IGridUi> CreationStrategy(ProcessIdProvider idProvider) => (name) => new GridUi(idProvider.Next(typeof(IGridProgram)), name);
        public string ProcessName { get; } = typeof(GridUi).ToString();
    }
}