using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    internal class EnrichedShipController : IEnrichedComponent<IMyShipController>
    {
        public EnrichedShipController(IMyShipController shipController)
        {
            Component = shipController;
            Name = shipController.DisplayNameText;
        }

        public IMyShipController Component { get; }
        public string Name { get; }
    }
}