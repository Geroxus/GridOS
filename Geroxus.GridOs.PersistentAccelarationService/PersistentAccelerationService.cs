using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public class PersistentAccelerationService : GridServiceBase<AccelerationInformation>
    {
        private TextNode _text;
        private int? _previousValue = null;

        /**
         * TODO this shouldn't necessitate specific things to exist. In particular it should shut itself down in that case.
         */
        public override void Run()
        {
            // OsFlagBridge.Instance.HorizontalSpeed;
            _text.AppendLine("Acceleration");
            FlightCapability flightCapability = OsProcessBridge.Instance.GetServices(typeof(FlightCapability)).OfType<FlightCapability>().Single();
            PhysicsValue<float> forceNeededToMaintainSpeed = flightCapability.Info.ShipGravity;
            PhysicsValue<float> maxThrustUp = flightCapability.Info.MaxThrustPerDirection[Vector3I.Down];
            float percentThrust = ((float)forceNeededToMaintainSpeed.Value / (float)maxThrustUp.Value);
            InputDriver someController = OsProcessBridge.Instance.GetDrivers(typeof(InputDriver)).OfType<InputDriver>().First();
            Vector3D naturalGravity = someController.GetNaturalGravity();
            _text.AppendLine($"Gravity:x({naturalGravity.X,4:N})y({naturalGravity.Y,4:N})z({naturalGravity.Z,4:N})");
            _text.AppendLine($"Gravity:x({naturalGravity.Normalized().X,4:N})y({naturalGravity.Normalized().Y,4:N})z({naturalGravity.Normalized().Z,4:N})");
            _text.AppendLine($"ThrustPerDirection:{Environment.NewLine}" +
                           $"up     ({flightCapability.Info.ThrustPerDirection[Vector3I.Down].Value,15:N}){Environment.NewLine}" +
                           $"forward({flightCapability.Info.ThrustPerDirection[Vector3I.Backward].Value,15:N}){Environment.NewLine}" +
                           $"left   ({flightCapability.Info.ThrustPerDirection[Vector3I.Right].Value,15:N})");
            _text.AppendLine($"{forceNeededToMaintainSpeed.ToString()} vs {maxThrustUp.ToString()}");
            if (OsFlagBridge.Instance.VerticalSpeed.HasValue)
            {
                int verticalSpeed = OsFlagBridge.Instance.VerticalSpeed.Value;
                _previousValue = verticalSpeed;
                MaintainVerticalAcceleration(percentThrust);
                _text.AppendLine(verticalSpeed.ToString());
            }
            else if (_previousValue != null)
            {
                _previousValue = null;
                SetThrusterToPercent(Vector3I.Down, 0);
            }
        }

        private void MaintainVerticalAcceleration(float percentThrust)
        {

            bool accelerate = false;
            if (false)
            {
                //remove safe percent
            }

            _text.AppendLine($"Acceleration set to: {percentThrust}");
            SetThrusterToPercent(Vector3I.Down, percentThrust);
        }

        private void SetThrusterToPercent(Vector3I direction, float percentThrust)
        {
            foreach (ThrustDriver thrust in OsProcessBridge.Instance.GetDrivers(typeof(ThrustDriver)).OfType<ThrustDriver>().Where(t => t.Direction == direction))
            {
                thrust.SetThrustPercent(percentThrust);
            }
        }

        public override List<string> GetRequirements() => new List<string> { typeof(FlightCapability).ToString() } ;

        public override void SetUp()
        {
            GridUiHandler ui = OsUiBridge.Instance.GetUiComponent(this);
            ui.Program = GridProgram.NONE;
            _text = ui.RootNode.CreateChildNode<TextNode>();
        }
        
        public override AccelerationInformation Info { get; }
    }

    public struct AccelerationInformation
    {
    }
}