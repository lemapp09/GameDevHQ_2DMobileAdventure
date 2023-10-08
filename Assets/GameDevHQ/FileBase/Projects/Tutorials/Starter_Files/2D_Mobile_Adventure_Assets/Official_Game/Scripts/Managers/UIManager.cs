using UnityEngine;
using TMPro;

namespace LemApperson_2D_Mobile_Adventure.Managers
{

    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private TMP_Text _playerGemCountText;
        [SerializeField] private TMP_Text _gemCount;
        [SerializeField] private GameObject[] _selectionImages, _lifeBars;
        [SerializeField] private  GameObject _shopPanel;
        
        public void OpenShop(int GemCount) {
            UpDateGemCount(GemCount);
            if (_shopPanel != null) {
                _shopPanel.SetActive(true);
            }
            ResetSelections();
        }

        public void UpDateGemCount(int GemCount) {
            _playerGemCountText.text = GemCount + " G";
            _gemCount.text = GemCount + " G";
        }

        public void CloseShop() {
            if (_shopPanel != null) {
                _shopPanel.SetActive(false);
            }
        }
        
        public void ResetSelections() {
            for (int i = 0; i < _selectionImages.Length; i++) {
                _selectionImages[i].SetActive(false);
            }
        }

        public void SetSelection(int SelectedItem) {
            _selectionImages[SelectedItem].SetActive(true);
            AudioManager.Instance.SFX(2);
        }

        public void UpDateHealthCount(int health) {
            _lifeBars[health].SetActive(false);
        }
    }
}