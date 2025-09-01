using UnityEngine;
using PickleP2P.Core.Mods;
using PickleP2P.Core.Net;

namespace PickleP2P.Core.Utils
{
    /// <summary>
    /// Backtick console with simple diagnostics and actions.
    /// </summary>
    public class DebugConsole : MonoBehaviour
    {
        private bool open;
        private Vector2 scroll;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote)) open = !open;
        }

        private void OnGUI()
        {
            if (!open) return;
            GUILayout.BeginArea(new Rect(20, Screen.height - 260, 360, 240), GUI.skin.box);
            scroll = GUILayout.BeginScrollView(scroll);
            GUILayout.Label("Debug Console");
            GUILayout.Label($"Time: {Time.time:F2}");
            GUILayout.Label($"FixedDelta: {Time.fixedDeltaTime:F3}");
            if (GUILayout.Button("Reload Mods"))
            {
                ModManager.Instance?.LoadAll();
            }
            if (GUILayout.Button("Reset Rally"))
            {
                var ball = GameObject.FindObjectOfType<Gameplay.Ball.BallController>();
                if (ball != null)
                {
                    ball.transform.position = new Vector3(0, 1.5f, 0);
                    ball.rb.velocity = Vector3.zero;
                    ball.rb.angularVelocity = Vector3.zero;
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}

