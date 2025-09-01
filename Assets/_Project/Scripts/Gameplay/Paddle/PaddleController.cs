using UnityEngine;
using PickleP2P.Core.Net;

namespace PickleP2P.Gameplay.Paddle
{
    /// <summary>
    /// Handles paddle swing timing and provides a collider area.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class PaddleController : MonoBehaviour
    {
        public Transform pivot;
        public float swingDuration = 0.12f; // 120 ms
        public float swingAngle = 60f;
        public KeyCode swingKey = KeyCode.Mouse0;
        public float hitPower = 6f;

        private float swingTimer;
        private bool swinging;
        private Quaternion restRotation;

        private void Awake()
        {
            if (pivot == null)
            {
                var go = new GameObject("PaddlePivot");
                go.transform.SetParent(transform, false);
                pivot = go.transform;
            }
            restRotation = pivot.localRotation;

            var col = GetComponent<BoxCollider>();
            if (col.size == Vector3.zero)
            {
                col.size = new Vector3(0.25f, 0.02f, 0.3f);
                col.center = new Vector3(0, 0, 0.15f);
            }
            col.isTrigger = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(swingKey) && !swinging)
            {
                swinging = true;
                swingTimer = 0f;
            }

            if (swinging)
            {
                swingTimer += Time.deltaTime;
                float t = Mathf.Clamp01(swingTimer / swingDuration);
                float angle = Mathf.Sin(t * Mathf.PI) * swingAngle; // ease swing
                pivot.localRotation = restRotation * Quaternion.Euler(-angle, 0, 0);
                if (t >= 1f)
                {
                    swinging = false;
                    pivot.localRotation = restRotation;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!swinging) return;
            var ball = other.GetComponent<PickleP2P.Gameplay.Ball.BallController>();
            if (ball == null) return;
            Vector3 direction = pivot.forward; // face normal
            Vector3 relativeVel = direction * hitPower;
            ball.ApplyHit(relativeVel);
        }
    }
}

