using GorillaPad.Functions.Screens;
using GorillaPad.Interfaces;
using GorillaPad.Tools;

namespace GorillaPad.Functions.Apps
{
    public class ScoreboardApp : AppSystem
    {
        public override string AppName => "Scoreboard";
        public override string AppVersion => "0.0.1";

        public override void OnAppOpen()
        {
            base.OnAppOpen();
            AppContent();
        }

        public override void AppContent()
        {
            var ScoreboardScreen = ContentLoader.BundleParent.transform.GetChild(2).GetChild(3).gameObject;
            ScoreboardScreen.SetActive(true);
        }

        public override void OnAppClose()
        {
            base.OnAppClose();
        }
    }
}
