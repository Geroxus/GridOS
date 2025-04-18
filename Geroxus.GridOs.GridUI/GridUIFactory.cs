using System;

namespace IngameScript
{
    public class GridUiFactory : IProcessFactory<GridUI>
    {
        public Func<ProcessId, string, GridUI> CreateProcess() => (id, name) => new GridUI(id, name);
    }
}