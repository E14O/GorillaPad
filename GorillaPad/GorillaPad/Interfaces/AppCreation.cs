using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GorillaPad.Functions.Apps;
using GorillaPad.Functions.UI;
using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad.Interfaces
{
    internal class AppCreation : MonoBehaviour
    {
        private readonly List<AppModule> Apps = new();
        public static GameObject AppParent, ScreenParent;
        private readonly string FolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Apps");

        public void Start()
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            ScreenParent = ContentLoader.BundleParent.transform.GetChild(1).transform.Find("AppInterfaces").gameObject;
            AppParent = ContentLoader.BundleParent.transform.GetChild(1).GetChild(6).transform.Find("GridLayout").gameObject;

            var appTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(AppModule)) && !t.IsAbstract);

            foreach (var type in appTypes)
            {
                Apps.Add((AppModule)Activator.CreateInstance(type));
            }

            CreateApplication();
            CreateDefaultApps();
        }

        public void CreateApplication()
        {
            string[] AllApps = Directory.GetFiles(FolderPath, "*.app");

            foreach (string appfile in AllApps)
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(appfile);
                string prefab = Path.GetFileNameWithoutExtension(appfile);

                GameObject CustomApp = Instantiate(assetBundle.LoadAsset<GameObject>(prefab));
                GameObject CustomAppIcon = CustomApp.transform.Find($"{prefab}Icon").gameObject;
                GameObject CustomAppScreen = CustomApp.transform.Find($"{prefab}Screen").gameObject;
                CustomAppScreen.SetActive(false);

                CustomAppIcon.transform.SetParent(AppParent.transform, false);
                CustomAppScreen.transform.SetParent(ScreenParent.transform, false);
                Destroy(CustomApp);
            }
        }

        public void CreateDefaultApps()
        {
            Transform parent = ContentLoader.Bundle.transform.GetChild(0).GetChild(1).GetChild(6).GetChild(1);

            var CreditsAppSystem = Apps.FirstOrDefault(creditsapp => creditsapp is CreditsApp);
            PadButton.Create(parent, "CreditsIcon", SelectedAudio.ButtonAudio, CreditsAppSystem.OnAppOpen);

            var SettingsAppSystem = Apps.FirstOrDefault(settapp => settapp is SettingsApp);
            PadButton.Create(parent, "SettingsIcon", SelectedAudio.ButtonAudio, SettingsAppSystem.OnAppOpen);
        }
    }

}
