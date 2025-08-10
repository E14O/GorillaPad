using System;
using GorillaPad.Tools;
using TMPro;
using UnityEngine;

namespace GorillaPad.Functions.Screens
{
    public class ScreenManager : MonoBehaviour
    {
        public static GameObject LockScreen, TopBar, HomeScreen;

        void Start()
        {
            HomeScreen = ContentLoader.BundleParent.transform.GetChild(2).GetChild(1).gameObject;
            LockScreen = ContentLoader.BundleParent.transform.GetChild(2).GetChild(0).gameObject;
            TopBar = ContentLoader.BundleParent.transform.GetChild(0).gameObject;

            SetupTopBar();
        }

        void SetupTopBar()
        {
            TopBar.transform.GetChild(0).gameObject.SetActive(false);
            TopBar.transform.GetChild(2).gameObject.SetActive(false);
            TopBar.SetActive(false);
        }

        void Update()
        {
            DateTime DT = DateTime.Now;
            TopBar.transform.GetChild(8).GetComponent<TextMeshPro>().text = DT.ToString("hh:mm tt").ToUpper();
            LockScreen.transform.GetChild(2).GetComponent<TextMeshPro>().text = DT.ToString("hh:mm tt").ToUpper();
            LockScreen.transform.GetChild(3).GetComponent<TextMeshPro>().text = DT.ToString("dddd dd MMMM");
        }
    }
}
