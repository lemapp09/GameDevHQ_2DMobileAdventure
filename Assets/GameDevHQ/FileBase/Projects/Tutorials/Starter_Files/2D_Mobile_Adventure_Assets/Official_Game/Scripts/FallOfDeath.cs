using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LemApperson_2D_Mobile_Adventure
{
    public class FallOfDeath : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other) {
            SceneManager.LoadScene("MainMenu");
        }
        private void OnTriggerExit2D(Collider2D other) {
            SceneManager.LoadScene("MainMenu");
        }
    }
}