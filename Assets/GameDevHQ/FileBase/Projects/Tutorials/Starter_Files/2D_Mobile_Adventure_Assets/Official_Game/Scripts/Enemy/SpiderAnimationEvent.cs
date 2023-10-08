using UnityEngine;

namespace LemApperson_2D_Mobile_Adventure.Enemy
{
    public class SpiderAnimationEvent : MonoBehaviour
    {
        [SerializeField] private Spider _spider;

        private void Start() {
            _spider = GetComponentInParent<Spider>();
        }
        
        public void Fire() {
            _spider?.Attack();
        }
    }
}