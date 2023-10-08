using UnityEngine;
using UnityEngine.SceneManagement;

namespace LemApperson_2D_Mobile_Adventure
{
    public class MainMenu : MonoBehaviour
    {
        public void LoadGame() {
          SceneManager.LoadScene("Game");
        }

        public void Quit() {
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}