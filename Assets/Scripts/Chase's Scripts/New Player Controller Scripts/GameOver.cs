using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;

    public void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;

        gameObject.SetActive(false);
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
}
