using System;
using System.Collections.Generic;
using GorillaPad.Functions.Managers;
using GorillaPad.Functions.UI;
using GorillaPad.Interfaces;
using UnityEngine;

namespace GorillaPad.Functions.Apps
{
    internal class SettingsApp : AppModule
    {
        // App That Cannot Be Removed.
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
                // Find defualt template, duplicate and move below the last one, change text (Title) then find button and add the script below 
                // if settings get more then the set amount auto create a new screen and screen switch function.
                //PadButton.Create(, "ClickSound", action);
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
