using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace PickleP2P.Core.Mods.ObjLoader
{
    /// <summary>
    /// Minimal MTL parser supporting Ka, Kd, map_Kd.
    /// </summary>
    public static class MtlParser
    {
        public class MtlMaterial
        {
            public string name;
            public Color kd = Color.white;
            public string mapKd;
        }

        public static Dictionary<string, MtlMaterial> Parse(string path)
        {
            var dict = new Dictionary<string, MtlMaterial>();
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return dict;
            using (var sr = new StreamReader(path))
            {
                string line;
                var inv = CultureInfo.InvariantCulture;
                MtlMaterial current = null;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                    var parts = line.Trim().Split(' ');
                    if (parts[0] == "newmtl")
                    {
                        current = new MtlMaterial { name = parts[1] };
                        dict[current.name] = current;
                    }
                    else if (parts[0] == "Kd" && current != null)
                    {
                        current.kd = new Color(
                            float.Parse(parts[1], inv),
                            float.Parse(parts[2], inv),
                            float.Parse(parts[3], inv));
                    }
                    else if (parts[0] == "map_Kd" && current != null)
                    {
                        current.mapKd = parts[1];
                    }
                }
            }
            return dict;
        }
    }
}

