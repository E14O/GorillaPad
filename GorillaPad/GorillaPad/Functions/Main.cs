using System;
using GorillaPad.Functions.Managers;
using GorillaPad.Functions.UI;
using GorillaPad.Interfaces;
using GorillaPad.Tools;
using Photon.Pun;
using UnityEngine;

namespace GorillaPad.Functions
{
    internal class Main : MonoBehaviour
    {
        public static Main instance;
        public GameObject AppInterfaces, PadColour;
        private bool LastState = false;

        private bool IsUnlocked = false;
        private bool SetPower = false;

        void Start()
        {
            instance = this;
            GorillaTagger.OnPlayerSpawned(Initialization);
        }

        void Initialization()
        {
            ContentLoader.InitialiseContent();
            try
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { Constants.CustomProp, Constants.Version } });
            }
            catch (Exception e)
            {
                PadLogging.LogError($"Setting Custom Prop Was Interrupted {e}");
                return;
            }
            ContentLoader.Bundle.AddComponent<ScreenManager>();
            ContentLoader.BundleParent.AddComponent<PadHolding>();

            AppInterfaces = ContentLoader.Bundle.transform.Find("Pad/Canvas/AppInterfaces").gameObject;
            PadColour = ContentLoader.Bundle.transform.Find("Pad/Model").gameObject;

            Transform parent = ContentLoader.BundleParent.transform.GetChild(1);
            PadButton.Create(parent, "HomeButton", SelectedAudio.ButtonAudio, ToggleMainFunction);
            PadButton.Create(parent, "PowerButton", SelectedAudio.PowerAudio, TogglePower);
            PadButton.Create(parent, "Volume+", SelectedAudio.ButtonAudio, IncreaseVolume);
            PadButton.Create(parent, "Volume-", SelectedAudio.ButtonAudio, DecreaseVolume);
        }

        void Update()
        {
            if (LastState && !AppModule.AppOpen)
            {
                AppModule.OnAppClose();
            }
            LastState = AppModule.AppOpen;

            if (GorillaTagger.Instance.offlineVRRig != null)
            {
                PadColour.GetComponent<MeshRenderer>().material.color = GorillaTagger.Instance.offlineVRRig.playerColor;
            }
        }

        void ToggleMainFunction()
        {
            if (!SetPower)
                return;

            if (AppModule.AppOpen)
            {
                AppModule.AppOpen = false;
                ScreenManager.LockScreen.SetActive(false);
                ScreenManager.HomeScreen.SetActive(true);
                IsUnlocked = true;
                return;
            }

            if (!IsUnlocked)
            {
                ScreenManager.LockScreen.SetActive(false);
                ScreenManager.HomeScreen.SetActive(true);
                IsUnlocked = true;
                return;
            }

            ScreenManager.LockScreen.SetActive(true);
            ScreenManager.HomeScreen.SetActive(false);
            IsUnlocked = false;
        }

        void TogglePower()
        {
            if (SetPower)
            {
                ScreenManager.LockScreen.SetActive(false);
                ScreenManager.HomeScreen.SetActive(false);
                ScreenManager.TopBar.SetActive(false);

                IsUnlocked = false;
                foreach (Transform app in Main.instance.AppInterfaces.transform)
                {
                    app.gameObject.SetActive(false);
                }

                SetPower = false;
            }
            else
            {
                ScreenManager.LockScreen.SetActive(true);
                ScreenManager.TopBar.SetActive(true);
                SetPower = true;
            }
        }

        void IncreaseVolume()
        {

        }

        void DecreaseVolume()
        {

        }
    }
}
