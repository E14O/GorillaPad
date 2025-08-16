using System;
using System.Reflection;
using UnityEngine;

namespace GorillaPad.Tools
{
    internal class ContentLoader : MonoBehaviour
    {
        public static ContentLoader Instance { get; private set; }
        public static GameObject Bundle, BundleParent, NetworkedPad;

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
            BundleParent.transform.SetPositionAndRotation(Constants.Asset.Position, Constants.Asset.Rotation);
            BundleParent.transform.localScale = Constants.Asset.Scale;

            GameObject SignParent = Bundle.transform.Find("Sign").gameObject;
            SignParent.transform.SetPositionAndRotation(Constants.Asset.Position, Constants.Asset.Rotation);
            SignParent.transform.localScale = Constants.Asset.Scale;

            NetworkedPad = Instantiate(BundleParent);
            NetworkedPad.name = Constants.NetworkName;
            NetworkedPad.SetActive(false);
        }

        public static void GetSounds(ref AudioSource PowerAudio, ref AudioSource ButtonAudio)
        {
            try
            {
                PowerAudio = BundleParent.transform.GetChild(2).transform.Find("SecondaryButton").gameObject.GetComponent<AudioSource>();
                ButtonAudio = BundleParent.transform.GetChild(2).transform.Find("PrimaryButton").gameObject.GetComponent<AudioSource>();

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
            return AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(path));
        }
    }
}
