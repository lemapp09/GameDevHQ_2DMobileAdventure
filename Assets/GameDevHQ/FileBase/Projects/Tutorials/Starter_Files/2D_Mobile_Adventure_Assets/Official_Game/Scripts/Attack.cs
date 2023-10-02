using System.Collections;
using UnityEngine;

namespace LemApperson_2D_Mobile_Adventure
{

    public class Attack : MonoBehaviour {
        private bool _coolDown;
        private void OnTriggerEnter2D(Collider2D other) {
            IDamageable hit = other.GetComponent<IDamageable>();
            if(hit != null && !_coolDown) {
                _coolDown = true;
                hit.Damage();
                StartCoroutine(CoolDown());
            }
        }

        private IEnumerator CoolDown() {
            yield return  new WaitForSeconds(0.75f);
            _coolDown = false;
        }
    }
}