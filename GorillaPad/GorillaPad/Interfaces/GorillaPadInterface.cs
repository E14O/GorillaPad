using GorillaPad.Functions.Screens;
using GorillaPad.Functions.UI;
using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad.Interfaces
{
    public class GorillaPadInterface : MonoBehaviour
    {
        private bool GPUnlocked = false;
        private bool GPPoweredOn = false;

        void Start()
        {
            GameObject MainButton = ContentLoader.BundleParent.transform.GetChild(1).GetChild(7).gameObject;
            GameObject PowerButton = ContentLoader.BundleParent.transform.GetChild(1).GetChild(3).gameObject;
            GameObject VolUp = ContentLoader.BundleParent.transform.GetChild(1).GetChild(6).gameObject;
            GameObject VolDown = ContentLoader.BundleParent.transform.GetChild(1).GetChild(5).gameObject;

            PadButton.Create(MainButton, SelectedAudio.ButtonAudio, MainButtonFunction);
            PadButton.Create(VolUp, SelectedAudio.ButtonAudio, VolumeUpButtonFunction);
            PadButton.Create(VolDown, SelectedAudio.ButtonAudio, VolumeDownButtonFunction);
            PadButton.Create(PowerButton, SelectedAudio.PowerAudio, PowerButtonFunction);
        }

        void MainButtonFunction()
        {
            if (AppSystem._AppOpen)
            {
                AppSystem._AppOpen = false;
                ScreenManager.LockScreen.SetActive(false);
                ScreenManager.HomeScreen.SetActive(true);
                AppSystem.OnAppOpened
                GPUnlocked = true;
            }
            else if (GPUnlocked)
            {
                ScreenManager.LockScreen.SetActive(true);
                ScreenManager.HomeScreen.SetActive(false);
                GPUnlocked = false;
            }
            else
            {
                ScreenManager.LockScreen.SetActive(false);
                ScreenManager.HomeScreen.SetActive(true);
                GPUnlocked = true;
            }

        }


        void PowerButtonFunction()
        {
            if (GPPoweredOn)
            {
                ScreenManager.LockScreen.SetActive(false); ScreenManager.HomeScreen.SetActive(false); ScreenManager.TopBar.SetActive(false);
                GPPoweredOn = false;
            }
            else if (!GPPoweredOn)
            {
                ScreenManager.LockScreen.SetActive(true); ScreenManager.TopBar.SetActive(true);
                GPPoweredOn = true;
            }
        }

        void VolumeUpButtonFunction()
        {

        }

        void VolumeDownButtonFunction()
        {

        }
    }
}
