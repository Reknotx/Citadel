using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Menu
{ 
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu Instance;

        public Button returnToCampButton;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(Instance);

            Instance = this;

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Time.timeScale = 0f;

            returnToCampButton.interactable = SceneManager.GetActiveScene().name == "CampScene";
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
            playerUnlock();
        }

        public void ReturnToCamp()
        {
            SceneManager.LoadScene("CampScene");
        }

        public void QuitGame()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }

        public void playerUnlock()
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.pauseActive = false;
        }
    }
}