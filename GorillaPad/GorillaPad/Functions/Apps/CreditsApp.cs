using System;
using System.Collections.Generic;
using System.Text;
using GorillaPad.Interfaces;
using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad.Functions.Apps
{
    internal class CreditsApp : AppSystem
    {
        // App That Cannot Be Removed.
        public override string AppName => "Credits";
        public override string AppVersion => "0.0.1";

        public override void OnAppOpen()
        {
            base.OnAppOpen();
            AppContent();
        }

        public override void AppContent()
        {
            base.AppContent();
            Transform creditsScreen = ContentLoader.Bundle.transform.Find("Pad/Canvas/AppInterfaces/CreditsApp");
            creditsScreen.gameObject.SetActive(true);
        }
    }
}
