using UnityEngine;
using PickleP2P.Core.Net;

namespace PickleP2P.UI.Hud
{
    /// <summary>
    /// Minimal HUD: score and voice indicator.
    /// </summary>
    public class HudController : MonoBehaviour
    {
        public ScoreManager scoreManager;
        public PickleP2P.Core.Voice.VoiceChatManager voice;

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width - 220, 20, 200, 120), GUI.skin.box);
            if (scoreManager != null)
            {
                GUILayout.Label($"Score A: {scoreManager.scoreA}");
                GUILayout.Label($"Score B: {scoreManager.scoreB}");
            }
            if (voice != null)
            {
                GUILayout.Label("PTT: V");
            }
            GUILayout.EndArea();
        }
    }
}

