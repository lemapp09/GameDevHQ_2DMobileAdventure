using UnityEngine;

namespace  LemApperson_2D_Mobile_Adventure.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator[] _anim;
        private int _moveID,_jumpID, _swingID, _deathID;


        private void Awake() {
            // _anim = GetComponentsInChildren<Animator>();
            _moveID = Animator.StringToHash("Move");
            _jumpID = Animator.StringToHash("Jump");
            _swingID = Animator.StringToHash("Swing");
            _deathID = Animator.StringToHash("Death");
        }

        public void Move(float move) {
            _anim[0].SetFloat(_moveID, Mathf.Abs(move));
        }

        public void Jump(bool IsJumping) {
            _anim[0].SetBool(_jumpID, IsJumping);
        }

        public void Swing() {
            _anim[0].SetBool(_swingID, true);
            _anim[1].SetBool(_swingID, true);
        }

        public void Death()
        {
            _anim[0].SetTrigger(_deathID);
        }
    }
}