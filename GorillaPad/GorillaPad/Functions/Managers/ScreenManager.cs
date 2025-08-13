using System;
using GorillaPad.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GorillaPad.Functions.Managers
{
    public class ScreenManager : MonoBehaviour
    {
        public static GameObject LockScreen, TopBar, HomeScreen;

        void Start()
        {
            HomeScreen = ContentLoader.BundleParent.transform.GetChild(1).transform.Find("HomeScreen").gameObject;
            LockScreen = ContentLoader.BundleParent.transform.GetChild(1).transform.Find("LockScreen").gameObject;
            TopBar = ContentLoader.BundleParent.transform.GetChild(1).transform.Find("Topbar").gameObject;
        }
        void Update()
        {
            DateTime DT = DateTime.Now;
            string dayending = "th";
            if (DT.Day == 1 || DT.Day == 21 || DT.Day == 31) dayending = "st";
            else if (DT.Day == 2 || DT.Day == 22) dayending = "nd";
            else if (DT.Day == 3 || DT.Day == 23) dayending = "rd";

            TopBar.transform.Find("TBTime").GetComponent<Text>().text = DT.ToString("hh:mm tt").ToUpper();
            LockScreen.transform.Find("Time Text").GetComponent<Text>().text = DT.ToString("hh:mm").ToUpper();
            LockScreen.transform.Find("Date Text").GetComponent<Text>().text = $"{DT:dddd}, {DT:dd}{dayending}, {DT:MMMM}";
        }
    }
}
