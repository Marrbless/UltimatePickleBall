using UnityEngine;

namespace PickleP2P.Core.Utils
{
    /// <summary>
    /// Adds a collider that fits the MeshRenderer bounds.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class BoundsAutoCollider : MonoBehaviour
    {
        public bool useBox = true;

        private void Awake()
        {
            var mr = GetComponent<MeshRenderer>();
            var b = mr.bounds;
            if (useBox)
            {
                var col = gameObject.AddComponent<BoxCollider>();
                col.center = transform.InverseTransformPoint(b.center);
                col.size = transform.InverseTransformVector(b.size);
            }
            else
            {
                var col = gameObject.AddComponent<SphereCollider>();
                col.center = transform.InverseTransformPoint(b.center);
                col.radius = Mathf.Max(b.extents.x, b.extents.y, b.extents.z);
            }
        }
    }
}

