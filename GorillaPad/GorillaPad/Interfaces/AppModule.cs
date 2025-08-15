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

        private static bool isAnimating = false;
        private static float animationTime = 0f;
        private static float animationDuration = 0.3f;
        private static bool isOpening = false;
        private static GameObject currentAppObject = null;

        public virtual void OnAppOpen()
        {
            ScreenManager.HomeScreen.SetActive(false);
            
            currentAppObject = ContentLoader.BundleParent.transform.Find($"Canvas/AppInterfaces/{AppName}App").gameObject;
            
            StartAppAnimation(true);
            
            AppOpen = true;
        }

        public static void OnAppClose()
        {
            StartAppAnimation(false);
        }

        private static void StartAppAnimation(bool opening)
        {
            if (isAnimating) return;
            
            isAnimating = true;
            animationTime = 0f;
            isOpening = opening;
            
            if (opening)
            {
                currentAppObject.SetActive(true);
                SetupAppForAnimation(currentAppObject, true);
            }
            else
            {
                SetupAppForAnimation(currentAppObject, false);
            }
        }

        private static void SetupAppForAnimation(GameObject appObject, bool isOpening)
        {
            if (appObject == null) return;
            
            CanvasGroup canvasGroup = appObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = appObject.AddComponent<CanvasGroup>();
            
            if (isOpening)
            {
                canvasGroup.alpha = 0f;
                appObject.transform.localScale = Vector3.zero;
            }
            else
            {
                canvasGroup.alpha = 1f;
                appObject.transform.localScale = Vector3.one;
            }
        }

        public static void UpdateAppAnimation()
        {
            if (!isAnimating || currentAppObject == null) return;
            
            animationTime += Time.deltaTime;
            float progress = Mathf.Clamp01(animationTime / animationDuration);
            
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);
            
            CanvasGroup canvasGroup = currentAppObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null) return;
            
            if (isOpening)
            {
                canvasGroup.alpha = easedProgress;
                currentAppObject.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, easedProgress);
            }
            else
            {
                canvasGroup.alpha = 1f - easedProgress;
                currentAppObject.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, easedProgress);
            }
            
            if (progress >= 1f)
            {
                if (isOpening)
                {
                    canvasGroup.alpha = 1f;
                    currentAppObject.transform.localScale = Vector3.one;
                }
                else
                {
                    foreach (Transform app in Main.instance.AppInterfaces.transform)
                    {
                        app.gameObject.SetActive(false);
                    }
                }
                
                isAnimating = false;
                currentAppObject = null;
            }
        }

        public virtual void AppContent()
        {

        }
    }
}
