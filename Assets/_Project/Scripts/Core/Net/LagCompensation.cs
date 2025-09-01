using UnityEngine;

namespace PickleP2P.Core.Net
{
    /// <summary>
    /// Placeholder for lag compensation checks.
    /// </summary>
    public static class LagCompensation
    {
        public static bool CheckContactWithinWindow(Vector3 paddlePos, Vector3 ballPos, float windowMs = 100f)
        {
            // MVP: simple distance threshold as a stand-in
            float distance = Vector3.Distance(paddlePos, ballPos);
            return distance < 0.3f;
        }
    }
}

