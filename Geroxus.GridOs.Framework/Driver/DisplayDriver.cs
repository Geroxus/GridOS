using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public class DisplayDriver : GridDriverBase<EnrichedTextSurface>
    {
        public GridProgram Program { get; private set; }
        public string Settings { get; private set; }

        private IMyTextSurface _component;
        private string _lastContent = String.Empty;


        /**
         * Write to this member to tell the Driver what to write onto the component.
         * The component will NOT redraw if nothing changed.
         */
        public StringBuilder DisplayText { get; } = new StringBuilder();

        public long ParentId { get; set; }
        public int SurfaceIdentifiert { get; set; }

        public override void Run()
        {
            // PollSettingsUpdate();
            switch (Program)
            {
                case GridProgram.NONE:
                    _component.WriteText(DisplayText);
                    DisplayText.Clear();
                    break;
                case GridProgram.LiveStats:
                    CheckBeforeUpdate();
                    break;
            }
        }

        private void PollSettingsUpdate()
        {
            // currently this brakes the UI. Why? I would love to know too!
            // TODO should test if there is an update provider and then poll the update here
            if (false)
            {
                // poll settings update 
            }
            else
            {
                var services = OsProcessBridge.Instance.GetServices(typeof(ScanCustomDataService));
                if (services.Count > 1)
                    throw new Exception($"There shouldn't be more than one {typeof(ScanCustomDataService).ToString()}");
                else if (services.Count == 1)
                    Settings = ((ScanCustomDataService)services.Single())
                        .GetUpdates(ParentId);
            }
        }

        private void CheckBeforeUpdate()
        {
            if (!DisplayText.ToString().Equals(_lastContent))
            {
                _component.WriteText(DisplayText);
                _lastContent = DisplayText.ToString();
            }

            DisplayText.Clear();
        }


        public override void SetUp()
        {
            Program = Component.Program;
            Settings = Component.Settings;
            _component = Component.Component;

            _component.ContentType = ContentType.TEXT_AND_IMAGE;
            _component.WriteText("");
        }

        public void AppendLine(string text)
        {
            DisplayText.AppendLine(text);
        }
    }
}