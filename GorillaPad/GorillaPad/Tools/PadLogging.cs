using BepInEx.Logging;

namespace GorillaPad.Tools
{
    internal class PadLogging
    {
        private static readonly ManualLogSource logger;

        public static void LogInfo(string message) => logger.LogInfo(message);
        public static void LogMessage(string message) => logger.LogMessage(message);
        public static void LogWarning(string message) => logger.LogWarning(message);
        public static void LogError(string message) => logger.LogError(message);
    }
}



