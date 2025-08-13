using GorillaPad.Functions.Screens;
using GorillaPad.Interfaces;
using GorillaPad.Tools;
using Photon.Pun;
using UnityEngine;

namespace GorillaPad.Functions
{
    internal class Main : MonoBehaviour
    {
        public static Main instance;

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
        }

        void Update()
        {
            if (AppSystem.LastAppState && !AppSystem._AppOpen)
            {
               AppSystem.OnAppClose();
            }
            AppSystem.LastAppState = AppSystem._AppOpen;
        }
    }
}
