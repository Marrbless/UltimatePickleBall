// Compile-safe minimal stubs for Mirror so the project builds before Mirror is imported.
// When Mirror is added via UPM, define MIRROR_INSTALLED in Project Settings > Player > Scripting Define Symbols
// or just rely on UNITY compiler finding the real Mirror namespace, which will override these stubs.

#if !MIRROR
using System;
using UnityEngine;

namespace Mirror
{
    public class SyncVarAttribute : Attribute { }
    public class ClientRpcAttribute : Attribute { public Channels channel; }
    public class TargetRpcAttribute : Attribute { public Channels channel; }
    public class CommandAttribute : Attribute { public Channels channel; }

    public class NetworkBehaviour : MonoBehaviour
    {
        public bool isServer => true;
        public bool isClient => true;
        public bool isLocalPlayer => true;
        public bool hasAuthority => true;
        public uint netId => 0;
        protected void CmdStub() { }
        protected void RpcStub() { }
    }

    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager singleton;
        public GameObject playerPrefab;
        public virtual void StartHost() { Debug.Log("[MirrorStub] StartHost()"); }
        public virtual void StartClient() { Debug.Log("[MirrorStub] StartClient()"); }
        public virtual void StartServer() { Debug.Log("[MirrorStub] StartServer()"); }
        public virtual void StopClient() { Debug.Log("[MirrorStub] StopClient()"); }
        public virtual void StopHost() { Debug.Log("[MirrorStub] StopHost()"); }
        public virtual void StopServer() { Debug.Log("[MirrorStub] StopServer()"); }
        public virtual void OnServerAddPlayer(NetworkConnectionToClient conn) { }
        public virtual void OnClientConnect() { }
        public virtual void OnClientDisconnect() { }
        public virtual void OnServerDisconnect(NetworkConnectionToClient conn) { }
    }

    public class NetworkConnectionToClient { }

    public class NetworkIdentity : MonoBehaviour { }

    public class NetworkClient
    {
        public static bool active { get; set; }
        public static double serverTickTime => Time.fixedDeltaTime;
    }

    public static class NetworkServer
    {
        public static bool active => true;
        public static void Spawn(GameObject obj) { }
        public static void Destroy(GameObject obj) { UnityEngine.Object.Destroy(obj); }
    }

    public class NetworkTime
    {
        public static double time => Time.timeAsDouble;
    }

    public enum Channels
    {
        Reliable = 0,
        Unreliable = 1
    }

    public static class Transport
    {
        public static string activeTransport => "Stub";
    }
}
#endif

