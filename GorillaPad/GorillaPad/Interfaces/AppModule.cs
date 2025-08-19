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

        private static GameObject currentAppObject = null;

        public virtual void OnAppOpen()
        {
            ScreenManager.HomeScreen.SetActive(false);

            currentAppObject = Main.instance.AppInterfaces.transform.Find($"{AppName}App").gameObject;
            if (currentAppObject == null) return;

            if (!currentAppObject.activeSelf)
                currentAppObject.SetActive(true);

            AnimationManager.CreateAnimation(ScreenManager.HomeScreen, currentAppObject, true);

            AppOpen = true;
        }

        public static void OnAppClose()
        {
            if (currentAppObject == null) return;

            AnimationManager.CreateAnimation(currentAppObject, ScreenManager.HomeScreen, true);

            AppOpen = false;
            currentAppObject = null;
        }

        public virtual void AppContent()
        {

        }
    }
}
