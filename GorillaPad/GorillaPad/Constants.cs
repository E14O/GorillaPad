using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad
{
    internal class Constants
    {
        public const string GUID = "e14o.h4rns.gorillapad";
        public const string Name = "GorillaPad";
        public const string Version = "1.0.2";
        public const string CustomProp = "GP";

        public const string BundleName = "GPBundle";
        public const string NetworkName = "NetPad";
        public const string BundlePath = "GorillaPad.Content.gp";

        // Static Transforms 
        public static readonly TransformState Asset = new(new Vector3(-68.9724f, 11.6753f, -82.7419f), Quaternion.Euler(0f, 190.2f, 0f), new Vector3(.08f, .08f, .08f));
        public static readonly TransformState Chest = new(new Vector3(0.0121f, 0.1969f, 0.1303f), Quaternion.Euler(0f, 0f, 270f), new Vector3(.09f, .09f, .09f));

        // Hand Held Transforms
        public static readonly TransformState LeftHand = new(new Vector3(-0.149f, 0.0532f, 0.0552f), Quaternion.Euler(90f, 90f, 0f), new Vector3(.098f, .098f, .098f));
        public static readonly TransformState RightHand = new(new Vector3(0.1104f, 0.1137f, 0.0552f), Quaternion.Euler(43.8616f, 60.3276f, 329.0535f), new Vector3(.098f, .098f, .098f));
    }
}