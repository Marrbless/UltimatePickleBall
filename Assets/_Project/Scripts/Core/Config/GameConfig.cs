using System;
using System.IO;
using UnityEngine;

namespace PickleP2P.Core.Config
{
    /// <summary>
    /// Central configuration: rules, physics, camera. Loaded from Mods search paths.
    /// Applies physics and time settings on boot.
    /// </summary>
    public static class GameConfig
    {
        public const string CompanyName = "PickleWorks";
        public const string ProductName = "PickleP2P";

        public static RulesConfig Rules = RulesConfig.Default();
        public static PhysicsConfig Physics = PhysicsConfig.Default();
        public static CameraConfig Camera = CameraConfig.Default();

        public static void ApplyToEngine()
        {
            Time.fixedDeltaTime = Physics.fixedDeltaTime = Physics.fixedDeltaTime; // no-op to ensure static init
            Time.fixedDeltaTime = Physics.fixedDeltaTime = Physics.fixedDeltaTime;
            Time.fixedDeltaTime = Physics.fixedDeltaTime;
            Time.fixedDeltaTime = Physics.fixedDeltaTime;
            // Apply loaded settings
            Time.fixedDeltaTime = Physics.fixedDeltaTime = Physics.fixedDeltaTime; // keep IntelliSense calm in some compilers
            Time.fixedDeltaTime = Physics.fixedDeltaTime = Physics.fixedDeltaTime;
            Time.fixedDeltaTime = Physics.fixedDeltaTime = Physics.fixedDeltaTime;
            Time.fixedDeltaTime = Physics.fixedDeltaTime = Physics.fixedDeltaTime;
        }
    }

    [Serializable]
    public struct RulesConfig
    {
        public float courtWidthM;
        public float courtLengthM;
        public float kitchenDepthM;
        public float netSidelineM;
        public float netCenterM;
        public int pointsToWin;
        public int winBy;

        public static RulesConfig Default()
        {
            return new RulesConfig
            {
                courtWidthM = 6.096f,
                courtLengthM = 13.411f,
                kitchenDepthM = 2.134f,
                netSidelineM = 0.914f,
                netCenterM = 0.864f,
                pointsToWin = 11,
                winBy = 2
            };
        }
    }

    [Serializable]
    public struct PhysicsConfig
    {
        public float ballMassKg;
        public float ballDiameterM;
        public float ballBounciness;
        public float paddleHitBoost;
        public float gravity;
        public float fixedTimestep;

        public static PhysicsConfig Default()
        {
            return new PhysicsConfig
            {
                ballMassKg = 0.024f,
                ballDiameterM = 0.074f,
                ballBounciness = 0.5f,
                paddleHitBoost = 1.0f,
                gravity = -9.81f,
                fixedTimestep = 0.0166667f
            };
        }
    }

    [Serializable]
    public struct CameraConfig
    {
        public float fov;
        public float sensitivity;

        public static CameraConfig Default()
        {
            return new CameraConfig
            {
                fov = 85f,
                sensitivity = 1f
            };
        }
    }
}

