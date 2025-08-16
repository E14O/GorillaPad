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

        private bool isLockScreenLerping = false;
        private float lockScreenLerpStartTime = 0f;
        private float lockScreenLerpDuration = 0.08f;
        private bool lockScreenLerpIn = true;
        private CanvasGroup lockScreenCanvasGroup;

        private CanvasGroup homeScreenCanvasGroup;
        private bool isTransitioning = false;
        private float transitionStartTime = 0f;
        private float transitionDuration = 0.3f;
        private bool transitioningToHome = false;

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
                PadLogging.LogError($" While Setting The Custom Prop It Was Interrupted {e}");
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

            if (isLockScreenLerping)
            {
                UpdateLockScreenLerp();
            }

            if (isTransitioning)
            {
                UpdateScreenTransition();
            }

            AppModule.UpdateAppAnimation();
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
                StartScreenTransition(true);
                IsUnlocked = true;
                return;
            }

            StartScreenTransition(false); 
            IsUnlocked = false;
        }

        void TogglePower()
        {
            if (SetPower)
            {
                StartLockScreenLerp(false);
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
                ScreenManager.TopBar.SetActive(true);
                SetPower = true;
                StartLockScreenLerp(true);
            }
        }

        void IncreaseVolume()
        {

        }

        void DecreaseVolume()
        {

        }

		void StartLockScreenLerp(bool fadeIn)
		{
			GameObject lockScreen = ScreenManager.LockScreen;
			if (lockScreen == null)
				return;
			
			if (lockScreenCanvasGroup == null)
			{
				lockScreenCanvasGroup = lockScreen.GetComponent<CanvasGroup>();
				if (lockScreenCanvasGroup == null)
					lockScreenCanvasGroup = lockScreen.AddComponent<CanvasGroup>();
			}
			
			lockScreen.SetActive(true);
			isLockScreenLerping = true;
			lockScreenLerpStartTime = Time.time;
			lockScreenLerpIn = fadeIn;
			
			if (fadeIn)
			{
				lockScreen.transform.localScale = Vector3.zero;
				lockScreenCanvasGroup.alpha = 0f;
			}
			else
			{
				lockScreen.transform.localScale = Vector3.one;
				lockScreenCanvasGroup.alpha = 1f;
			}
		}

		void UpdateLockScreenLerp()
		{
			GameObject lockScreen = ScreenManager.LockScreen;
			if (lockScreen == null)
			{
				isLockScreenLerping = false;
				return;
			}
			float elapsed = Time.time - lockScreenLerpStartTime;
			float t = Mathf.Clamp01(elapsed / lockScreenLerpDuration);
			
			float eased = 1f - Mathf.Pow(1f - t, 2f);
			
			if (lockScreenLerpIn)
			{
				lockScreen.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, eased);
				if (lockScreenCanvasGroup != null)
					lockScreenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, eased);
			}
			else
			{
				lockScreen.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, eased);
				if (lockScreenCanvasGroup != null)
					lockScreenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, eased);
			}
			
			if (t >= 1f)
			{
				isLockScreenLerping = false;
				if (lockScreenLerpIn)
				{
					lockScreen.transform.localScale = Vector3.one;
					if (lockScreenCanvasGroup != null)
						lockScreenCanvasGroup.alpha = 1f;
				}
				else
				{
					lockScreen.SetActive(false);
					lockScreen.transform.localScale = Vector3.one;
					if (lockScreenCanvasGroup != null)
						lockScreenCanvasGroup.alpha = 1f;
				}
			}
		}

		void StartScreenTransition(bool toHome)
		{
			if (lockScreenCanvasGroup == null && ScreenManager.LockScreen != null)
			{
				lockScreenCanvasGroup = ScreenManager.LockScreen.GetComponent<CanvasGroup>();
				if (lockScreenCanvasGroup == null)
					lockScreenCanvasGroup = ScreenManager.LockScreen.AddComponent<CanvasGroup>();
			}
			if (homeScreenCanvasGroup == null && ScreenManager.HomeScreen != null)
			{
				homeScreenCanvasGroup = ScreenManager.HomeScreen.GetComponent<CanvasGroup>();
				if (homeScreenCanvasGroup == null)
					homeScreenCanvasGroup = ScreenManager.HomeScreen.AddComponent<CanvasGroup>();
			}

			if (lockScreenCanvasGroup == null || homeScreenCanvasGroup == null)
				return;

			isTransitioning = true;
			transitionStartTime = Time.time;
			transitioningToHome = toHome;

			if (toHome)
			{
				ScreenManager.LockScreen.SetActive(true);
				ScreenManager.HomeScreen.SetActive(true);
				lockScreenCanvasGroup.alpha = 1f;
				homeScreenCanvasGroup.alpha = 0f;
			}
			else
			{
				ScreenManager.LockScreen.SetActive(true);
				ScreenManager.HomeScreen.SetActive(true);
				lockScreenCanvasGroup.alpha = 0f;
				homeScreenCanvasGroup.alpha = 1f;
			}

			lockScreenCanvasGroup.interactable = false;
			lockScreenCanvasGroup.blocksRaycasts = false;
			homeScreenCanvasGroup.interactable = false;
			homeScreenCanvasGroup.blocksRaycasts = false;
		}

		void UpdateScreenTransition()
		{
			if (lockScreenCanvasGroup == null || homeScreenCanvasGroup == null)
			{
				isTransitioning = false;
				return;
			}

			float elapsed = Time.time - transitionStartTime;
			float t = Mathf.Clamp01(elapsed / transitionDuration);

			float eased = 1f - Mathf.Pow(1f - t, 2f);

			if (transitioningToHome)
			{
				lockScreenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, eased);
				homeScreenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, eased);
			}
			else
			{
				lockScreenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, eased);
				homeScreenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, eased);
			}

			if (t >= 1f)
			{
				isTransitioning = false;
				if (transitioningToHome)
				{
					ScreenManager.LockScreen.SetActive(false);
					ScreenManager.HomeScreen.SetActive(true);
					homeScreenCanvasGroup.alpha = 1f;
				}
				else
				{
					ScreenManager.LockScreen.SetActive(true);
					ScreenManager.HomeScreen.SetActive(false);
					lockScreenCanvasGroup.alpha = 1f;
				}
				lockScreenCanvasGroup.interactable = true;
				lockScreenCanvasGroup.blocksRaycasts = true;
				homeScreenCanvasGroup.interactable = true;
				homeScreenCanvasGroup.blocksRaycasts = true;
			}
		}
    }
}
