using System;
using System.Collections.Generic;
using System.Text;
using GorillaPad.Interfaces;
using GorillaPad.Tools;
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
        }
    }
}
