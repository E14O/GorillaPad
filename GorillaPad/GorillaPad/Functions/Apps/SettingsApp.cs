using GorillaPad.Interfaces;
using System;
using System.Collections.Generic;

namespace GorillaPad.Functions.Apps
{
    internal class SettingsApp : AppModule
    {
        public override string AppName => "Settings";
        public override string AppVersion => "0.0.1";

        public override void OnAppOpen()
        {
            base.OnAppOpen();
            AppContent();
        }

        public override void AppContent()
        {
            base.AppContent();

            Dictionary<string, Action> AllSettings = new()
            {
                { "Pad Colour", ChangePadColour },
                { "Test", RunTest }
            };

            foreach (var setting in AllSettings)
            {
                string Title = setting.Key;
                Action Action = setting.Value;
            }
        }

        private void ChangePadColour()
        {

        }

        private void RunTest()
        {

        }
    }
}