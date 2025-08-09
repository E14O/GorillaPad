using BepInEx;
using GorillaPad.Logic;
using UnityEngine;

namespace GorillaPad
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        void Start() => new GameObject(Constants.Name, typeof(Main));
    }
}
