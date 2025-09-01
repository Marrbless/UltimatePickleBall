using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using PickleP2P.Core.Utils;
using PickleP2P.Core.Config;

namespace PickleP2P.Core.Bootstrap
{
    /// <summary>
    /// Initializes Steam (if available) and bootstraps the initial scene.
    /// </summary>
    public static class SteamBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            // Apply default configs early
            try
            {
                // Load configs via ModManager once it starts; apply defaults here
                Time.fixedDeltaTime = PhysicsConfig.Default().fixedTimestep;
                Physics.gravity = new Vector3(0f, PhysicsConfig.Default().gravity, 0f);
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.RULES, $"Failed to apply default engine settings: {ex.Message}");
            }

#if STEAMWORKS
            try
            {
                if (!SteamIsInitialized())
                {
                    Log.Warn(LogCategory.NET, "Steam not initialized. Running without Steam.");
                }
                else
                {
                    SetRichPresence("In Menu");
                }
            }
            catch (Exception ex)
            {
                Log.Warn(LogCategory.NET, $"Steam bootstrap exception: {ex.Message}");
            }
#endif

            // Load MainMenu if not already in a scene
            if (SceneManager.sceneCount == 0 || SceneManager.GetActiveScene().name != "MainMenu")
            {
                try
                {
                    SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                }
                catch
                {
                    // In case scene isn't added to build settings yet, create menu programmatically
                    var go = new GameObject("MainMenuUI");
                    go.AddComponent<PickleP2P.UI.Menus.MainMenuUI>();
                }
            }
        }

#if STEAMWORKS
        private static bool SteamIsInitialized()
        {
            try
            {
                if (!Steamworks.SteamAPI.Init())
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void SetRichPresence(string state)
        {
            try
            {
                Steamworks.SteamFriends.SetRichPresence("status", state);
            }
            catch { }
        }
#endif
    }
}

