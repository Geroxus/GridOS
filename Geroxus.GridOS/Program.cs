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
        private GridOs _os;

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            _os = new GridOs();
            OsGridAccessBridge.Instance.RegisterGridTerminalSystem(GridTerminalSystem);
            LOGGER.RegisterOutput(s => Echo(s));
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {
            _os.Operate();
        }
    }

    public static class LOGGER
    {
        public static void RegisterOutput(Action<string> action)
        {
            Write = s => action($"[[Info]] {s}");
        }

        public static Action<string> Write { get; set; }
    }
}