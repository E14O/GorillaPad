using System;
using System.Reflection;
using Photon.Pun;
using UnityEngine;

namespace GorillaPad.Tools
{
    internal class ContentLoader : MonoBehaviour
    {
        public static ContentLoader Instance { get; private set; }

        public static GameObject Bundle, BundleParent;

        public static bool _SoundsLoaded = false;

        public static void InitialiseContent()
        {
            try
            {
                if (Bundle == null)
                {
                    Bundle = Instantiate(InitialiseAssetBundle(Constants.BundlePath).LoadAsset<GameObject>(Constants.BundleName));
                    Bundle.name = $"{PhotonNetwork.LocalPlayer.NickName}'s GorillaPad";
                    StartUpPosition();

                    Debug.Log($"[GorillaPad]- Asset Loaded: {Bundle.name}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Initialization With {Constants.Name} Was Interrupted {e}");
            }
        }

        public static void GetSounds(ref AudioSource PowerAudio, ref AudioSource ButtonAudio)
        {
            if (!_SoundsLoaded)
            {
                Debug.Log("[GorillaPad]- Sounds Loaded");
                _SoundsLoaded = true;
            }

            PowerAudio = Bundle.transform.GetChild(0).GetChild(3).gameObject.GetComponent<AudioSource>();
            ButtonAudio = Bundle.transform.GetChild(0).GetChild(4).gameObject.GetComponent<AudioSource>();
        }

        public static void StartUpPosition()
        {
            BundleParent = Bundle.transform.Find("GPParent").gameObject;
            BundleParent.transform.SetLocalPositionAndRotation(Constants.AssetPosition.Pos, Constants.AssetPosition.Rot);
            BundleParent.transform.localScale = Constants.AssetPosition.Scale;

            GameObject SignParent = Bundle.transform.Find("GPSignParent").gameObject;
            SignParent.transform.SetLocalPositionAndRotation(Constants.SignPosition.Pos, Constants.SignPosition.Rot);
            SignParent.transform.localScale = Constants.SignPosition.Scale;
        }

        public static AssetBundle InitialiseAssetBundle(string path)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            {
                return AssetBundle.LoadFromStream(stream);
            }
        }
    }
}
