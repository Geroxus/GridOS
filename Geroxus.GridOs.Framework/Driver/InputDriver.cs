using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public class InputDriver : GridDriverBase<EnrichedShipController>
    {

        public bool IsControlled => Component.Component.IsUnderControl;

        public override void Run()
        {
            
        }

        public override void SetUp()
        {
        }

        public Vector3D GetNaturalGravity() => Component.Component.GetNaturalGravity();
        public MyShipMass CalculateShipMass() => Component.Component.CalculateShipMass();
    }
}