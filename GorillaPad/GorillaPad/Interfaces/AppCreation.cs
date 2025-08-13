using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GorillaPad.Functions.UI;
using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad.Interfaces
{
    internal class AppCreation : MonoBehaviour
    {
        private List<AppSystem> Apps = new();
        public static GameObject AppParent;
        private static string PFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string GorillaPadAppFolder = Path.Combine(PFolder, "GorillaPadApps");

        public void Start()
        {
            Directory.CreateDirectory(GorillaPadAppFolder);

            AppParent = ContentLoader.BundleParent.transform.GetChild(2).GetChild(1).GetChild(3).gameObject;


            var appTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(AppSystem)) && !t.IsAbstract);

            foreach (var type in appTypes)
            {
                Apps.Add((AppSystem)Activator.CreateInstance(type));
            }
            ApplicationCreation();
        }

        public void ApplicationCreation()
        {
            string[] AllApps = Directory.GetFiles(GorillaPadAppFolder, "*.app");

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

    }

}
