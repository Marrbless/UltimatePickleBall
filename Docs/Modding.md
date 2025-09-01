## PickleP2P Modding Guide

No code required. Mods live in one of two locations (searched in order):
- `%USERPROFILE%/AppData/LocalLow/PickleWorks/PickleP2P/Mods`
- `Assets/StreamingAssets/Mods` (shipped defaults)

### Manifest: Mods/pack.json
Example:
```
{
  "assets": {
    "paddle": {"model": "models/paddle.obj", "mtl": "models/paddle.mtl", "scale": [1,1,1], "rotateEuler": [0,0,0], "offset": [0,0,0]},
    "ball":   {"model": "models/ball.obj"},
    "net":    {"model": "models/net.obj"}
  },
  "textures": {
    "court_diffuse": "textures/court_diffuse.png",
    "paddle_albedo": "textures/paddle_albedo.png"
  },
  "audio": {
    "hit": "audio/hit.wav",
    "bounce": "audio/bounce.wav",
    "serve": "audio/serve.wav"
  },
  "rules": "config/rules.json",
  "physics": "config/physics.json",
  "camera": "config/camera.json"
}
```

### Supported Overrides
- **Models**: OBJ + MTL with `v`, `vt`, `vn`, `usemtl`, `map_Kd`. Textures referenced in MTL are relative to the MTL file.
- **Textures**: PNG/JPG via `Texture2D.LoadImage`.
- **Audio**: WAV/OGG via `UnityWebRequestMultimedia.GetAudioClip`.
- **Config**: `rules.json`, `physics.json`, `camera.json`.

### Defaults
`rules.json`:
```
{ "courtWidthM": 6.096, "courtLengthM": 13.411, "kitchenDepthM": 2.134, "netSidelineM": 0.914, "netCenterM": 0.864, "pointsToWin": 11, "winBy": 2 }
```

`physics.json`:
```
{ "ballMassKg": 0.024, "ballDiameterM": 0.074, "ballBounciness": 0.5, "paddleHitBoost": 1.0, "gravity": -9.81, "fixedTimestep": 0.0166667 }
```

`camera.json`:
```
{ "fov": 85, "sensitivity": 1.0 }
```

### Hot Reload
- Press F5 to reload configs and textures/audio at runtime. Models are reloaded on restart for safety.
- Console (`Backquote`) has a "Reload Mods" button.

### Transform Overrides
- In `pack.json` under `assets.paddle/ball/net`, set:
  - `scale`: `[x,y,z]`
  - `rotateEuler`: `[x,y,z]` degrees
  - `offset`: `[x,y,z]` meters

### Auto Colliders
- A bounds-fitting collider is added to loaded OBJ meshes automatically. Override by adding your own collider in-scene if needed.

### 3-Minute Replacement Tutorial
1. Copy your new texture to `Assets/StreamingAssets/Mods/textures/court_diffuse.png`.
2. Play the game; press F5. The court updates live.
3. Replace `Assets/StreamingAssets/Mods/models/paddle.obj` and matching `.mtl`.
4. Restart Play Mode; the new paddle model loads with a sensible collider.
5. Replace `Assets/StreamingAssets/Mods/audio/hit.wav`; next rally plays the new sound.

### Troubleshooting
- If an asset fails to load, the log shows `[MODS]` messages with the failing path.
- If your model appears too big/small or offset, use the transform overrides.
- Ensure OBJ uses triangles/quads; complex polygons are triangulated during import.

