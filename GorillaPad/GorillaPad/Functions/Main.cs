using GorillaPad.Functions.Managers;
using GorillaPad.Functions.UI;
using GorillaPad.Interfaces;
using GorillaPad.Tools;
using Photon.Pun;
using System;
using System.Collections;
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

        private float currentVolume = 0.5f;
        private const float volumeStep = 0.1f;

        private AudioSource powerAudio;
        private AudioSource buttonAudio;

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

            ContentLoader.GetSounds(ref powerAudio, ref buttonAudio);
            ApplyVolume();

            Transform parent = ContentLoader.BundleParent.transform.GetChild(1);
            PadButton.Create(parent, "HomeButton", SelectedAudio.ButtonAudio, ToggleMainFunction);
            PadButton.Create(parent, "PowerButton", SelectedAudio.PowerAudio, TogglePower);
            PadButton.Create(parent, "Volume+", SelectedAudio.ButtonAudio, IncreaseVolume);
            PadButton.Create(parent, "Volume-", SelectedAudio.ButtonAudio, DecreaseVolume);
        }

        void Update()
        {
            if (LastState && !AppModule.AppOpen)
                AppModule.OnAppClose();

            LastState = AppModule.AppOpen;

            if (ContentLoader.BundleLoaded)
            {
                if (GorillaTagger.Instance.offlineVRRig != null)
                    PadColour.GetComponent<MeshRenderer>().material.color = GorillaTagger.Instance.offlineVRRig.playerColor;
            }
        }

        void ToggleMainFunction()
        {
            if (!SetPower)
            {
                TogglePower();
                if (SetPower)
                {
                    ScreenManager.LockScreen.SetActive(false);
                    ScreenManager.HomeScreen.SetActive(true);
                    IsUnlocked = true;
                }
                return;
            }

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
                    Debug.LogError("Main instance not available to run coroutine");

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
            currentVolume = Mathf.Clamp01(currentVolume + volumeStep);
            ApplyVolume();
            PadLogging.LogInfo($"Volume Increased: {Mathf.RoundToInt(currentVolume * 100)}%");
        }

        void DecreaseVolume()
        {
            currentVolume = Mathf.Clamp01(currentVolume - volumeStep);
            ApplyVolume();
            PadLogging.LogInfo($"Volume Decreased: {Mathf.RoundToInt(currentVolume * 100)}%");
        }

        private void ApplyVolume()
        {
            if (powerAudio != null) powerAudio.volume = currentVolume;
            if (buttonAudio != null) buttonAudio.volume = currentVolume;
        }

        public static void ReturnPad()
        {
            if (instance != null)
                instance.StartCoroutine(instance.ReturnRoutine());
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

            pad.transform.SetParent(null, false);
            pad.transform.localPosition = Vector3.zero;
            pad.transform.localRotation = Quaternion.identity;
            pad.transform.localScale = Constants.Asset.Scale;

            var returnButton = ContentLoader.SignParent.transform.Find("Sign");
            if (returnButton)
                returnButton.GetComponent<PadButton>().Destroy();
        }
    }
}