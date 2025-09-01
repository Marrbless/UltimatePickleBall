using UnityEngine;
using PickleP2P.Core.Config;

namespace PickleP2P.Gameplay.Court
{
    /// <summary>
    /// Procedurally builds a regulation pickleball court.
    /// </summary>
    public class CourtBuilder : MonoBehaviour
    {
        public Material courtMaterial;
        public Transform root;

        private void Awake()
        {
            if (root == null)
            {
                root = new GameObject("CourtRoot").transform;
                root.SetParent(transform, false);
            }
            Build();
        }

        public void Build()
        {
            float width = GameConfig.Rules.courtWidthM;
            float length = GameConfig.Rules.courtLengthM;
            float netCenterH = GameConfig.Rules.netCenterM;
            float netSideH = GameConfig.Rules.netSidelineM;
            float kitchen = GameConfig.Rules.kitchenDepthM;

            // Court floor
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "CourtFloor";
            floor.transform.SetParent(root, false);
            floor.transform.localScale = new Vector3(width, 0.02f, length);
            floor.transform.localPosition = new Vector3(0, -0.01f, 0);
            if (courtMaterial == null)
            {
                courtMaterial = new Material(Shader.Find("Standard"));
                courtMaterial.color = new Color(0.2f, 0.5f, 0.3f);
                // Apply mod texture if present
                var mod = PickleP2P.Core.Mods.ModManager.Instance;
                if (mod != null && mod.ActiveManifest != null && mod.ActiveManifest.textures != null && mod.ActiveManifest.textures.TryGetValue("court_diffuse", out var rel))
                {
                    string path = mod.ResolvePath(rel);
                    var tex = PickleP2P.Core.Mods.Loaders.TextureLoader.LoadTexture(path);
                    if (tex != null) courtMaterial.mainTexture = tex;
                }
            }
            floor.GetComponent<MeshRenderer>().material = courtMaterial;

            // Net (flat box at center)
            var net = GameObject.CreatePrimitive(PrimitiveType.Cube);
            net.name = "Net";
            net.transform.SetParent(root, false);
            net.transform.localScale = new Vector3(width, (netSideH + netCenterH) * 0.5f, 0.05f);
            net.transform.localPosition = new Vector3(0, net.transform.localScale.y / 2f, 0);
            var netCol = net.GetComponent<BoxCollider>();
            netCol.isTrigger = false;

            // Kitchen lines (for visuals only in MVP)
            CreateLine(new Vector3(0, 0.001f, length * 0.5f - kitchen), width);
            CreateLine(new Vector3(0, 0.001f, -length * 0.5f + kitchen), width);
        }

        private void CreateLine(Vector3 position, float width)
        {
            var line = GameObject.CreatePrimitive(PrimitiveType.Cube);
            line.name = "Line";
            line.transform.SetParent(root, false);
            line.transform.localScale = new Vector3(width, 0.002f, 0.02f);
            line.transform.localPosition = position;
            line.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
}

