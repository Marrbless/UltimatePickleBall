using UnityEngine;
using Mirror;

namespace PickleP2P.Core.Net
{
    /// <summary>
    /// Server authoritative ball state sync.
    /// </summary>
    public class BallNetwork : NetworkBehaviour
    {
        public Rigidbody rb;

        private void Awake()
        {
            if (rb == null)
            {
                rb = gameObject.GetComponent<Rigidbody>();
                if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
            }
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }
}

