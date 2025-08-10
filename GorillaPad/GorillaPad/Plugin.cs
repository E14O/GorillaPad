using BepInEx;
using BepInEx.Logging;
using GorillaPad.Functions;
using GorillaPad.Interfaces;
using UnityEngine;

namespace GorillaPad
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private void Start() => new GameObject(Constants.Name, typeof(Main), typeof(AppCreation));
    }
}
