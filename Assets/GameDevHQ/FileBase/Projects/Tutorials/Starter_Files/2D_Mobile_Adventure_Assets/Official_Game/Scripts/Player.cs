using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace  LemApperson_2D_Mobile_Adventure
{
    public class Player : MonoBehaviour
    {
        private PlayerMovement _input;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _jumpForce = 5.0f;
        [SerializeField] private LayerMask _groundLayer;
        private bool _resetJump;

        private void Awake() {
            _input = new PlayerMovement();
            _input.Player.Jump.performed += Jump;
            _input.Player.MoveLeft.performed += MoveLeft;
            _input.Player.MoveLeft.canceled += MoveLeft;
            _input.Player.MoveRight.performed += MoveRight;
            _input.Player.MoveRight.canceled += MoveRight;
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        private void MoveRight(InputAction.CallbackContext context) {
            MovePlayer(_input.Player.MoveRight.ReadValue<float>());
        }

        private void MoveLeft(InputAction.CallbackContext context) {
            MovePlayer(_input.Player.MoveLeft.ReadValue<float>() * -1.0f);
        }

        private void Jump(InputAction.CallbackContext context) {
            if (CheckIsGrounded())
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                StartCoroutine(ResetJumpRoutine());
            }
        }
        
        private void OnEnable() {
            _input.Enable();
        }

        private void OnDisable() {
            _input.Disable();
        }

        private void MovePlayer(float horizontalInput) {
            _rigidbody.velocity = new Vector2(horizontalInput * _speed, _rigidbody.velocity.y);
        }

        private bool CheckIsGrounded() {
            if (!_resetJump) {
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 1f, _groundLayer);
                if (hitInfo.collider != null) {
                    return true;
                }
            }
            return false;
        }

        private IEnumerator ResetJumpRoutine() {
            _resetJump = true;
            yield return new WaitForSeconds(0.1f);
            _resetJump = false;
        }
    }
}