using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad
{
    internal class Constants
    {
        public const string GUID = "e14o.h4rns.gorillapad";

        public const string Name = "GorillaPad";

        public const string Version = "1.0.0";

        public const string CustomProp = "GP";

        public const string BundleName = "GPBundle";

        public const string BundlePath = "GorillaPad.Content.gp";

        // Static Transforms 

        public static readonly TransformState AssetPosition = new(new Vector3(-68.4512f, 11.41f, -81.4067f), Quaternion.Euler(0, 10, 270), new Vector3(.1f, .1f, .1f));

        public static readonly TransformState SignPosition = new(new Vector3(-68.453f, 11.2156f, -81.4102f), Quaternion.Euler(0, 190, 90), new Vector3(.1f, .1f, .1f));

        public static readonly TransformState ChestPosition = new(new Vector3(-0.001f, 0.16f, 0.15f), Quaternion.Euler(89.6948f, 97.7934f, 98.5541f), new Vector3(0.15f, 0.15f, 0.15f));

        // Hand Held Transforms

        public static readonly TransformState LeftHand = new(new Vector3(-0.14f, 0.06f, 0.05f), Quaternion.Euler(9.5455f, 180.8367f, 175.2001f), new Vector3(0.15f, 0.15f, 0.15f));

        public static readonly TransformState RightHand = new(new Vector3(0.14f, 0.04f, 0.05f), Quaternion.Euler(4.9056f, 176.9048f, 193.4551f), new Vector3(0.15f, 0.15f, 0.15f));
    }
}