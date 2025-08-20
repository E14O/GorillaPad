using BepInEx;
using BepInEx.Logging;
using GorillaPad.Functions;
using GorillaPad.Functions.Managers;
using GorillaPad.Interfaces;
using GorillaPad.Tools;
using UnityEngine;

namespace GorillaPad
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        void Start()
        {
            new GameObject(Constants.Name, typeof(Main), typeof(PadNetworking), typeof(AnimationManager));
            ConfigManager.Init(Config);
        }
    }
}
