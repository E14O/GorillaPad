using System.Reflection;
using Photon.Pun;
using UnityEngine;
using GorillaPad.Logic.Screens;

namespace GorillaPad.Functions
{
    internal class ContentLoader : MonoBehaviour
    {
        public static bool _AssetLoaded = false;
        public static bool _SoundsLoaded = false;
        public static GameObject GorillaPad, GorillaPadMainParent, PowerSound, ButtonSound;

        public static void LoadAssetBundle()
        {
            var bundle = LoadAssetBundle(Constants.AssetBundlePath);
            GorillaPad = bundle.LoadAsset<GameObject>("GPBundle");
            GorillaPad = Instantiate(GorillaPad);
            GorillaPad.name = $"{PhotonNetwork.LocalPlayer.NickName}'s GorillaPad";
            SetPos();

            PowerSound = GorillaPad.transform.GetChild(0).GetChild(3).gameObject;
            ButtonSound = GorillaPad.transform.GetChild(0).GetChild(4).gameObject;


            Debug.Log($"[GorillaPad]- Asset Loaded: {GorillaPad.name}");
            _AssetLoaded = true;
        }

        public static void GetSounds()
        {
            Debug.Log("[GorillaPad]- Sounds Loaded");
            _SoundsLoaded = true;
        }

        public static void SetPos()
        {
            GorillaPadMainParent = GorillaPad.transform.Find("GPParent").gameObject;
            GorillaPadMainParent.transform.SetLocalPositionAndRotation(new Vector3(-68.4512f, 11.41f, -81.4067f), Quaternion.Euler(0, 10, 270));
            GorillaPadMainParent.transform.localScale = new Vector3(.1f, .1f, .1f);

            GameObject SignParent = GorillaPad.transform.Find("GPSignParent").gameObject;
            SignParent.transform.SetLocalPositionAndRotation(new Vector3(-68.453f, 11.2156f, -81.4102f), Quaternion.Euler(0, 190, 90));
            SignParent.transform.localScale = new Vector3(.1f, .1f, .1f);
        }

        public static AssetBundle LoadAssetBundle(string path)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            var assetBundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return assetBundle;
        }
    }
}
