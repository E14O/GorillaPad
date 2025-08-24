using GorillaPad.Functions;
using GorillaPad.Functions.Managers;
using UnityEngine;

namespace GorillaPad.Interfaces
{
    public abstract class AppModule
    {
        public abstract string AppName { get; }
        public abstract string AppVersion { get; }

        public static bool AppOpen = false;
        private static GameObject currentAppObject = null;
        private static AppModule currentAppModule = null;

        public virtual void OnAppOpen()
        {
            ScreenManager.HomeScreen.SetActive(false);

            currentAppObject = Main.instance.AppInterfaces.transform.Find($"{AppName}App").gameObject;
            if (currentAppObject == null) return;

            if (!currentAppObject.activeSelf)
                currentAppObject.SetActive(true);

            AnimationManager.CreateAnimation(ScreenManager.HomeScreen, currentAppObject, true);

            AppOpen = true;
            currentAppModule = this;
        }

        public static void OnAppClose()
        {
            if (currentAppObject == null) return;

            AnimationManager.CreateAnimation(currentAppObject, ScreenManager.HomeScreen, true);

            AppOpen = false;
            currentAppObject = null;
            currentAppModule = null;
        }

        public virtual void AppContent()
        {

        }
        public virtual void Tick() { }

        public static void TickCurrent()
        {
            if (currentAppModule != null && AppOpen)
                currentAppModule.Tick();
        }
    }
}
