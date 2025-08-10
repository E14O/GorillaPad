using System;
using System.Collections.Generic;
using System.Reflection;
using GorillaPad.Logic.UI;
using GorillaPad.Tools;
using System.Linq;
using UnityEngine;

namespace GorillaPad.Interfaces
{
    internal class AppCreation : MonoBehaviour
    {
        private List<AppSystem> Apps = new List<AppSystem>();
        private GameObject AppParent;

        public void Start()
        {
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
                    Debug.LogError($"{Apps}");
                    Debug.LogError("CREATING SCOREBOARD APP");
                    var SBAPP = AppParent.transform.GetChild(1).gameObject;
                    SBAPP.SetActive(true);
                    PadButton.Create(SBAPP, PadButtonSound.Button, App.OnAppOpen);

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
                    // Duplicate The "Defualt" App, Parent it to the main App Bar parent, change the app name to App.AppName.
                }
            }
        }
    }

}
