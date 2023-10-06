using Unity.VisualScripting;
using UnityEngine;

namespace  LemApperson_2D_Mobile_Adventure
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected Animator _enemy_Anim;
        [SerializeField] protected SpriteRenderer _enemy_Sprite;
        [SerializeField] protected int health, speed , gems;
        [SerializeField] protected  Transform pointA, pointB;
        [SerializeField] protected GameObject diamondPrefab;
        protected int idleID ,hitID , deathID,attackID,inCombatID, speedID;
        protected Vector2 destination, direction;
        protected bool _isIdle, _isFacingLeft, _isHit, _isInCombat, _isDead;
        protected Player _player;

        public virtual void Awake() {
            _player = FindObjectOfType<Player>();
            if(_player == null) Debug.Log("Player was not found");
            idleID = Animator.StringToHash("Idle");
            hitID = Animator.StringToHash("Hit");
            deathID = Animator.StringToHash("Death");
            attackID = Animator.StringToHash("Attack");
            inCombatID = Animator.StringToHash("InCombat");
            speedID = Animator.StringToHash("Speed");
        }
        public virtual void Update() {
            if (!_isIdle && !_isHit && !_isInCombat && !_isDead) {
                if(transform.position.x <= pointA.position.x) {
                    destination = new Vector2(pointB.position.x, pointB.position.y);
                    _enemy_Anim.SetTrigger(idleID);
                    _isFacingLeft = false;
                    _enemy_Sprite.flipX = _isFacingLeft;
                } else if(transform.position.x >= pointB.position.x) {
                    destination = new Vector2(pointA.position.x, pointA.position.y);
                    _enemy_Anim.SetTrigger(idleID);
                    _isFacingLeft = true;
                    _enemy_Sprite.flipX = _isFacingLeft;
                }
                transform.position = Vector2.MoveTowards(transform.position, 
                    new Vector2(destination.x, destination.y), speed * Time.deltaTime);
            }
            if (_isHit ){    
                if (PlayerIsClose(2.0f)) {
                    _enemy_Anim.SetBool(inCombatID, true);
                    _isInCombat = true;
                }  else {
                    _isHit = false;
                    _isInCombat = false;
                    _enemy_Sprite.flipX = _isFacingLeft;
                    _enemy_Anim.SetBool(inCombatID, false);
                }
            }
            if (_isInCombat) {
                if (direction.x > 0) {
                    _enemy_Sprite.flipX = false;
                }  else {
                    _enemy_Sprite.flipX = true;
                }
            }
        }
        
        /// <summary>
        /// The Enemy Idle & Hit animation triggers these methods
        /// </summary>
        /// <param name="EnemyIdleAnimation_stateHash"></param>
        public void OnAnimationStateEntered(int stateHash) {
            if (stateHash == idleID) {
                _isIdle = true;
            }
            if (stateHash == hitID) {
                if (!_isInCombat) {
                    _isHit = false;
                }
            }
        }

        public void OnAnimationStateExited(int stateHash) {
            if (stateHash == idleID) {
                _isIdle = false;
            }
        }

        public virtual void DestroyThisEnemy() {
            Destroy(this.gameObject);
        }

        public virtual bool PlayerIsClose(float distanceAway)
        {
            direction = _player.transform.position - transform.position;
            return ((Vector3.Distance(this.transform.position, _player.transform.position)) < distanceAway);
        }
        public virtual void Damage() {
            if(!_isHit && !_isDead) {
                health--;
                _enemy_Anim.SetTrigger(hitID);
                _isHit = true;
                if (health < 1 && !_isDead) {
                    _isDead = true;
                    _enemy_Anim.SetTrigger(deathID);
                    Invoke(nameof(DestroyThisEnemy), 5f);
                    GameObject diamond = Instantiate(diamondPrefab, transform.position, Quaternion.identity);
                    diamond.GetComponent<Diamond>()?.SetNumberOfDiamonds( gems );
                }
            }
        }
    }
}