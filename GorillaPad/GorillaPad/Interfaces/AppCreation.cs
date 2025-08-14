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
        public static GameObject AppParent;
        private readonly string FolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Apps");

        public void Start()
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

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
                string prefabName = Path.GetFileNameWithoutExtension(appfile) + ".prefab";
                GameObject CustomApp = Instantiate(assetBundle.LoadAsset<GameObject>(prefabName));
                GameObject CustomAppIcon = CustomApp.transform.GetChild(0).gameObject;
                GameObject CustomAppScreen = CustomApp.transform.GetChild(1).gameObject;
                CustomAppIcon.transform.SetParent(AppParent.transform, false);

                // CustomAppScreen.transform.SetParent(Screens.transform, false);
                // Also need to set the screen false. & disable all colliders
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
