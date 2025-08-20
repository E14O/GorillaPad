using BepInEx.Configuration;
using UnityEngine;

namespace GorillaPad.Functions.Managers
{
    public class ConfigManager : MonoBehaviour
    {
        public static ConfigFile file;
        private static ConfigEntry<ColourChoice> Colour;


        public static void Init(ConfigFile Config)
        { 
            file = Config;
            Colour = Config.Bind(
                "Apperance",
                "Colour",
                ColourChoice.PlayerColour
            );
        }

        public enum ColourChoice
        {
            PlayerColour,
            Red,
            Green,
            Blue,
            Orange
        }

        public static ColourChoice GetColourChoice()
        {
            return Colour.Value;
        }
    }
}