using System;
using GorillaPad.Tools;
using TMPro;
using UnityEngine;

namespace GorillaPad.Functions.Managers
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
            string dayending = "th";
            if (DT.Day == 1 || DT.Day == 21 || DT.Day == 31) dayending = "st";
            else if (DT.Day == 2 || DT.Day == 22) dayending = "nd";
            else if (DT.Day == 3 || DT.Day == 23) dayending = "rd";

            TopBar.transform.GetChild(8).GetComponent<TextMeshPro>().text = DT.ToString("hh:mm tt").ToUpper();
            LockScreen.transform.GetChild(2).GetComponent<TextMeshPro>().text = DT.ToString("hh:mm").ToUpper();
            LockScreen.transform.GetChild(3).GetComponent<TextMeshPro>().text = $"{DT:dddd}, {DT:dd}{dayending}, {DT:MMMM}";
        }
    }
}
