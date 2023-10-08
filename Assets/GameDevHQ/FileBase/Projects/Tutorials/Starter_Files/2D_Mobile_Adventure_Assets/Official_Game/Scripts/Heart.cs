using UnityEngine;

namespace LemApperson_2D_Mobile_Adventure
{
    public class Heart : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                other.GetComponent<Player.Player>()?.AddALife();
                Destroy(gameObject);
            }
        }
    }
}