using System;
using System.Reflection;
using UnityEngine;

namespace GorillaPad.Tools
{
    internal class ContentLoader : MonoBehaviour
    {
        public static ContentLoader Instance { get; private set; }
        public static GameObject Bundle { get; private set; }
        public static GameObject BundleParent { get; private set; }

        private static bool SoundsLoaded = false;

        public static void InitialiseContent()
        {
            try
            {
                if (Bundle != null) return;

                AssetBundle assetBundle = InitialiseBundle(Constants.BundlePath);
                Bundle = Instantiate(assetBundle.LoadAsset<GameObject>(Constants.BundleName));

                BundlePositions();

                PadLogging.LogMessage($"Initialization With {Constants.BundleName} Was Successful!");
            }
            catch (Exception e)
            {
                PadLogging.LogError($"Initialization With {Constants.BundleName} Was Interrupted {e}");
            }
        }

        public static void BundlePositions()
        {
            BundleParent = Bundle.transform.Find("GPParent").gameObject;
            BundleParent.transform.SetLocalPositionAndRotation(Constants.AssetPosition.Pos, Constants.AssetPosition.Rot);
            BundleParent.transform.localScale = Constants.AssetPosition.Scale;

            GameObject SignParent = Bundle.transform.Find("GPSignParent").gameObject;
            SignParent.transform.SetLocalPositionAndRotation(Constants.SignPosition.Pos, Constants.SignPosition.Rot);
            SignParent.transform.localScale = Constants.SignPosition.Scale;
        }

        public static void GetSounds(ref AudioSource PowerAudio, ref AudioSource ButtonAudio)
        {
            if (!SoundsLoaded)
            {
                PadLogging.LogMessage($"Successfully Loaded AudioSources!");
                SoundsLoaded = true;
            }

            PowerAudio = Bundle.transform.GetChild(0).GetChild(3).gameObject.GetComponent<AudioSource>();
            ButtonAudio = Bundle.transform.GetChild(0).GetChild(4).gameObject.GetComponent<AudioSource>();
        }

        private static AssetBundle InitialiseBundle(string path)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            return AssetBundle.LoadFromStream(stream);
        }
    }
}
