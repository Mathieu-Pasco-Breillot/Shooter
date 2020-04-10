using UnityEngine;

namespace Com.Kearny.Shooter.Player
{
    public class Look : MonoBehaviour
    {
        public static bool cursorLocked = true;

        public Transform player;

        public Transform playerEyes;

        public float xSensitivity;
        public float ySensitivity;

        public float minYAngle = -65f;
        public float maxYAngle = 30f;

        private readonly Quaternion _camCenter = new Quaternion(0, 0, 0, 1);

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (cursorLocked)
            {
                SetY();
                SetX();
            }

            UpdateLockCursor();
        }

        private void SetY()
        {
            var yAngle = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;
            var xRotation = Quaternion.AngleAxis(yAngle, -Vector3.right);
            var delta = playerEyes.localRotation * xRotation;

            var angle = Quaternion.Angle(_camCenter, delta);
            if (angle > minYAngle && angle < maxYAngle)
                playerEyes.localRotation = delta;
        }

        private void SetX()
        {
            var yAngle = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
            var xRotation = Quaternion.AngleAxis(yAngle, Vector3.up);
            var delta = player.localRotation * xRotation;

            player.localRotation = delta;
        }

        void UpdateLockCursor()
        {
            if (cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    cursorLocked = false;
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    cursorLocked = true;
                }
            }
        }
    }
}