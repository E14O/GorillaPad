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

        public const string BundlePath = "GorillaPad.Content.gp";

        // Static Transforms 
        
        public static readonly TransformState Asset = new(new Vector3(-68.4413f, 11.3857f, -81.3887f), Quaternion.Euler(0, 100, 0), new Vector3(.06f, .06f, .06f));

        public static readonly TransformState Chest = new(new Vector3(-0.001f, 0.16f, 0.15f), Quaternion.Euler(0f, 0f, 270f), new Vector3(.09f, .09f, .09f));

        // Hand Held Transforms

        public static readonly TransformState LeftHand = new(new Vector3(-0.14f, 0.06f, 0.05f), Quaternion.Euler(9.5455f, 180.8367f, 175.2001f), new Vector3(0.15f, 0.15f, 0.15f));

        public static readonly TransformState RightHand = new(new Vector3(0.14f, 0.04f, 0.05f), Quaternion.Euler(4.9056f, 176.9048f, 193.4551f), new Vector3(0.15f, 0.15f, 0.15f));
    }
}