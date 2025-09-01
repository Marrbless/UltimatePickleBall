using UnityEngine;
using PickleP2P.Core.Config;

namespace PickleP2P.Gameplay.Ball
{
    /// <summary>
    /// Ball physics setup and simple hit effect API.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class BallController : MonoBehaviour
    {
        public Rigidbody rb;
        public PhysicMaterial physicMaterial;
        private PickleP2P.Core.Net.ScoreManager scoreManager;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.mass = GameConfig.Physics.ballMassKg;
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            var sphere = GetComponent<SphereCollider>();
            sphere.radius = GameConfig.Physics.ballDiameterM / 2f;
            if (physicMaterial == null)
            {
                physicMaterial = new PhysicMaterial("BallPhys")
                {
                    bounciness = GameConfig.Physics.ballBounciness,
                    dynamicFriction = 0.3f,
                    staticFriction = 0.3f,
                    bounceCombine = PhysicMaterialCombine.Multiply
                };
            }
            sphere.material = physicMaterial;
            scoreManager = GameObject.FindObjectOfType<PickleP2P.Core.Net.ScoreManager>();
        }

        public void ApplyHit(Vector3 impulse)
        {
            rb.AddForce(impulse, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.name.Contains("Net"))
            {
                scoreManager?.OnBallNetHit();
                return;
            }
            // Ground check: Court floor only
            if (collision.collider.name.Contains("CourtFloor"))
            {
                scoreManager?.OnBallGrounded(transform.position);
            }
        }
    }
}

