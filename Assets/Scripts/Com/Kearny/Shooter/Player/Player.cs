using Com.Kearny.Shooter.GameMechanics;
using Com.Kearny.Shooter.Guns;
using UnityEngine;

namespace Com.Kearny.Shooter.Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(GunController))]
    public class Player : LivingEntity
    {
        private Transform _mainCameraTransform;

        public float moveSpeed = 5;

        public float xSensitivity;
        public float ySensitivity;
        public float maxAngle = 75f;

        private PlayerController _controller;
        public Camera _mainCamera;

        private GunController _gunController;

        private readonly Quaternion _camCenter = new Quaternion(0, 0, 0, 1);

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            
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

            SetX();
            SetY();
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
    }
}