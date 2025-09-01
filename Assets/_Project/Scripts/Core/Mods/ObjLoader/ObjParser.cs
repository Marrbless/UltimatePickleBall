using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace PickleP2P.Core.Mods.ObjLoader
{
    /// <summary>
    /// Minimal OBJ parser supporting v/vt/vn and usemtl.
    /// </summary>
    public class ObjParser
    {
        public struct ObjMesh
        {
            public List<Vector3> positions;
            public List<Vector2> uvs;
            public List<Vector3> normals;
            public List<int> triangles;
        }

        public static ObjMesh Parse(string path)
        {
            var mesh = new ObjMesh
            {
                positions = new List<Vector3>(),
                uvs = new List<Vector2>(),
                normals = new List<Vector3>(),
                triangles = new List<int>()
            };

            var tempV = new List<Vector3>();
            var tempVT = new List<Vector2>();
            var tempVN = new List<Vector3>();
            var dict = new Dictionary<string,int>();

            var verts = new List<Vector3>();
            var uvs = new List<Vector2>();
            var norms = new List<Vector3>();
            var tris = new List<int>();

            using (var sr = new StreamReader(path))
            {
                string line;
                var inv = CultureInfo.InvariantCulture;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                    var parts = line.Trim().Split(' ');
                    if (parts[0] == "v")
                    {
                        tempV.Add(new Vector3(
                            float.Parse(parts[1], inv),
                            float.Parse(parts[2], inv),
                            float.Parse(parts[3], inv)));
                    }
                    else if (parts[0] == "vt")
                    {
                        tempVT.Add(new Vector2(
                            float.Parse(parts[1], inv),
                            float.Parse(parts[2], inv)));
                    }
                    else if (parts[0] == "vn")
                    {
                        tempVN.Add(new Vector3(
                            float.Parse(parts[1], inv),
                            float.Parse(parts[2], inv),
                            float.Parse(parts[3], inv)));
                    }
                    else if (parts[0] == "f")
                    {
                        // triangulate assuming convex polygons
                        var face = new List<int>();
                        for (int i = 1; i < parts.Length; i++)
                        {
                            string key = parts[i];
                            if (!dict.TryGetValue(key, out int idx))
                            {
                                var comps = key.Split('/');
                                int vi = int.Parse(comps[0]);
                                int ti = comps.Length > 1 && comps[1] != string.Empty ? int.Parse(comps[1]) : 0;
                                int ni = comps.Length > 2 ? int.Parse(comps[2]) : 0;
                                var v = tempV[ToIndex(vi, tempV.Count)];
                                Vector2 uv = ti != 0 ? tempVT[ToIndex(ti, tempVT.Count)] : Vector2.zero;
                                Vector3 n = ni != 0 ? tempVN[ToIndex(ni, tempVN.Count)] : Vector3.up;
                                verts.Add(v);
                                uvs.Add(uv);
                                norms.Add(n);
                                idx = verts.Count - 1;
                                dict[key] = idx;
                            }
                            face.Add(idx);
                        }
                        for (int i = 1; i + 1 < face.Count; i++)
                        {
                            tris.Add(face[0]);
                            tris.Add(face[i]);
                            tris.Add(face[i + 1]);
                        }
                    }
                }
            }

            mesh.positions = verts;
            mesh.uvs = uvs;
            mesh.normals = norms;
            mesh.triangles = tris;
            return mesh;
        }

        private static int ToIndex(int index, int count)
        {
            return index < 0 ? count + index : index - 1;
        }
    }
}

