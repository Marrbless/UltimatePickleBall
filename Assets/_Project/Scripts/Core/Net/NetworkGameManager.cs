using UnityEngine;
using Mirror;
using PickleP2P.Core.Utils;
using PickleP2P.Core.Config;

namespace PickleP2P.Core.Net
{
    /// <summary>
    /// Network manager: spawns players, score manager, and ball. Transport selection handled in scene.
    /// </summary>
    public class NetworkGameManager : NetworkManager
    {
        [Header("Prefabs")]
        public GameObject playerPrefabRef;
        public GameObject ballPrefabRef;
        public GameObject scoreManagerPrefabRef;

        private GameObject spawnedBall;
        private GameObject spawnedScoreManager;

        private void Awake()
        {
            // Ensure prefabs exist
            if (playerPrefabRef == null)
            {
                playerPrefabRef = CreateDefaultPlayerPrefab();
            }
            if (ballPrefabRef == null)
            {
                ballPrefabRef = CreateDefaultBallPrefab();
            }
            if (scoreManagerPrefabRef == null)
            {
                var go = new GameObject("ScoreManager");
                scoreManagerPrefabRef = go;
                go.AddComponent<ScoreManager>();
            }

            TryConfigureTransport();
        }

        public override void StartHost()
        {
            base.StartHost();
            Log.Info(LogCategory.NET, "StartHost called");
        }

        public override void StartClient()
        {
            base.StartClient();
            Log.Info(LogCategory.NET, "StartClient called");
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            var player = Instantiate(playerPrefabRef);
#if MIRROR
            // In real Mirror, attach player to connection for authority.
            Mirror.NetworkServer.AddPlayerForConnection(conn, player);
#else
            NetworkServer.Spawn(player);
#endif
            Log.Info(LogCategory.NET, "Player added to server");

            if (spawnedScoreManager == null && scoreManagerPrefabRef != null)
            {
                spawnedScoreManager = Instantiate(scoreManagerPrefabRef);
                NetworkServer.Spawn(spawnedScoreManager);
            }
            if (spawnedBall == null && ballPrefabRef != null)
            {
                spawnedBall = Instantiate(ballPrefabRef);
                NetworkServer.Spawn(spawnedBall);
                // Build court if not present
                var courtGo = new GameObject("Court");
                courtGo.AddComponent<PickleP2P.Gameplay.Court.CourtBuilder>();
            }
        }

        private GameObject CreateDefaultPlayerPrefab()
        {
            var go = new GameObject("Player");
            go.AddComponent<NetworkPlayer>();
            go.AddComponent<PickleP2P.Gameplay.Player.PlayerController>();
            go.AddComponent<PickleP2P.Gameplay.Player.FirstPersonCamera>();
            go.AddComponent<PickleP2P.Gameplay.Paddle.PaddleController>();
            go.AddComponent<PickleP2P.Core.Voice.VoiceChatManager>();
            var cc = go.AddComponent<CharacterController>();
            cc.height = 1.8f; cc.center = new Vector3(0, 0.9f, 0); cc.radius = 0.3f;
            return go;
        }

        private GameObject CreateDefaultBallPrefab()
        {
            var go = new GameObject("Ball");
            go.AddComponent<NetworkIdentity>();
            go.AddComponent<PickleP2P.Gameplay.Ball.BallController>();
            go.AddComponent<BallNetwork>();
            var sr = go.AddComponent<SphereCollider>();
            sr.material = new PhysicMaterial("BallPhys");
            var rb = go.AddComponent<Rigidbody>();
            rb.useGravity = true;
            return go;
        }

        private void TryConfigureTransport()
        {
            // If a transport already exists, do nothing.
            var existingTransport = GetComponent("Transport");
            if (existingTransport != null) return;

            // Try STEAM transport first if STEAMWORKS
#if STEAMWORKS
            var fizzyType = System.Type.GetType("FizzySteamworks.FizzySteamworks, FizzySteamworks");
            if (fizzyType != null)
            {
                gameObject.AddComponent(fizzyType);
                Log.Info(LogCategory.NET, "Using FizzySteamworks transport.");
                return;
            }
#endif
            // Try KCP
            var kcpType = System.Type.GetType("kcp2k.KcpTransport, kcp2k");
            if (kcpType != null)
            {
                gameObject.AddComponent(kcpType);
                Log.Info(LogCategory.NET, "Using KcpTransport.");
                return;
            }
            // Try Telepathy
            var telepathyType = System.Type.GetType("TelepathyTransport, Mirror");
            if (telepathyType != null)
            {
                gameObject.AddComponent(telepathyType);
                Log.Info(LogCategory.NET, "Using TelepathyTransport.");
                return;
            }

            Log.Warn(LogCategory.NET, "No transport found. Mirror package likely missing.");
        }
    }
}

