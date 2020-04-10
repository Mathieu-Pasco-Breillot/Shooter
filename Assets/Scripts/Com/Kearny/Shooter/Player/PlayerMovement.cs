using System.Collections;
using UnityEngine;

namespace Com.Kearny.Shooter.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Variables

        [Header("Player Motor")] [Range(1f, 15f)]
        public float walkSpeed;

        [Range(1f, 15f)] public float runSpeed;
        [Range(1f, 15f)] public float jumpForce;

        public Camera normalCamera;
        public Transform weaponParent;

        // FOV
        private float _baseFov;
        private const float SprintFovModifier = 1.15f;
        private const float FovChangeSpeed = 5f;

        private CharacterController _characterController;

        // Head Bob
        private Vector3 _weaponParentOrigin;
        private float _movementCounter;
        private float _idleCounter;
        private Vector3 _targetWeaponBobPosition;

        #endregion

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        private void Start()
        {
            _baseFov = normalCamera.fieldOfView;
            if (Camera.main != null) Camera.main.enabled = false;
            _characterController = GetComponent<CharacterController>();
            _weaponParentOrigin = weaponParent.localPosition;
        }

        // Update is called once per frame
        private void Update()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");

            var isGrounded = _characterController.isGrounded;
            var localTransform = transform;
            Vector3 fwdMovement = isGrounded ? localTransform.forward * verticalInput : Vector3.zero;
            Vector3 rightMovement = isGrounded ? localTransform.right * horizontalInput : Vector3.zero;

            float speed;
            if (Input.GetKey(KeyCode.LeftShift) && ((verticalInput != 0) || (horizontalInput != 0)))
                speed = Sprint();
            else
                speed = Walk();

            _characterController.SimpleMove(Vector3.ClampMagnitude(fwdMovement + rightMovement, 1f) * speed);

            // Jumping
            if (_characterController.isGrounded)
                Jump();

            Idle(horizontalInput, verticalInput);
        }

        private void Idle(float horizontalInput, float verticalInput)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (horizontalInput != 0 || verticalInput != 0) return;
            
            HeadBob(_idleCounter, 0.01f, 0.01f);
            _idleCounter += Time.deltaTime;
        }

        #endregion

        #region Private Methods

        private void HeadBob(float z, float xIntensity, float yIntensity)
        {
            _targetWeaponBobPosition = _weaponParentOrigin + new Vector3(Mathf.Cos(z) * xIntensity,
                Mathf.Sin(z * 2) * yIntensity,
                0);
            
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, _targetWeaponBobPosition, Time.deltaTime *8f);
        }

        private float Walk()
        {
            normalCamera.fieldOfView = Mathf.Lerp(normalCamera.fieldOfView, _baseFov, Time.deltaTime * FovChangeSpeed);

            HeadBob(_movementCounter, 0.05f, 0.05f);
            _movementCounter += Time.deltaTime * 2;

            return walkSpeed;
        }

        private float Sprint()
        {
            normalCamera.fieldOfView =
                Mathf.Lerp(normalCamera.fieldOfView, _baseFov * SprintFovModifier, Time.deltaTime * FovChangeSpeed);

            HeadBob(_movementCounter, 0.05f, 0.05f);
            _movementCounter += Time.deltaTime * 4;

            return runSpeed;
        }

        private void Jump()
        {
            if (Input.GetButtonDown("Jump"))
            {
                StartCoroutine(PerformJumpRoutine());
            }
        }

        private IEnumerator PerformJumpRoutine()
        {
            var jump = jumpForce;

            do
            {
                HeadBob(0, 0, 0);

                _characterController.Move(Vector3.up * (jump * Time.deltaTime));
                jump -= Time.deltaTime;
                yield return null;
            } while (!_characterController.isGrounded);
        }

        #endregion
    }
}