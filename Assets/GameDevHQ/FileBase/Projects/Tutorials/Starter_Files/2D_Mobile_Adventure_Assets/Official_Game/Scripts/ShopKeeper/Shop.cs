using LemApperson_2D_Mobile_Adventure.Managers;
using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.UI;

namespace LemApperson_2D_Mobile_Adventure.ShopKeeper
{
    public class Shop : MonoBehaviour
    {
        private int _currentlySelectedItem = 0;
        private Player.Player _player;
        
        public void SelectItem(int SelectedItem) {
            UIManager.Instance.ResetSelections();
            UIManager.Instance.SetSelection(SelectedItem);
            _currentlySelectedItem = SelectedItem;
        }
        
        public void BuyItem() {
            if (_player != null) {
                switch (_currentlySelectedItem) {
                    case 0: //  0: Flame Sword, 200
                        if (_player.HowManyGems() >= 200) {
                            UpdateGems(200);
                            // Update Inventory
                        }
                        break;
                    case 1: //  1: Boots of Flight, 400
                        if (_player.HowManyGems() >= 400) {
                            UpdateGems(400);
                            _player.IncreaseJumpForce();
                        }
                        break;
                    case 2: //   2: Keys to Castle, 100
                        if (_player.HowManyGems() >= 100) {
                            UpdateGems(100);
                            if( GameManager.Instance != null){
                                GameManager.Instance.HasKeyToCastle = true;
                            }
                        }
                        break;
                }
            }
        }

        private void UpdateGems(int GemCount) {
            if (_player != null) {
                _player.DeductGems(GemCount);
                UIManager.Instance.UpDateGemCount(_player.HowManyGems());
                AudioManager.Instance.SFX(3);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player") ) {
                _player = other.GetComponent<Player.Player>();
                if (_player != null) {
                    UIManager.Instance.OpenShop(_player.HowManyGems());
                    AudioManager.Instance.Ambient(1);
                    _player.EntersShop();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                if(UIManager.Instance != null){
                    UIManager.Instance.CloseShop();
                }
                if(AudioManager.Instance != null){
                    AudioManager.Instance.Ambient(0);
                }
                if(_player != null){
                    _player.ExitsShop();
                }
            }
        }

    }
}