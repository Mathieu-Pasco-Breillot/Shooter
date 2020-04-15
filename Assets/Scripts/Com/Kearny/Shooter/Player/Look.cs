using UnityEngine;

namespace Com.Kearny.Shooter.Player
{
    public class Look : MonoBehaviour
    {
        #region Variables

        private static bool _cursorLocked = true;

        public Transform player;
        public Transform cameras;
        public Transform weapon;

        public float xSensitivity;
        public float ySensitivity;

        public float maxAngle = 90f;

        private readonly Quaternion camCenter = new Quaternion(0, 0, 0, 1);

        #endregion

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (_cursorLocked)
            {
                SetY();
                SetX();
            }

            UpdateLockCursor();
        }

        #endregion

        #region Private Methods

        private void SetY()
        {
            var yAngle = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;
            var xRotation = Quaternion.AngleAxis(yAngle, -Vector3.right);
            var delta = cameras.localRotation * xRotation;
            var angle = Quaternion.Angle(camCenter, delta);
            
            if (!(angle > -maxAngle) || !(angle < maxAngle)) return;
            
            cameras.localRotation = delta;
            weapon.rotation = cameras.rotation;
        }

        private void SetX()
        {
            var yAngle = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
            var xRotation = Quaternion.AngleAxis(yAngle, Vector3.up);
            var delta = player.localRotation * xRotation;

            player.localRotation = delta;
        }

        private static void UpdateLockCursor()
        {
            if (_cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _cursorLocked = false;
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _cursorLocked = true;
                }
            }
        }

        #endregion
    }
}