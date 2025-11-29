using System.Text;

namespace IngameScript
{
    public class PersistentAccelerationService : GridServiceBase<AccelarationInformation>
    {
        private StringBuilder _ui;

        public override void Run()
        {
            // OsFlagBridge.Instance.HorizontalSpeed;
            _ui.AppendLine("Acceleration");
            _ui.AppendLine(OsFlagBridge.Instance.HorizontalSpeed?.ToString());
        }

        public override void SetUp()
        {
            _ui = OsUiBridge.Instance.GetUiComponent(this);
        }
        
        public override AccelarationInformation Info { get; }
    }

    public struct AccelarationInformation
    {
    }
}