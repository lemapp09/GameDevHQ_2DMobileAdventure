using UnityEngine;

namespace  LemApperson_2D_Mobile_Adventure
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected Animator _enemy_Anim;
        [SerializeField] protected SpriteRenderer _enemy_Sprite;
        [SerializeField] protected int health;
        [SerializeField] protected int speed;
        [SerializeField] protected int gems;
        [SerializeField] protected  Transform pointA, pointB;
        protected int _idleID;
        protected Vector2 destination;
        protected bool _isIdle;


        public virtual void Awake() {
            _idleID = Animator.StringToHash("Idle");
        }
        public virtual void Update()
        {
            _isIdle = _enemy_Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle");
            if (!_isIdle) {
                if(transform.position.x <= pointA.position.x) {
                    destination = new Vector2(pointB.position.x, pointB.position.y);
                    _enemy_Anim.SetTrigger(_idleID);
                    _enemy_Sprite.flipX = false;
                } else if(transform.position.x >= pointB.position.x) {
                    destination = new Vector2(pointA.position.x, pointA.position.y);
                    _enemy_Anim.SetTrigger(_idleID);
                    _enemy_Sprite.flipX = true;
                }
                transform.position = Vector2.MoveTowards(transform.position, 
                    new Vector2(destination.x, destination.y), speed * Time.deltaTime);
            }
        }
    }
}