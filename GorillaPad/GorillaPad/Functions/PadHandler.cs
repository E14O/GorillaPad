using System;
using System.Collections;
using GorillaNetworking;
using GorillaPad.Functions.Managers;
using GorillaPad.Functions.UI;
using GorillaPad.Interfaces;
using GorillaPad.Tools;
using Photon.Pun;
using UnityEngine;

namespace GorillaPad.Functions
{
    public class PadHandler : MonoBehaviour
    {
        public static PadHandler instance;

        public GameObject AppInterfaces;
        public GameObject PadColour;

        private bool LastState = false;
        private bool IsUnlocked = false;
        private bool SetPower = false;
        private bool IsReturning = false;

        private float CurrentVolume = 0.5f;
        private const float VolumeStep = 0.1f;

        private AudioSource PowerAudio;
        private AudioSource ButtonAudio;
        private AudioSource BuzzAudio;

        void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            GorillaTagger.OnPlayerSpawned(Initialization);
        }

        void Initialization()
        {
            ContentLoader.InitialiseContent();
            try
            {
                var Properties = new ExitGames.Client.Photon.Hashtable
                {
                    { "GPHolding", false },
                    { "GPIsLeft", false },
                    { "GPMounted", true },
                    { Constants.CustomProp, Constants.Version }
                };

                PhotonNetwork.LocalPlayer.SetCustomProperties(Properties);
            }
            catch (Exception e)
            {
                PadLogging.LogError($"Failed to set custom properties: {e}");
                return;
            }

            ContentLoader.BundleParent.AddComponent<ScreenManager>();
            ContentLoader.BundleParent.AddComponent<PadHolding>();
            ContentLoader.BundleParent.AddComponent<AppCreation>();
            ContentLoader.BundleParent.AddComponent<NotificationManager>();

            AppInterfaces = ContentLoader.BundleParent.transform.Find("Canvas/AppInterfaces").gameObject;
            PadColour = ContentLoader.BundleParent.transform.Find("Model").gameObject;

            ContentLoader.GetSounds(ref PowerAudio, ref ButtonAudio, ref BuzzAudio);
            ApplyVolume();

            Transform buttonParent = ContentLoader.BundleParent.transform.Find("Canvas");

            PadButton.Create(buttonParent, "HomeButton", SelectedAudio.ButtonAudio, ToggleMainFunction);
            PadButton.Create(buttonParent, "PowerButton", SelectedAudio.PowerAudio, TogglePower);
            PadButton.Create(buttonParent, "Volume+", SelectedAudio.ButtonAudio, IncreaseVolume);
            PadButton.Create(buttonParent, "Volume-", SelectedAudio.ButtonAudio, DecreaseVolume);
        }

        void Update()
        {
            if (LastState && !AppModule.AppOpen)
                AppModule.OnAppClose();

            LastState = AppModule.AppOpen;

            var distance = Vector3.Distance(ContentLoader.BundleParent.transform.position, ContentLoader.SignParent.transform.position);
            if (distance <= .2f && PadHolding.LetGo == true && !IsReturning)
            {
                IsReturning = true;
                StartCoroutine(ReturnRoutine());
            }

            if (ContentLoader.BundleLoaded)
            {
                if (GorillaTagger.Instance.offlineVRRig != null)
                    PadColour.GetComponent<MeshRenderer>().material.color = GorillaTagger.Instance.offlineVRRig.playerColor;

                AppModule.TickCurrent();
            }
        }

        void ToggleMainFunction()
        {
            if (!SetPower)
                return;

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
                    AnimationManager.CreateAnimation(ScreenManager.LockScreen, null, false);

                if (ScreenManager.HomeScreen.activeSelf)
                    AnimationManager.CreateAnimation(ScreenManager.HomeScreen, null, false);

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


                if (instance != null && instance.isActiveAndEnabled)
                    instance.StartCoroutine(instance.PlayLockScreenAnimation());
                else
                    PadLogging.LogError("Animation coroutine failed");

                IsUnlocked = false;
                SetPower = true;
            }
        }

        void IncreaseVolume()
        {
            if (!SetPower)
                return;

            CurrentVolume = Mathf.Clamp01(CurrentVolume + VolumeStep);
            ApplyVolume();

            PadLogging.LogInfo($"Volume Increased: {Mathf.RoundToInt(CurrentVolume * 100)}%");
        }

        void DecreaseVolume()
        {
            if (!SetPower)
                return;

            CurrentVolume = Mathf.Clamp01(CurrentVolume - VolumeStep);
            ApplyVolume();

            PadLogging.LogInfo($"Volume Decreased: {Mathf.RoundToInt(CurrentVolume * 100)}%");
        }

        private void ApplyVolume()
        {
            if (PowerAudio != null) PowerAudio.volume = CurrentVolume;
            if (ButtonAudio != null) ButtonAudio.volume = CurrentVolume;
        }

        private IEnumerator PlayLockScreenAnimation()
        {
            yield return null;
            AnimationManager.CreateAnimation(ScreenManager.LockScreen, null, true);
        }

        public static void ReturnPad()
        {
            var Props = new ExitGames.Client.Photon.Hashtable();
            ExtensionMethods.AddOrUpdate(Props, "GPMounted", true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(Props);
            instance?.StartCoroutine(instance.ReturnRoutine());
        }

        private IEnumerator ReturnRoutine()
        {
            var pad = ContentLoader.BundleParent;
            Vector3 startPos = pad.transform.position;
            Quaternion startRot = pad.transform.rotation;

            Vector3 targetPos = Constants.Asset.Position;
            Quaternion targetRot = Constants.Asset.Rotation;

            float t = 0f;

            while (t < 1f)
            {
                pad.transform.position = Vector3.Lerp(startPos, targetPos, t);
                pad.transform.rotation = Quaternion.Lerp(startRot, targetRot, t);

                t += Time.deltaTime * 3f;
                yield return null;
            }

            pad.transform.SetParent(ContentLoader.Bundle.transform, false);
            pad.transform.position = Constants.Asset.Position;
            pad.transform.rotation = Constants.Asset.Rotation;
            pad.transform.localScale = Constants.Asset.Scale;

            PadHolding.LetGo = false;
            IsReturning = false;
        }
    }
}