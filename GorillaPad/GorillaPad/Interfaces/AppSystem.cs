using GorillaPad.Functions.Managers;
using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad.Interfaces
{
    public abstract class AppSystem
    {
        public abstract string AppName { get; }
        public abstract string AppVersion { get; }

        public static bool _AppOpen = false;
        public static bool LastAppState = false;

        public virtual void OnAppOpen()
        {
            ScreenManager.HomeScreen.SetActive(false);
            _AppOpen = true;
        }
        public static void OnAppClose()
        {
            // new model please put all the screens in a separate parent so i can just set them all to false when a app closes :)
            var ScoreboardScreen = ContentLoader.BundleParent.transform.GetChild(2).GetChild(3).gameObject;
            ScoreboardScreen.SetActive(false);
            // ^ Will be changed to what said above code ^
        }

        public virtual void AppContent()
        {

        }
    }
}
