using BepInEx.Logging;

namespace GorillaPad.Tools
{
    internal static class PadLogging
    {
        private static ManualLogSource logger;

        private static ManualLogSource LoggerInstance
        {
            get
            {
                logger ??= Logger.CreateLogSource(Constants.Name);
                return logger;
            }
        }

        public static void LogInfo(string message) => LoggerInstance.LogInfo(message);
        public static void LogMessage(string message) => LoggerInstance.LogMessage(message);
        public static void LogWarning(string message) => LoggerInstance.LogWarning(message);
        public static void LogError(string message) => LoggerInstance.LogError(message);
    }
}
