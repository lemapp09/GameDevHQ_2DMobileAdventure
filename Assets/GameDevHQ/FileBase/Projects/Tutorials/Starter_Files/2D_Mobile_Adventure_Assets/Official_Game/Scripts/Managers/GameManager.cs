
using UnityEngine;

namespace LemApperson_2D_Mobile_Adventure.Managers
{

    public class GameManager : MonoSingleton<GameManager>
    {
        public bool HasKeyToCastle { get; set; }
        [SerializeField] private Player.Player _player;

        public void RewardGems(int numberOfGems) {
            _player.CollectGems(numberOfGems);
        }
    }
}