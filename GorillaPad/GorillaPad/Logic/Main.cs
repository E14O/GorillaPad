using GorillaPad.Interfaces;
using GorillaPad.Logic.Screens;
using GorillaPad.Tools;
using Photon.Pun;
using UnityEngine;

namespace GorillaPad.Logic
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
            ContentLoader.BundleParent.AddComponent<GorillaPadHoldableEngine>();
        }
    }
}
