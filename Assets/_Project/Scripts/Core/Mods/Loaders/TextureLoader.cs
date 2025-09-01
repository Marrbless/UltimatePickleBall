using System.Collections;
using System.IO;
using UnityEngine;

namespace PickleP2P.Core.Mods.Loaders
{
    public static class TextureLoader
    {
        public static Texture2D LoadTexture(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;
            byte[] data = File.ReadAllBytes(path);
            var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(data, false);
            tex.name = Path.GetFileName(path);
            return tex;
        }
    }
}

