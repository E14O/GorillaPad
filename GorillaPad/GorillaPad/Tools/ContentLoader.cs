using System;
using System.Reflection;
using UnityEngine;

namespace GorillaPad.Tools
{
    internal class ContentLoader : MonoBehaviour
    {
        public static ContentLoader Instance { get; private set; }
        public static GameObject Bundle;
        public static GameObject BundleParent;

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
            BundleParent = Bundle.transform.Find("Pad").gameObject;
            BundleParent.transform.SetPositionAndRotation(Constants.AssetPosition.Pos, Constants.AssetPosition.Rot);
            BundleParent.transform.localScale = Constants.AssetPosition.Scale;

            GameObject SignParent = Bundle.transform.Find("Sign").gameObject;
            SignParent.transform.SetPositionAndRotation(Constants.SignPosition.Pos, Constants.SignPosition.Rot);
            SignParent.transform.localScale = Constants.SignPosition.Scale;
        }

        public static void GetSounds(ref AudioSource PowerAudio, ref AudioSource ButtonAudio)
        {
            try
            {
                PowerAudio = Bundle.transform.GetChild(0).GetChild(2).transform.Find("SecondaryButton").gameObject.GetComponent<AudioSource>();
                ButtonAudio = Bundle.transform.GetChild(0).GetChild(2).transform.Find("PrimaryButton").gameObject.GetComponent<AudioSource>();

                if (!SoundsLoaded)
                {
                    PadLogging.LogMessage("Successfully Loaded AudioSources!");
                    SoundsLoaded = true;
                }
            }
            catch (Exception e)
            {
                PadLogging.LogError($"Initialization With AudioSources Was Interrupted {e}");
                SoundsLoaded = false;
            }
        }


        private static AssetBundle InitialiseBundle(string path)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            return AssetBundle.LoadFromStream(stream);
        }
    }
}
