using UnityEngine;
using Mirror;

namespace PickleP2P.Core.Net
{
    /// <summary>
    /// Represents a connected player. Owns local components.
    /// </summary>
    public class NetworkPlayer : NetworkBehaviour
    {
        public Transform headTransform;
        public PickleP2P.Gameplay.Player.PlayerController playerController;
        public PickleP2P.Gameplay.Player.FirstPersonCamera fpCamera;
        public PickleP2P.Gameplay.Paddle.PaddleController paddleController;

        private void Awake()
        {
            if (headTransform == null)
            {
                var head = new GameObject("Head");
                head.transform.SetParent(transform, false);
                head.transform.localPosition = new Vector3(0, 1.7f, 0);
                headTransform = head.transform;
            }
        }

        private void Start()
        {
            if (isLocalPlayer)
            {
                if (fpCamera == null) fpCamera = gameObject.AddComponent<PickleP2P.Gameplay.Player.FirstPersonCamera>();
                if (playerController == null) playerController = gameObject.AddComponent<PickleP2P.Gameplay.Player.PlayerController>();
                if (paddleController == null) paddleController = gameObject.AddComponent<PickleP2P.Gameplay.Paddle.PaddleController>();
                fpCamera.target = headTransform;
            }
        }
    }
}

