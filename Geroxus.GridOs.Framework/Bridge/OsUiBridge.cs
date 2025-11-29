using System.Collections.Generic;
using System.Text;

namespace IngameScript
{
    public class OsUiBridge : OsBridge<OsUiBridge>
    {
        private Dictionary<ProcessId, StringBuilder> _uiComponents = new Dictionary<ProcessId, StringBuilder>();
        public Dictionary<ProcessId, StringBuilder> AllComponents => _uiComponents;

        public StringBuilder GetUiComponent(IGridOsProcess process)
        {
            return _uiComponents.GetValueOrNew(process.ProcessId);
        }
    }
}