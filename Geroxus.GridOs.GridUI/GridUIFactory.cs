using System;

namespace IngameScript
{
    public class GridUiFactory : IProcessFactory<GridUI>
    {
        public Func<string, GridUI> CreateProcess(ProcessIdProvider idProvider) => (name) => new GridUI(idProvider.Next(typeof(IGridProgram)), name);
    }
}