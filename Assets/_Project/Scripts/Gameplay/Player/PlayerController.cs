using UnityEngine;

namespace PickleP2P.Gameplay.Player
{
    /// <summary>
    /// Simple CharacterController-based first person movement.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 4.5f;
        public float sprintMultiplier = 1.4f;
        public float gravity = -9.81f;
        public Transform head;

        private CharacterController characterController;
        private Vector3 velocity;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            if (characterController.height < 1.2f)
            {
                characterController.height = 1.8f;
                characterController.center = new Vector3(0, 0.9f, 0);
                characterController.radius = 0.3f;
            }
            if (head == null)
            {
                head = transform;
            }
        }

        private void Update()
        {
            float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            input = Vector2.ClampMagnitude(input, 1f);

            Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;
            Vector3 move = (forward * input.y + right * input.x) * speed;

            if (characterController.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            characterController.Move((move + velocity) * Time.deltaTime);
        }
    }
}

