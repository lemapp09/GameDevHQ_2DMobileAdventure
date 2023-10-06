using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace LemApperson_2D_Mobile_Adventure
{
    public class Spider : Enemy, IDamageable
    {
        [SerializeField] private GameObject acidPrefab;
        [SerializeField] private float _timeBetweenChecks = 0.2f; // check for closeness of Player 5 times/ second
        private float _timeOfLastChecks;
        
        public override void Awake() {
            base.Awake();
        }
        
        public override void Damage() {
            health--;
            if (health < 1) {
                _isDead = true;
                GameObject diamond = Instantiate(diamondPrefab, transform.position, Quaternion.identity);
                diamond.GetComponent<Diamond>()?.SetNumberOfDiamonds( gems );
                _enemy_Anim.SetTrigger(deathID);
                Destroy(this.gameObject, 0.8f);
            }
        }
         
        public override void Update() {
            if (Time.time - _timeOfLastChecks > _timeBetweenChecks) {
                _timeOfLastChecks = Time.time;
                if (PlayerIsClose(6.0f)) {
                    _enemy_Anim.SetBool(inCombatID, true);
                }  else  {
                    _enemy_Anim.SetBool(inCombatID, false);
                }
            }
        }

        public void Attack() {
            GameObject acidAttack = Instantiate(acidPrefab, transform.position, Quaternion.identity);
            acidAttack.name = "AcidAttack";
            _enemy_Anim.SetFloat(speedID, Random.Range(0.5f, 1.5f));
        }
    }
}