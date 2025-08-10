using BepInEx;
using GorillaPad.Interfaces;
using GorillaPad.Logic;
using GorillaPad.Logic.Apps;
using UnityEngine;

namespace GorillaPad
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        void Start() => new GameObject(Constants.Name, typeof(Main), typeof(AppCreation));
    }
}
