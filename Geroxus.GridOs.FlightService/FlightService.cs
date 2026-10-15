using System;
using System.Collections.Generic;
using VRageMath;

namespace IngameScript
{
    public class FlightService : GridServiceBase<FlightInfo>
    {
        private static FlightInfo _info = new FlightInfo();
        public override FlightInfo Info { get; } = _info;
        public override void Run()
        {
            var inputDrivers = OsProcessBridge.Instance.GetDrivers(typeof(InputDriver));
            foreach (var inputDriver in inputDrivers)
            {
                var linearVelocity = ((InputDriver)inputDriver).GetShipVelocities().LinearVelocity;
                _info.LinearVelocity = linearVelocity;
            }
        }

        public override List<string> GetRequirements() => new List<String>();

        public override void SetUp()
        {
        }

    }

    public struct FlightInfo
    {
        public Vector3D LinearVelocity { get; set; }
    }
}
