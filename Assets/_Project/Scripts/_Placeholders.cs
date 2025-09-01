using UnityEngine;

// Placeholder prefabs creation helper (no editor steps needed). Not required to run.
public static class Placeholders
{
    public static GameObject CreateQuadPaddle()
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        go.name = "PaddleMesh";
        go.transform.localScale = new Vector3(0.2f, 0.3f, 1f);
        return go;
    }
}

