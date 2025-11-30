using System;
using System.Text;

namespace IngameScript
{
    public class GridUiHandler
    {
        private StringBuilder _internal = new StringBuilder();
        public GridUiHandler AppendLine(String str)
        {
            _internal.AppendLine(str);
            return this;
        }

        public string Out()
        {
            string output = _internal.ToString();
            _internal.Clear();
            return output;
        }
    }
}