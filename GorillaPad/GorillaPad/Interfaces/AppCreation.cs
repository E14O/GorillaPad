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
        private List<AppSystem> Apps = new List<AppSystem>();
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
            foreach (var App in Apps)
            {
                if (App.AppName == "Scoreboard")
                {
                    PadLogging.LogError($"{Apps}");
                    PadLogging.LogError($"CREATING SCOREBOARD APP");
                    var SBAPP = AppParent.transform.GetChild(1).gameObject;
                    SBAPP.SetActive(true);
                    PadButton.Create(SBAPP, SelectedAudio.ButtonAudio, App.OnAppOpen);
                }
                else if (App.AppName == "Settings")
                {

                }
                else if (App.AppName == "Music")
                {

                }
                else if (App.AppName == "Credits")
                {

                }
                else
                {
                  
                }
            }
        }
    }

}
