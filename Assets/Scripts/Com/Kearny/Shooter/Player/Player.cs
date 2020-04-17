using Com.Kearny.Shooter.Guns;
using UnityEngine;

namespace Com.Kearny.Shooter.Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(GunController))]
    public class Player : MonoBehaviour
    {
        private bool _cursorLocked = true;
        
        public float moveSpeed = 5;

        public float xSensitivity;
        public float ySensitivity;
        public float maxAngle = 75f;

        private PlayerController _controller;
        private Camera _mainCamera;
        private Transform _mainCameraTransform;

        private GunController _gunController;
        
        private readonly Quaternion _camCenter = new Quaternion(0,0,0,1);

        // Start is called before the first frame update
        private void Start()
        {
            _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            _mainCameraTransform = _mainCamera.transform;
            _controller = GetComponent<PlayerController>();
            _gunController = GetComponent<GunController>();
        }

        // Update is called once per frame
        private void Update()
        {
            // Body movement input
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");

            var localTransform = transform;
            var forwardMovement = localTransform.forward * verticalInput;
            var rightMovement = localTransform.right * horizontalInput;
            _controller.Move(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1f) * moveSpeed);
            
            // Look movement input
            if (_cursorLocked)
            {
                SetX();
                SetY();
            }
        }

        private void FixedUpdate()
        {
            UpdateLockCursor();
        }

        private void SetY()
        {
            var yAngle = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;
            var xRotation = Quaternion.AngleAxis(yAngle, -Vector3.right);
            var delta = _mainCameraTransform.localRotation * xRotation;
            var angle = Quaternion.Angle(_camCenter, delta);
            
            if (!(angle > -maxAngle) || !(angle < maxAngle)) return;
            
            _mainCameraTransform.localRotation = delta;
        }

        private void SetX()
        {
            var yAngle = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
            var xRotation = Quaternion.AngleAxis(yAngle, Vector3.up);
            var delta = transform.localRotation * xRotation;

            _controller.Rotate(delta);
        }

        private void UpdateLockCursor()
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
    }
}