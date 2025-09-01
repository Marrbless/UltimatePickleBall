using UnityEngine;
using Mirror;
using PickleP2P.Core.Utils;

namespace PickleP2P.UI.Menus
{
    /// <summary>
    /// Minimal Main Menu with Host/Join buttons rendered via IMGUI to avoid dependencies.
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        private string address = "localhost";
        private Vector2 scroll;
        private bool showMods;

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(20, 20, 360, 500), GUI.skin.box);
            scroll = GUILayout.BeginScrollView(scroll);
            GUILayout.Label("PickleP2P - Main Menu");
            GUILayout.Space(10);
            if (GUILayout.Button("Host"))
            {
                GetOrCreateNetwork().StartHost();
            }
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Address:", GUILayout.Width(60));
            address = GUILayout.TextField(address);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Join"))
            {
                GetOrCreateNetwork().StartClient();
            }
            GUILayout.Space(15);
            if (GUILayout.Button(showMods ? "Hide Mods" : "Mods")) showMods = !showMods;
            if (showMods)
            {
                GUILayout.Label("Press F5 in-game to reload Mods");
            }
            GUILayout.Space(15);
            GUILayout.Label("Controls: WASD move, Mouse look, LShift sprint, Mouse0 swing, V PTT");
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private NetworkManager GetOrCreateNetwork()
        {
            var nm = FindObjectOfType<NetworkManager>();
            if (nm == null)
            {
                var go = new GameObject("NetworkGameManager");
                nm = go.AddComponent<PickleP2P.Core.Net.NetworkGameManager>();
            }
            return nm;
        }
    }
}

