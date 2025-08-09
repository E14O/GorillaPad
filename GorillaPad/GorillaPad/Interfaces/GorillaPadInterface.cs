using GorillaPad.Functions;
using GorillaPad.Logic.UI;
using GorillaPad.Logic.Screens;
using UnityEngine;

namespace GorillaPad.Interfaces
{
    public class GorillaPadInterface : MonoBehaviour
    {
        private bool GPUnlocked = false;
        private bool GPPoweredOn = false;

        void Start()
        {
            GameObject MainButton = ContentLoader.GorillaPadMainParent.transform.GetChild(1).GetChild(7).gameObject;
            GameObject PowerButton = ContentLoader.GorillaPadMainParent.transform.GetChild(1).GetChild(3).gameObject;
            GameObject VolUp = ContentLoader.GorillaPadMainParent.transform.GetChild(1).GetChild(6).gameObject;
            GameObject VolDown = ContentLoader.GorillaPadMainParent.transform.GetChild(1).GetChild(5).gameObject;

            GorillaPadButton.Create(MainButton, MainButtonFunction);
            GorillaPadButton.Create(PowerButton, PowerButtonFunction);
            GorillaPadButton.Create(VolUp, VolumeUpButtonFunction);
            GorillaPadButton.Create(VolDown, VolumeDownButtonFunction);
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
                ContentLoader.PowerSound.GetComponent<AudioSource>().Play();
                GPPoweredOn = false;
            }
            else if (!GPPoweredOn)
            {
                ScreenManager.LockScreen.SetActive(true); ScreenManager.TopBar.SetActive(true);
                ContentLoader.PowerSound.GetComponent<AudioSource>().Play();
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
