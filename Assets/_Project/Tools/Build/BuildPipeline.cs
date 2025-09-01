using System.IO;
using UnityEditor;

namespace PickleP2P.Tools.Build
{
    public static class BuildPipelineMenu
    {
        [MenuItem("Tools/Build/Make Windows Build")]
        public static void MakeWindowsBuild()
        {
            string path = "Builds/Windows";
            Directory.CreateDirectory(path);
            string exePath = Path.Combine(path, "PickleP2P.exe");
            var scenes = new[]
            {
                "Assets/_Project/Scenes/Boot.unity"
            };
            BuildPipeline.BuildPlayer(scenes, exePath, BuildTarget.StandaloneWindows64, BuildOptions.None);
        }
    }
}

