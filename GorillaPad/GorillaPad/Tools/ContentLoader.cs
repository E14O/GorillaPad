using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GorillaPad.Tools
{
    internal class ContentLoader : MonoBehaviour
    {
        public static ContentLoader Instance { get; private set; }
        public static GameObject Bundle, BundleParent, NetworkedPad, SignParent;

        private static bool SoundsLoaded = false;
        public static bool BundleLoaded = false;

        public static void InitialiseContent()
        {
            try
            {
                if (Bundle != null) return;

                Bundle = Instantiate(InitialiseBundle(Constants.BundlePath).LoadAsset<GameObject>(Constants.BundleName));

                BundlePositions();

                BundleLoaded = true;
                PadLogging.LogInfo($"Initialization With {Constants.BundleName} Was Successful!");
            }
            catch (Exception e)
            {
                PadLogging.LogError($"Initialization With {Constants.BundleName} Was Interrupted {e}");
            }
        }

        public static void BundlePositions()
        {
            // Stump:
            BundleParent = Bundle.transform.Find("Pad").gameObject;
            BundleParent.transform.SetPositionAndRotation(Constants.Asset.Position, Constants.Asset.Rotation);
            BundleParent.transform.localScale = Constants.Asset.Scale;

            SignParent = Bundle.transform.Find("Sign").gameObject;
            SignParent.transform.SetPositionAndRotation(Constants.Asset.Position, Constants.Asset.Rotation);
            SignParent.transform.localScale = Constants.Asset.Scale;

            // Networking:
            NetworkedPad = Instantiate(BundleParent);
            NetworkedPad.name = Constants.NetworkName;
            NetworkedPad.SetActive(false);
        }

        public static void GetSounds(ref AudioSource PowerAudio, ref AudioSource ButtonAudio, ref AudioSource BuzzAudio)
        {
            try
            {
                Transform SoundParent = BundleParent.transform.GetChild(2);
                PowerAudio = SoundParent.transform.Find("SecondaryButton").gameObject.GetComponent<AudioSource>();
                ButtonAudio = SoundParent.transform.Find("PrimaryButton").gameObject.GetComponent<AudioSource>();
                // BuzzAudio = SoundParent.transform.Find("NotificationAudio").gameObject.GetComponent<AudioSource>();
                BuzzAudio = null;

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