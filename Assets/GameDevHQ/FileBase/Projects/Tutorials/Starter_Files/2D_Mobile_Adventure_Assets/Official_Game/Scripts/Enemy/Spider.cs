using UnityEngine;
using UnityEngine.Serialization;

namespace LemApperson_2D_Mobile_Adventure
{
    public class Spider : Enemy, IDamageable
    {
        public override void Awake() {
            base.Awake();
        }
        public override void Damage() {
            health--;
            if (health < 1) {
                Destroy(this.gameObject);
            }
        }
    }
}