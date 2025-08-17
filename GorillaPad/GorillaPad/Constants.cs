using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad
{
    internal class Constants
    {
        public const string GUID = "e14o.h4rns.spankypluh.gorillapad";

        public const string Name = "GorillaPad";

        public const string Version = "1.0.0";

        public const string CustomProp = "GP";

        public const string BundleName = "GPBundle";

        public const string NetworkName = "NetPad";

        public const string BundlePath = "GorillaPad.Content.gp";

        // Static Transforms 
        
        public static readonly TransformState Asset = new(new Vector3(-68.4413f, 11.3857f, -81.3887f), Quaternion.Euler(0f, 100f, 1.0907f), new Vector3(.06f, .06f, .06f));

        public static readonly TransformState Chest = new(new Vector3(-0.001f, 0.16f, 0.15f), Quaternion.Euler(0f, 0f, 270f), new Vector3(.09f, .09f, .09f));


        // Hand Held Transforms


        public static readonly TransformState LeftHand = new(new Vector3(-0.0963f, 0.087f, 0.0238f), Quaternion.Euler(41.609f, 75.893f, 71.86f), new Vector3(.098f, .098f, .098f));

        public static readonly TransformState RightHand = new(new Vector3(0.0994f, 0.0902f, 0.0402f), Quaternion.Euler(318.182f, 90.532f, 85.883f), new Vector3(.098f, .098f, .098f));

    }
}