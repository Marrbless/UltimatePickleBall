using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace PickleP2P.Core.Mods.Loaders
{
    public static class AudioLoader
    {
        public static IEnumerator LoadAudioClipCoroutine(string path, AudioType type, System.Action<AudioClip> onLoaded)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                onLoaded?.Invoke(null);
                yield break;
            }
            using (var req = UnityWebRequestMultimedia.GetAudioClip("file://" + path, type))
            {
                yield return req.SendWebRequest();
                if (req.result != UnityWebRequest.Result.Success)
                {
                    onLoaded?.Invoke(null);
                }
                else
                {
                    var clip = DownloadHandlerAudioClip.GetContent(req);
                    onLoaded?.Invoke(clip);
                }
            }
        }
    }
}

