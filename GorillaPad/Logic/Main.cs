using GorillaPad.Functions;
using GorillaPad.Interfaces;
using GorillaPad.Logic.Screens;
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
            GorillaTagger.OnPlayerSpawned(GorillaPadMain);
        }

        void GorillaPadMain()
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { Constants.CustomProp, Constants.Version } });

            ContentLoader.LoadAssetBundle();
            ContentLoader.GorillaPad.AddComponent<ScreenManager>();
            ContentLoader.GorillaPad.AddComponent<GorillaPadInterface>();
            ContentLoader.GorillaPadMainParent.AddComponent<GorillaPadHoldableEngine>();
        }
    }
}
