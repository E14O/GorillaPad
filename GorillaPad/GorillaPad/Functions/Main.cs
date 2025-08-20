using System;
using System.Collections;
using GorillaPad.Functions.Managers;
using GorillaPad.Functions.UI;
using GorillaPad.Interfaces;
using GorillaPad.Tools;
using Photon.Pun;
using UnityEngine;
using BepInEx.Configuration;

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
                PadLogging.LogError($"Unable To Set Custom Prop, It Was Interrupted {e}");
                return;
            }

            ContentLoader.BundleParent.AddComponent<ScreenManager>();
            ContentLoader.BundleParent.AddComponent<PadHolding>();
            ContentLoader.BundleParent.AddComponent<AppCreation>();

            AppInterfaces = ContentLoader.BundleParent.transform.Find("Canvas/AppInterfaces").gameObject;
            PadColour = ContentLoader.BundleParent.transform.Find("Model").gameObject;

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
            if (!SetPower) return;

            if (AppModule.AppOpen)
            {
                AppModule.AppOpen = false;
                AppModule.OnAppClose();
                return;
            }

            GameObject activeApp = null;
            foreach (Transform child in AppCreation.ScreenParent.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    activeApp = child.gameObject;
                    break;
                }
            }

            if (ScreenManager.LockScreen.activeSelf)
            {
                AnimationManager.CreateAnimation(ScreenManager.LockScreen, ScreenManager.HomeScreen, true);
                IsUnlocked = true;
            }
            else if (ScreenManager.HomeScreen.activeSelf)
            {
                AnimationManager.CreateAnimation(ScreenManager.HomeScreen, ScreenManager.LockScreen, true);
                IsUnlocked = false;
            }
            else if (activeApp != null)
            {
                AnimationManager.CreateAnimation(activeApp, ScreenManager.HomeScreen, true);
                IsUnlocked = true;
            }
        }
        void TogglePower()
        {
            if (SetPower)
            {
                if (ScreenManager.LockScreen.activeSelf)
                {
                    AnimationManager.CreateAnimation(ScreenManager.LockScreen, null, false);
                    ScreenManager.LockScreen.SetActive(false);
                }

                if (ScreenManager.HomeScreen.activeSelf)
                {
                    AnimationManager.CreateAnimation(ScreenManager.HomeScreen, null, false);
                    ScreenManager.HomeScreen.SetActive(false);
                }

                ScreenManager.TopBar.SetActive(false);

                foreach (Transform app in AppInterfaces.transform)
                    app.gameObject.SetActive(false);

                IsUnlocked = false;
                SetPower = false;
            }
            else
            {
                ScreenManager.TopBar.SetActive(true);
                ScreenManager.HomeScreen.SetActive(false);
                ScreenManager.LockScreen.SetActive(true);

                StartCoroutine(PlayLockScreenAnimation());

                IsUnlocked = false;
                SetPower = true;
            }
        }

        private IEnumerator PlayLockScreenAnimation()
        {
            yield return null;
            AnimationManager.CreateAnimation(ScreenManager.LockScreen, null, true);
        }

        void IncreaseVolume()
        {

        }

        void DecreaseVolume()
        {

        }

        public static void ReturnPad()
        {
            ContentLoader.BundleParent.transform.SetPositionAndRotation(Constants.Asset.Position, Constants.Asset.Rotation);
            ContentLoader.BundleParent.transform.localScale = Constants.Asset.Scale;
            var ReturnButtonScript = ContentLoader.SignParent.transform.Find("Sign");
            ReturnButtonScript.GetComponent<PadButton>().Destroy();
        }
    }
}
