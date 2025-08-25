using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GorillaPad.Functions.Apps;
using GorillaPad.Functions.UI;
using GorillaPad.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GorillaPad.Interfaces
{
    public class AppCreation : MonoBehaviour
    {
        private readonly List<AppModule> Apps = new();
        public static GameObject AppParent, ScreenParent;
        private readonly string FolderPathApps = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Apps");
        private readonly string FolderPathDLL = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MainApp");

        public void Start()
        {
            if (!Directory.Exists(FolderPathApps))
                Directory.CreateDirectory(FolderPathApps);

            if (!Directory.Exists(FolderPathDLL))
                Directory.CreateDirectory(FolderPathDLL);

            ScreenParent = ContentLoader.BundleParent.transform.GetChild(1).transform.Find("AppInterfaces").gameObject;
            AppParent = ContentLoader.BundleParent.transform.GetChild(1).GetChild(6).transform.Find("GridLayout").gameObject;

            var appTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(AppModule)) && !t.IsAbstract);
            foreach (var type in appTypes)
                Apps.Add((AppModule)Activator.CreateInstance(type));

            CreateApplication();
            CreateDefaultApps();
        }

        public void CreateApplication()
        {
            string[] AppPath = Directory.GetFiles(FolderPathApps, "*.app");
            string[] DllPath = Directory.GetFiles(FolderPathDLL, "*.dll");

            foreach (string App in AppPath)
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(App);
                string Bundle = Path.GetFileNameWithoutExtension(App);

                GameObject CustomApp = Instantiate(assetBundle.LoadAsset<GameObject>(Bundle));
                GameObject AppIcon = CustomApp.transform.Find($"{Bundle}Icon").gameObject;
                GameObject AppScreen = CustomApp.transform.Find($"{Bundle}App").gameObject;
                GameObject AppText = AppParent.transform.Find("CreditsIcon/Text").gameObject;

                if (AppIcon == null || AppScreen == null || AppText == null)
                {
                    Destroy(CustomApp);
                    continue;
                }

                GameObject NewText = Instantiate(AppText);
                NewText.transform.SetParent(AppIcon.transform, false);
                var TextToChange = NewText.GetComponent<Text>();

                var matchedApp = Apps.FirstOrDefault(app => app.GetType().Name == Bundle);
                TextToChange.text = matchedApp != null ? matchedApp.AppName : Bundle;

                AppIcon.transform.SetParent(AppParent.transform, false);
                AppIcon.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                AppIcon.GetComponent<RectTransform>().localScale = Vector3.one * 0.9919f;

                AppScreen.SetActive(false);
                AppScreen.transform.SetParent(ScreenParent.transform, false);

                Destroy(CustomApp);
            }

            foreach (string Dll in DllPath)
            {
                var GetApp = Assembly.LoadFile(Dll).GetTypes().Where(t => t.IsSubclassOf(typeof(AppModule)) && !t.IsAbstract);
                foreach (var type in GetApp)
                {
                    var App = (AppModule)Activator.CreateInstance(type);
                    Apps.Add(App);

                    Transform parent = ContentLoader.Bundle.transform.GetChild(0).GetChild(1).GetChild(6).GetChild(1);
                    PadButton.Create(parent, $"{App.AppName}Icon", SelectedAudio.ButtonAudio, App.OnAppOpen);
                }
            }
        }

        public void CreateDefaultApps()
        {
            Transform parent = ContentLoader.Bundle.transform.GetChild(0)?.GetChild(1)?.GetChild(6)?.GetChild(1);
            var CreditsApp = Apps.FirstOrDefault(creditsapp => creditsapp is CreditsApp);
            PadButton.Create(parent, "CreditsIcon", SelectedAudio.ButtonAudio, CreditsApp.OnAppOpen);

            var SettingsApp = Apps.FirstOrDefault(settapp => settapp is SettingsApp);
            PadButton.Create(parent, "SettingsIcon", SelectedAudio.ButtonAudio, SettingsApp.OnAppOpen);
        }
    }
}