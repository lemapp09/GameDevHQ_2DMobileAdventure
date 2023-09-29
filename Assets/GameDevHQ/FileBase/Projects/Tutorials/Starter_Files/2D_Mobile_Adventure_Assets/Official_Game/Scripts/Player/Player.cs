using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace  LemApperson_2D_Mobile_Adventure
{
    public class Player : MonoBehaviour
    {
        private PlayerMovement _input;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _playerSpriteRenderer;
        [SerializeField] private SpriteRenderer _swordArcSpriteRenderer;
        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _jumpForce = 5.0f;
        [SerializeField] private PlayerAnimation _playerAnim;
        [SerializeField] private LayerMask _groundLayer;
        private bool _resetJump;

        private void Awake() {
            _input = new PlayerMovement();
            _input.Player.Jump.performed += Jump;
            _input.Player.MoveLeft.performed += MoveLeft;
            _input.Player.MoveLeft.canceled += MoveLeft;
            _input.Player.MoveRight.performed += MoveRight;
            _input.Player.MoveRight.canceled += MoveRight;
            _input.Player.Swing.performed += Swing; // AKA Attack
            _playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce * 0.33f);
        }


        private void MoveRight(InputAction.CallbackContext context) {
            MovePlayer(_input.Player.MoveRight.ReadValue<float>());
            _playerSpriteRenderer.flipX = false;
            _swordArcSpriteRenderer.flipX = false;
            _swordArcSpriteRenderer.transform.rotation = Quaternion.Euler(66, 48, -80);
        }

        private void MoveLeft(InputAction.CallbackContext context) {
            MovePlayer(_input.Player.MoveLeft.ReadValue<float>() * -1.0f);
            _playerSpriteRenderer.flipX = true;
            _swordArcSpriteRenderer.flipX = true;
            _swordArcSpriteRenderer.transform.rotation = Quaternion.Euler(66, 48, 80);
        }

        private void Jump(InputAction.CallbackContext context) {
            if (CheckIsGrounded()) {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                StartCoroutine(ResetJumpRoutine());
            }
        }
        
        private void Swing(InputAction.CallbackContext context) { // AKA Attack
            if (CheckIsGrounded()) {
                _playerAnim.Swing();
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
            _playerAnim.Move(horizontalInput);
        }

        private bool CheckIsGrounded() {
            if (!_resetJump) {
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.76f, _groundLayer);
                if (hitInfo.collider != null) {
                    return true;
                }
            }
            return false;
        }

        private IEnumerator ResetJumpRoutine()
        {
            _playerAnim.Jump(true);
            _resetJump = true;
            yield return new WaitForSeconds(0.74f);
            _playerAnim.Jump(false);
            _resetJump = false;
        }
    }
}