﻿using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad
{
    internal class Constants
    {
        public const string GUID = "e14o.h4rns.spankypluh.cody.gorillapad";
        public const string Name = "GorillaPad";
        public const string Version = "1.0.0";
        public const string CustomProp = "GP";

        public const string BundleName = "GPBundle";
        public const string NetworkName = "NetPad";
        public const string BundlePath = "GorillaPad.Content.gp";

        // Static Transforms 
        public static readonly TransformState Asset = new(new Vector3(-68.4413f, 11.3857f, -81.3887f), Quaternion.Euler(0f, 100f, 1.0907f), new Vector3(.06f, .06f, .06f));
        public static readonly TransformState Chest = new(new Vector3(0.0121f, 0.1969f, 0.1303f), Quaternion.Euler(0f, 0f, 270f), new Vector3(.09f, .09f, .09f));

        // Hand Held Transforms
        public static readonly TransformState LeftHand = new(new Vector3(-0.0963f, 0.087f, 0.0238f), Quaternion.Euler(41.609f, 75.893f, 71.86f), new Vector3(.098f, .098f, .098f));
        public static readonly TransformState RightHand = new(new Vector3(0.1104f, 0.1137f, 0.0552f), Quaternion.Euler(43.8616f, 60.3276f, 329.0535f), new Vector3(.098f, .098f, .098f));

        // TO FIX:
        /* when moving, and grabbed, POS gets messed up
         * fix right hand POS
         * make our own holdable engine
         * fix netowking errors
        */
    }
}