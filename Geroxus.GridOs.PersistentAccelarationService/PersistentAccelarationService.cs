namespace IngameScript
{
    public class PersistentAccelarationService : GridServiceBase<AccelarationInformation>
    {
        public override void Run()
        {
            // OsFlagBridge.Instance.HorizontalSpeed;
            GridUi ui = OsProcessBridge.Instance.GetAllProcesses().Find((p) => p.GetType() == typeof(GridUi)) as GridUi;
            ui?.Display(OsFlagBridge.Instance.HorizontalSpeed?.ToString());
        }

        public override void SetUp()
        {
            
        }

        public override AccelarationInformation Info { get; }
    }

    public struct AccelarationInformation
    {
    }
}