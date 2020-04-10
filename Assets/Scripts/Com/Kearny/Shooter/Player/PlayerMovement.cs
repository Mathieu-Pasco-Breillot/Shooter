using System;
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
        private float _baseFov;
        private const float SprintFovModifier = 1.15f;

        private CharacterController _characterController;
        private const float FovChangeSpeed = 5f;

        #endregion

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        private void Start()
        {
            _baseFov = normalCamera.fieldOfView;
            if (Camera.main != null) Camera.main.enabled = false;
            _characterController = GetComponent<CharacterController>();
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

            var speed = Input.GetKey(KeyCode.LeftShift) ? Sprint() : Walk();

            _characterController.SimpleMove(Vector3.ClampMagnitude(fwdMovement + rightMovement, 1f) * speed);

            if (_characterController.isGrounded)
                Jump();
        }

        #endregion

        #region Private Methods

        private float Walk()
        {
            normalCamera.fieldOfView = Mathf.Lerp(normalCamera.fieldOfView, _baseFov, Time.deltaTime * FovChangeSpeed);

            return walkSpeed;
        }

        private float Sprint()
        {
            normalCamera.fieldOfView =
                Mathf.Lerp(normalCamera.fieldOfView, _baseFov * SprintFovModifier, Time.deltaTime * FovChangeSpeed);

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
                _characterController.Move(Vector3.up * (jump * Time.deltaTime));
                jump -= Time.deltaTime;
                yield return null;
            } while (!_characterController.isGrounded);
        }

        #endregion
    }
}