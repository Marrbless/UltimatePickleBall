using UnityEngine;
using PickleP2P.Core.Config;

namespace PickleP2P.Gameplay.Player
{
    /// <summary>
    /// Mouse-look first-person camera.
    /// </summary>
    public class FirstPersonCamera : MonoBehaviour
    {
        public Transform target;
        public float sensitivity = 1f;
        public float minPitch = -80f;
        public float maxPitch = 80f;

        private float yaw;
        private float pitch;
        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
            if (cam == null)
            {
                var go = new GameObject("PlayerCamera");
                cam = go.AddComponent<Camera>();
            }
            cam.fieldOfView = GameConfig.Camera.fov;
            sensitivity = GameConfig.Camera.sensitivity;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (target == null) return;

            float mouseX = Input.GetAxis("Mouse X") * sensitivity * 120f * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * 120f * Time.deltaTime;

            yaw += mouseX;
            pitch = Mathf.Clamp(pitch - mouseY, minPitch, maxPitch);

            target.rotation = Quaternion.Euler(0, yaw, 0);
            cam.transform.position = target.position;
            cam.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        }
    }
}

