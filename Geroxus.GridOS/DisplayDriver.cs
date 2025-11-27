using System;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public class DisplayDriver : IDisplayDriver
    {
        public string Name { get; }
        public ProcessId ProcessId { get; }
        public GridProgram Program { get; }
        public string Settings { get; }
        
        private readonly IMyTextSurface _component;
        private string _lastContent = String.Empty;

        private StringBuilder DisplayText { get; } = new StringBuilder();


        public DisplayDriver(EnrichedTextSurface surface, ProcessId processId, string name)
        {
            _component = surface.Component;
            Program = surface.Program;
            Settings = surface.Settings;

            _component.ContentType = ContentType.TEXT_AND_IMAGE;
            _component.WriteText("");
            
            Name = name;
            ProcessId = processId;
        }

        public void Run()
        {
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

        private void CheckBeforeUpdate()
        {
            if (!DisplayText.ToString().Equals(_lastContent))
            {
               _component.WriteText(DisplayText);
               _lastContent = DisplayText.ToString();
            } 
            DisplayText.Clear();
        }
        

        public void SetUp()
        {
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }

        public void AppendLine(string text)
        {
            DisplayText.AppendLine(text);
        }
    }
}