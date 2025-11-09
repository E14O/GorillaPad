using BepInEx;
using GorillaPad.Functions;
using GorillaPad.Functions.Managers;
using UnityEngine;

namespace GorillaPad
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private void Start()
        {
            new GameObject(Constants.Name, typeof(Main), typeof(PadNetworking), typeof(AnimationManager));
            ConfigManager.Init(Config);

        }
    }
}