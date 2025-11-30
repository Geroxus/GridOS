using System.Collections.Generic;
using System.Text;

namespace IngameScript
{
    public class OsUiBridge : OsBridge<OsUiBridge>
    {
        private readonly Dictionary<ProcessId, GridUiHandler> _uiComponents = new Dictionary<ProcessId, GridUiHandler>();
        public Dictionary<ProcessId, GridUiHandler> AllComponents => _uiComponents;

        public GridUiHandler GetUiComponent(IGridOsProcess process)
        {
            return _uiComponents.GetValueOrNew(process.ProcessId);
        }
    }
}