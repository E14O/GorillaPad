using GorillaPad.Functions.Managers;
using GorillaPad.Interfaces;
using GorillaPad.Tools;
using Oculus.Platform.Models;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace GorillaPad.Functions
{
    internal class Main : MonoBehaviour
    {
        public static Main instance;
        public GameObject AppInterfaces;
        private bool LastState = false;

        void Start()
        {
            instance = this;
            GorillaTagger.OnPlayerSpawned(Initialization);
        }

        void Initialization()
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { Constants.CustomProp, Constants.Version } });
            ContentLoader.InitialiseContent();

            ContentLoader.Bundle.AddComponent<ScreenManager>();
            ContentLoader.Bundle.AddComponent<GorillaPadInterface>();
            ContentLoader.BundleParent.AddComponent<HoldableEngine>();

            AppInterfaces = ContentLoader.Bundle.transform.Find("Pad/Canvas/AppInterfaces").gameObject;
            AppInterfaces.SetActive(true);
        }


        void Update()
        {
            if (LastState && !AppSystem._AppOpen)
            {
                AppSystem.OnAppClose();
            }
            LastState = AppSystem._AppOpen;
        }
    }
}
