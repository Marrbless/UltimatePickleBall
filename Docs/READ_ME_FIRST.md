## PickleP2P - READ ME FIRST

Welcome to PickleP2P (Unity 2022.3 LTS, Built-in Render Pipeline). This repo contains an MVP for a first-person 3D pickleball game with online play via Mirror and optional Steam P2P transport, in-house G.711 μ-law voice, and a simple mod system.

### Quick Start (Editor)
- Open the project in Unity 2022.3 LTS.
- Open scene: `Assets/_Project/Scenes/Boot.unity`.
- Press Play. The `MainMenu` UI appears automatically.
- Click Host. A simple court spawns, you can move with WASD, mouse look, swing with Mouse0. Voice PTT is V.

### Networking Packages
- Mirror (UPM Git): add to `Packages/manifest.json`:
  "com.mirror-networking.mirror": "https://github.com/MirrorNetworking/Mirror.git#release",
- FizzySteamworks (UPM Git):
  "com.mirror-networking.fizzysteamworks": "https://github.com/MirrorNetworking/FizzySteamworks.git#release",

After adding, open Project Settings > Player > Scripting Define Symbols, add `MIRROR` (optional; Mirror DLL presence will override our stubs) and `STEAMWORKS` if using Steam.

### Steamworks.NET (Optional)
1) Import Steamworks.NET per docs. Test with AppID 480 (Spacewar) in Editor.
2) Add `STEAMWORKS` to Scripting Define Symbols.
3) In `NetworkManager` select FizzySteamworks transport if present; our bootstrap tries to add it automatically when STEAMWORKS is defined.

### Voice Chat
- In-house μ-law at 8 kHz, 20 ms frames, sent via Mirror unreliable channel.
- Push-to-talk: V. Open-mic toggle can be enabled in `VoiceChatManager` inspector.

### Modding
- Mods live in two paths (searched in order):
  - `%USERPROFILE%/AppData/LocalLow/PickleWorks/PickleP2P/Mods`
  - `Assets/StreamingAssets/Mods`
- Press F5 to hot-reload configs and textures/audio.
- See `Docs/Modding.md` for full schema and examples.

### Building
- Menu: Tools > Build > Make Windows Build
- This produces `Builds/Windows/PickleP2P.exe`.

### Testing Two Instances Locally
1) In Editor, Play, click Host.
2) Make a Windows build.
3) Run the build, click Join (uses fallback transport).
4) Move paddles, swing; ball and score sync. Hold V to send voice.

### Notes
- All Steam code is wrapped in `#if STEAMWORKS`.
- If Mirror/Steam packages are absent, the project still compiles using compile-safe stubs and minimal local behavior.

