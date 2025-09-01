using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PickleP2P.Core.Voice.Codec;
using PickleP2P.Core.Utils;

namespace PickleP2P.Core.Voice
{
    /// <summary>
    /// Captures microphone, encodes mu-law, sends via Mirror unreliable channel, 3D playback per speaker.
    /// </summary>
    public class VoiceChatManager : NetworkBehaviour
    {
        public static VoiceChatManager Instance;

        [Header("Capture")]
        public int sampleRate = 8000;
        public int frameMs = 20; // 20ms frames
        public KeyCode pushToTalk = KeyCode.V;
        public bool openMic;
        public float inputGain = 1f;
        public float maxDistance = 20f;

        private AudioClip micClip;
        private string device;
        private int samplesPerFrame;
        private int micReadPos;
        private float[] micBuffer;
        private short[] pcm16Buffer;
        private byte[] muBuffer;

        private readonly Dictionary<uint, Speaker> speakers = new Dictionary<uint, Speaker>();

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
            samplesPerFrame = sampleRate * frameMs / 1000;
            micBuffer = new float[samplesPerFrame];
            pcm16Buffer = new short[samplesPerFrame];
            muBuffer = new byte[samplesPerFrame];
        }

        public override void OnStartLocalPlayer()
        {
            StartMicrophone();
        }

        private void OnDestroy()
        {
            StopMicrophone();
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            bool capturing = openMic || Input.GetKey(pushToTalk);
            if (!capturing || micClip == null) return;

            int micPos = Microphone.GetPosition(device);
            int framesAvailable = micPos - micReadPos;
            if (framesAvailable < samplesPerFrame) return;

            micClip.GetData(micBuffer, micReadPos);
            micReadPos = (micReadPos + samplesPerFrame) % micClip.samples;

            for (int i = 0; i < samplesPerFrame; i++)
            {
                float f = Mathf.Clamp(micBuffer[i] * inputGain, -1f, 1f);
                pcm16Buffer[i] = (short)(f * short.MaxValue);
            }
            G711MuLaw.Encode(pcm16Buffer, samplesPerFrame, muBuffer);
            CmdSendVoice(muBuffer, samplesPerFrame);
        }

        [Command(channel = Channels.Unreliable)]
        private void CmdSendVoice(byte[] mu, int length)
        {
            RpcReceiveVoice(mu, length, netId);
        }

        [ClientRpc(channel = Channels.Unreliable)]
        private void RpcReceiveVoice(byte[] mu, int length, uint speakerNetId)
        {
            if (!speakers.TryGetValue(speakerNetId, out var speaker))
            {
                var go = new GameObject($"Speaker_{speakerNetId}");
                var src = go.AddComponent<AudioSource>();
                src.spatialBlend = 1f;
                src.rolloffMode = AudioRolloffMode.Linear;
                src.maxDistance = maxDistance;
                src.playOnAwake = false;
                speaker = new Speaker { source = src };
                speakers[speakerNetId] = speaker;
            }

            // Decode and play
            if (speaker.pcm16 == null || speaker.pcm16.Length != length)
            {
                speaker.pcm16 = new short[length];
                speaker.floatBuffer = new float[length];
            }
            G711MuLaw.Decode(mu, length, speaker.pcm16);
            for (int i = 0; i < length; i++) speaker.floatBuffer[i] = speaker.pcm16[i] / 32768f;
            var clip = AudioClip.Create("voice", length, 1, sampleRate, false);
            clip.SetData(speaker.floatBuffer, 0);
            speaker.source.clip = clip;
            speaker.source.Play();
        }

        private void StartMicrophone()
        {
            if (Microphone.devices.Length == 0)
            {
                Log.Warn(LogCategory.VOICE, "No microphone devices found.");
                return;
            }
            device = Microphone.devices[0];
            micClip = Microphone.Start(device, true, 1, sampleRate);
            micReadPos = 0;
        }

        private void StopMicrophone()
        {
            if (micClip != null && !string.IsNullOrEmpty(device))
            {
                Microphone.End(device);
                micClip = null;
            }
        }

        private class Speaker
        {
            public AudioSource source;
            public short[] pcm16;
            public float[] floatBuffer;
        }
    }
}

