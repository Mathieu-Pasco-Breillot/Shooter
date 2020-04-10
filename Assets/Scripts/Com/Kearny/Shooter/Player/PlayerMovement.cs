using System;
using System.Collections;
using UnityEngine;

namespace Com.Kearny.Shooter.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Player Motor")] [Range(1f, 15f)]
        public float walkSpeed;

        [Range(1f, 15f)] public float runSpeed;
        [Range(1f, 15f)] public float jumpForce;

        private CharacterController _characterController;

        // Start is called before the first frame update
        private void Start()
        {
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

            Console.WriteLine(Input.GetKeyDown(KeyCode.LeftShift));

            var speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            _characterController.SimpleMove(Vector3.ClampMagnitude(fwdMovement + rightMovement, 1f) * speed);

            if (_characterController.isGrounded)
                Jump();
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
    }
}