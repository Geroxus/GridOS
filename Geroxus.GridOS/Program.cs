using System;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        private static readonly DateTime  BuildDate = new DateTime(2025, 4, 23);
        
        private readonly GridOs _os;

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            
            FlightCapability.Register();

            _os = GridOs.BootStrap(GridTerminalSystem);
            LOGGER.RegisterOutput(s => Echo(s));

            _os.Version = $"0.1-beta-{BuildDate.Date.ToShortDateString()}";
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {
            _os.Operate();
        }
    }
}