using UnityEngine;

namespace LemApperson_2D_Mobile_Adventure
{
    public class Diamond : MonoBehaviour
    {
        [SerializeField] private int _numberOfDiamonds = 1;

        public void SetNumberOfDiamonds(int NumberOfDiamonds) {
            _numberOfDiamonds = NumberOfDiamonds;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                other.GetComponent<Player.Player>()?.CollectGems(_numberOfDiamonds);
                Destroy(gameObject);
            }
        }
    }
}