using System.Collections.Generic;
using System.IO;
using UnityEngine;
using PickleP2P.Core.Mods.Loaders;

namespace PickleP2P.Core.Mods.ObjLoader
{
    /// <summary>
    /// Builds a Unity Mesh and Material from parsed OBJ/MTL.
    /// </summary>
    public static class ObjRuntimeBuilder
    {
        public static GameObject Build(string objPath, string mtlPath, Vector3? scale = null, Vector3? euler = null, Vector3? offset = null)
        {
            var parsed = ObjParser.Parse(objPath);
            var go = new GameObject(Path.GetFileNameWithoutExtension(objPath));
            var mf = go.AddComponent<MeshFilter>();
            var mr = go.AddComponent<MeshRenderer>();

            var mesh = new Mesh();
            mesh.SetVertices(parsed.positions);
            mesh.SetUVs(0, parsed.uvs);
            mesh.SetNormals(parsed.normals);
            mesh.SetTriangles(parsed.triangles, 0);
            mesh.RecalculateBounds();
            if (parsed.normals == null || parsed.normals.Count == 0)
            {
                mesh.RecalculateNormals();
            }
            mf.sharedMesh = mesh;

            // Material
            var mat = new Material(Shader.Find("Standard"));
            if (!string.IsNullOrEmpty(mtlPath))
            {
                var mats = MtlParser.Parse(mtlPath);
                foreach (var m in mats.Values)
                {
                    mat.color = m.kd;
                    if (!string.IsNullOrEmpty(m.mapKd))
                    {
                        string texPath = Path.Combine(Path.GetDirectoryName(mtlPath), m.mapKd);
                        var tex = TextureLoader.LoadTexture(texPath);
                        if (tex != null) mat.mainTexture = tex;
                    }
                    break;
                }
            }
            mr.sharedMaterial = mat;

            // Add collider from bounds for convenience
            go.AddComponent<PickleP2P.Core.Utils.BoundsAutoCollider>();

            if (scale.HasValue) go.transform.localScale = scale.Value;
            if (euler.HasValue) go.transform.localRotation = Quaternion.Euler(euler.Value);
            if (offset.HasValue) go.transform.localPosition = offset.Value;

            return go;
        }
    }
}

