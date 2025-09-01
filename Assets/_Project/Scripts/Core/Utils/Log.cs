using UnityEngine;

namespace PickleP2P.Core.Utils
{
    public enum LogCategory
    {
        NET,
        VOICE,
        MODS,
        RULES,
        GAMEPLAY,
        UI
    }

    public static class Log
    {
        public static bool EnableInfo = true;
        public static bool EnableWarnings = true;
        public static bool EnableErrors = true;

        public static void Info(LogCategory category, string message)
        {
            if (!EnableInfo) return;
            Debug.Log($"[{category}] {message}");
        }

        public static void Warn(LogCategory category, string message)
        {
            if (!EnableWarnings) return;
            Debug.LogWarning($"[{category}] {message}");
        }

        public static void Error(LogCategory category, string message)
        {
            if (!EnableErrors) return;
            Debug.LogError($"[{category}] {message}");
        }
    }
}

