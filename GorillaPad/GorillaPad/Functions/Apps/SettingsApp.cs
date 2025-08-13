using System;
using System.Collections.Generic;
using System.Text;
using GorillaPad.Interfaces;

namespace GorillaPad.Functions.Apps
{
    internal class SettingsApp : AppSystem
    {
        // App That Cannot Be Removed.
        public override string AppName => "Settings";
        public override string AppVersion => "0.0.1";

        public override void OnAppOpen()
        {
            base.OnAppOpen();
        }

        public override void AppContent()
        {
            base.AppContent();
        }
    }
}
