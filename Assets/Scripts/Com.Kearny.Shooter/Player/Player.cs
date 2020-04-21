using Com.Kearny.Shooter.GameMechanics;
using UnityEngine;

namespace Com.Kearny.Shooter.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class Player : LivingEntity
    {
        private PlayerController _controller;
        private Transform _mainCameraTransform;

        public float XSensitivity { get; set; }
        public float YSensitivity { get; set; }
        public float MaxAngle { get; set; } = 75f;
        public float MoveSpeed { get; set; } = 5;
        public Camera MainCamera { get; set; }

        private readonly Quaternion _camCenter = new Quaternion(0, 0, 0, 1);

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            
            _mainCameraTransform = MainCamera.transform;
            _controller = GetComponent<PlayerController>();
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
            _controller.Move(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1f) * MoveSpeed);

            SetX();
            SetY();
        }

        private void SetY()
        {
            var yAngle = Input.GetAxis("Mouse Y") * YSensitivity * Time.deltaTime;
            var xRotation = Quaternion.AngleAxis(yAngle, -Vector3.right);
            var delta = _mainCameraTransform.localRotation * xRotation;
            var angle = Quaternion.Angle(_camCenter, delta);

            if (!(angle > -MaxAngle) || !(angle < MaxAngle)) return;

            _mainCameraTransform.localRotation = delta;
        }

        private void SetX()
        {
            var yAngle = Input.GetAxis("Mouse X") * XSensitivity * Time.deltaTime;
            var xRotation = Quaternion.AngleAxis(yAngle, Vector3.up);
            var delta = transform.localRotation * xRotation;

            _controller.Rotate(delta);
        }
    }
}