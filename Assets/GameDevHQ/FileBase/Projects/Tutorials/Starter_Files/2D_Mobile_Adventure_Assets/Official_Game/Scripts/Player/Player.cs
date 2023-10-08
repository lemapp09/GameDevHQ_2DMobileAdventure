using System;
using System.Collections;
using LemApperson_2D_Mobile_Adventure.Managers;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace  LemApperson_2D_Mobile_Adventure.Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        private PlayerMovement _input;
        private StarterAssetsInputs _starterAssetsInputs;
        private UnityEvent joystickOutputEvent;
        [SerializeField] private bool _isAndroid;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _playerSpriteRenderer;
        [SerializeField] private SpriteRenderer _swordArcSpriteRenderer;
        [SerializeField] private int _health = 4, jumpCount = 0, maxJumps = 2;
        [SerializeField] private float _speed = 1.0f,  _jumpForce = 7.5f;
        [SerializeField] private PlayerAnimation _playerAnim;
        [SerializeField] private LayerMask _groundLayer;
        private bool _resetJump, _isInShop;
        public int  Health { get; set; }
        // variable for amount of diamonds
        [SerializeField] private int _numberOfDiamonds = 1000;

        private void Awake()
        {
            if(joystickOutputEvent == null) joystickOutputEvent = new UnityEvent();
            // joystickOutputEvent.AddListener(movementJS);
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

        public void Damage() {
            _health--;
            UIManager.Instance.UpDateHealthCount(_health);
            if (_health < 1) {
                if (_playerAnim != null) {
                    _playerAnim.Death();
                }
                AudioManager.Instance.SFX(5);
                GameManager.Instance.LoadMainMenu();
            }
        }

        public void CollectGems(int NumberOfGems) {
            _numberOfDiamonds += NumberOfGems;
            UIManager.Instance.UpDateGemCount(_numberOfDiamonds);
        }

        public int HowManyGems() {
            return _numberOfDiamonds;
        }

        public void DeductGems(int NumberToDeduct) {
            _numberOfDiamonds -= NumberToDeduct;
            if (_numberOfDiamonds < 0) {
                _numberOfDiamonds = 0;
            }
        }

        public void movementJS(Vector2 pointerPosition) {
            if (_isAndroid)
            {
                int move = (int)Mathf.Clamp(pointerPosition.x, -1, 1);
                MovePlayer(move);
                if (move > 0)
                {
                    _playerSpriteRenderer.flipX = false;
                    _swordArcSpriteRenderer.flipX = false;
                    _swordArcSpriteRenderer.transform.rotation = Quaternion.Euler(66, 48, -80);
                }
                else if (move < 0)
                {
                    _playerSpriteRenderer.flipX = true;
                    _swordArcSpriteRenderer.flipX = true;
                    _swordArcSpriteRenderer.transform.rotation = Quaternion.Euler(66, 48, 80);
                }
            }
        }
        
        private void MoveRight(InputAction.CallbackContext context)
        {
            if (!_isAndroid)
            {
                MovePlayer(_input.Player.MoveRight.ReadValue<float>());
                _playerSpriteRenderer.flipX = false;
                _swordArcSpriteRenderer.flipX = false;
                _swordArcSpriteRenderer.transform.rotation = Quaternion.Euler(66, 48, -80);
            }
        }
        
        private void MoveLeft(InputAction.CallbackContext context) {
            if (!_isAndroid)
            {
                MovePlayer(_input.Player.MoveLeft.ReadValue<float>() * -1.0f);
                _playerSpriteRenderer.flipX = true;
                _swordArcSpriteRenderer.flipX = true;
                _swordArcSpriteRenderer.transform.rotation = Quaternion.Euler(66, 48, 80);
            }
        }

        private void Jump(InputAction.CallbackContext context) {
            if (!_isAndroid)
            {
                if (CheckIsGrounded() || (jumpCount > 0 && jumpCount < maxJumps) )
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                    jumpCount++;
                    StartCoroutine(ResetJumpRoutine());
                }
            }
        }

        public void Jump() {
            if (_isAndroid) {
                if (CheckIsGrounded()  || (jumpCount > 0 && jumpCount < maxJumps)) {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                    jumpCount++;
                    StartCoroutine(ResetJumpRoutine());
                }
            }
        }

        public void IncreaseJumpForce() {
            _jumpForce += 5;
        }
        
        private void Swing(InputAction.CallbackContext context) { // AKA Attack
            if (!_isAndroid && !_isInShop) {
                if (CheckIsGrounded() && _playerAnim != null) {
                    _playerAnim.Swing();
                    AudioManager.Instance.SFX(4);
                }
            }
        }
        
        public void Swing() { // AKA Attack
            if (_isAndroid) {
                if (CheckIsGrounded() && _playerAnim != null) {
                    _playerAnim.Swing();
                    AudioManager.Instance.SFX(4);
                }
            }
        }

        private void MovePlayer(float horizontalInput) {
            if (_rigidbody != null && _playerAnim != null) {
                _rigidbody.velocity = new Vector2(horizontalInput * _speed, _rigidbody.velocity.y);
                _playerAnim.Move(horizontalInput);
            }
        }

        private bool CheckIsGrounded() {
            if (!_resetJump) {
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down,
                    0.76f, _groundLayer);
                if (hitInfo.collider != null) {
                    return true;
                }
            }
            return false;
        }


        private IEnumerator ResetJumpRoutine() {
            if (_playerAnim != null && jumpCount == 1) {
                _playerAnim.Jump(true);
                _resetJump = true;
                yield return new WaitForSeconds(0.74f);
                _playerAnim.Jump(false);
                _resetJump = false;
            }
            if (jumpCount > 1) {
                yield return new WaitForSeconds(1f);
                jumpCount = 0;
            }
        }

        public void EntersShop() {
            _isInShop = true;
        }
        
        public void ExitsShop() {
                _isInShop = false;
        }
            
        private void OnEnable() {
            _input.Enable();
            _input.Player.Enable();
        }

        private void OnDisable() {
            _input.Disable();
            _input.Player.Disable();
        }

        public void AddALife() {
            if (_health < 4) {
                _health++;
                UIManager.Instance.UpDateHealthCount(_health);
            }
        }
    }
}