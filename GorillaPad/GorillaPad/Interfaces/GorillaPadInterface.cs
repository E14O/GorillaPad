using GorillaPad.Logic.Screens;
using GorillaPad.Logic.UI;
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

            PadButton.Create(MainButton, MainButtonFunction);
            PadButton.Create(PowerButton, PowerButtonFunction);
            PadButton.Create(VolUp, VolumeUpButtonFunction);
            PadButton.Create(VolDown, VolumeDownButtonFunction);
        }

        void MainButtonFunction()
        {
            if (GPUnlocked && GPPoweredOn)
            {
                ScreenManager.LockScreen.SetActive(true); ScreenManager.HomeScreen.SetActive(false);
                GPUnlocked = false;
            }
            else if (!GPUnlocked && GPPoweredOn)
            {
                ScreenManager.LockScreen.SetActive(false); ScreenManager.HomeScreen.SetActive(true);
                GPUnlocked = true;
            }
        }

        void PowerButtonFunction()
        {
            if (GPPoweredOn)
            {
                ScreenManager.LockScreen.SetActive(false); ScreenManager.HomeScreen.SetActive(false); ScreenManager.TopBar.SetActive(false);

                AudioSource PowerAudio = null, ButtonAudio = null;
                ContentLoader.GetSounds(ref PowerAudio, ref ButtonAudio);
                PowerAudio.Play();

                GPPoweredOn = false;
            }
            else if (!GPPoweredOn)
            {
                ScreenManager.LockScreen.SetActive(true); ScreenManager.TopBar.SetActive(true);

                AudioSource PowerAudio = null, ButtonAudio = null;
                ContentLoader.GetSounds(ref PowerAudio, ref ButtonAudio);
                PowerAudio.Play();

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
