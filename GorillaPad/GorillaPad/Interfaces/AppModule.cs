using GorillaPad.Functions;
using GorillaPad.Functions.Managers;
using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad.Interfaces
{
    public abstract class AppModule
    {
        public abstract string AppName { get; }
        public abstract string AppVersion { get; }

        public static bool AppOpen = false;
        public static bool LastAppState = false;

        public virtual void OnAppOpen()
        {
            ScreenManager.HomeScreen.SetActive(false);
            ContentLoader.BundleParent.transform.Find($"Canvas/AppInterfaces/{AppName}App").gameObject.SetActive(true);

            AppOpen = true;
        }

        public static void OnAppClose()
        {
            foreach (Transform app in Main.instance.AppInterfaces.transform)
            {
                app.gameObject.SetActive(false);
            }
        }

        public virtual void AppContent()
        {

        }
    }
}
