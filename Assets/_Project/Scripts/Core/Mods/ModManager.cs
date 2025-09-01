using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using PickleP2P.Core.Utils;
using PickleP2P.Core.Config;

namespace PickleP2P.Core.Mods
{
    /// <summary>
    /// Loads mod manifest and content from search paths. Supports F5 hot-reload.
    /// </summary>
    public class ModManager : MonoBehaviour
    {
        public static ModManager Instance;

        public Manifest ActiveManifest { get; private set; }
        public string[] SearchRoots;
        public event System.Action ModsReloaded;

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
            SearchRoots = new []
            {
                GetUserModPath(),
                Path.Combine(Application.streamingAssetsPath, "Mods")
            };
            LoadAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Log.Info(LogCategory.MODS, "Hot-reloading mods...");
                LoadAll();
            }
        }

        public void LoadAll()
        {
            var manifest = LoadManifest();
            ActiveManifest = manifest;
            LoadConfigs(manifest);
            ApplyEngineSettings();
            ModsReloaded?.Invoke();
        }

        private void ApplyEngineSettings()
        {
            Time.fixedDeltaTime = GameConfig.Physics.fixedTimestep;
            Physics.gravity = new Vector3(0f, GameConfig.Physics.gravity, 0f);
        }

        private Manifest LoadManifest()
        {
            string manifestPath = FindFirstExisting("pack.json");
            if (string.IsNullOrEmpty(manifestPath))
            {
                Log.Warn(LogCategory.MODS, "No pack.json found. Using defaults.");
                return new Manifest
                {
                    assets = new Manifest.AssetMap
                    {
                        paddle = new Manifest.AssetEntry { model = "models/paddle.obj", mtl = "models/paddle.mtl" },
                        ball = new Manifest.AssetEntry { model = "models/ball.obj" },
                        net = new Manifest.AssetEntry { model = "models/net.obj" }
                    },
                    textures = new Dictionary<string, string>
                    {
                        {"court_diffuse", "textures/court_diffuse.png"},
                        {"paddle_albedo", "textures/paddle_albedo.png"}
                    },
                    audio = new Dictionary<string, string>
                    {
                        {"hit", "audio/hit.wav"},
                        {"bounce", "audio/bounce.wav"},
                        {"serve", "audio/serve.wav"}
                    },
                    rules = "config/rules.json",
                    physics = "config/physics.json",
                    camera = "config/camera.json"
                };
            }

            try
            {
                string json = File.ReadAllText(manifestPath);
                var mf = JsonUtility.FromJson<Manifest>(json);
                return mf;
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.MODS, $"Failed to parse pack.json: {ex.Message}");
                return new Manifest();
            }
        }

        private void LoadConfigs(Manifest manifest)
        {
            // Rules
            string rulesPath = ResolvePath(manifest.rules);
            if (!string.IsNullOrEmpty(rulesPath))
            {
                try { GameConfig.Rules = JsonUtility.FromJson<RulesConfig>(File.ReadAllText(rulesPath)); }
                catch (Exception ex) { Log.Warn(LogCategory.MODS, $"Rules parse failed: {ex.Message}"); }
            }
            // Physics
            string physicsPath = ResolvePath(manifest.physics);
            if (!string.IsNullOrEmpty(physicsPath))
            {
                try { GameConfig.Physics = JsonUtility.FromJson<PhysicsConfig>(File.ReadAllText(physicsPath)); }
                catch (Exception ex) { Log.Warn(LogCategory.MODS, $"Physics parse failed: {ex.Message}"); }
            }
            // Camera
            string cameraPath = ResolvePath(manifest.camera);
            if (!string.IsNullOrEmpty(cameraPath))
            {
                try { GameConfig.Camera = JsonUtility.FromJson<CameraConfig>(File.ReadAllText(cameraPath)); }
                catch (Exception ex) { Log.Warn(LogCategory.MODS, $"Camera parse failed: {ex.Message}"); }
            }
        }

        public string ResolvePath(string relative)
        {
            if (string.IsNullOrEmpty(relative)) return null;
            foreach (var root in SearchRoots)
            {
                string p = Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(p)) return p;
            }
            return null;
        }

        public string FindFirstExisting(string relative)
        {
            foreach (var root in SearchRoots)
            {
                string p = Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(p)) return p;
            }
            return null;
        }

        public Manifest.AssetEntry GetAssetEntry(string id)
        {
            if (ActiveManifest == null || ActiveManifest.assets == null) return null;
            switch (id)
            {
                case "paddle": return ActiveManifest.assets.paddle;
                case "ball": return ActiveManifest.assets.ball;
                case "net": return ActiveManifest.assets.net;
                default: return null;
            }
        }

        public GameObject BuildAssetGO(string id)
        {
            var entry = GetAssetEntry(id);
            if (entry == null || string.IsNullOrEmpty(entry.model)) return null;
            string obj = ResolvePath(entry.model);
            if (string.IsNullOrEmpty(obj)) return null;
            string mtl = ResolvePath(entry.mtl);
            Vector3? scale = null, euler = null, offset = null;
            if (entry.scale != null && entry.scale.Length == 3) scale = new Vector3(entry.scale[0], entry.scale[1], entry.scale[2]);
            if (entry.rotateEuler != null && entry.rotateEuler.Length == 3) euler = new Vector3(entry.rotateEuler[0], entry.rotateEuler[1], entry.rotateEuler[2]);
            if (entry.offset != null && entry.offset.Length == 3) offset = new Vector3(entry.offset[0], entry.offset[1], entry.offset[2]);
            return ObjLoader.ObjRuntimeBuilder.Build(obj, mtl, scale, euler, offset);
        }

        private string GetUserModPath()
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(home, "AppData", "LocalLow", GameConfig.CompanyName, GameConfig.ProductName, "Mods");
        }
    }
}

