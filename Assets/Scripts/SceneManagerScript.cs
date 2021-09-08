using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public GameObject target;



    public void Update()
    {
        
    }


    #region Main Menu scene navigation 
    /// <summary> sends user to the Camp scene </summary>
    public void MainMenuStartGame()
    {
        SceneManager.LoadScene(7);
    }

    /// <summary> sends user to the Options scene </summary>
    public void MainMenuOptions()
    {
       
        SceneManager.LoadScene(1);
    }
    /// <summary> sends user to the Credits scene </summary>
    public void MainMenuCredits()
    {
        
        SceneManager.LoadScene(2);
        
    }

    /// <summary> sends user back to the Main Menu scene </summary>
    public void MainMenuBack()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    /// <summary> exits the game (does not save) </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
    #region  pause menu scene navigation
    /// <summary> brings up the pause menu over the camp or mine scene(does not pause game time to allow mines to keep running)</summary>
    public void MineCampPause()
    {
        target = GameObject.FindGameObjectWithTag("CampSceneObjects");
        target.SetActive ( false);
        SceneManager.LoadScene(5,LoadSceneMode.Additive);
    }

    /// <summary> returns the player to the camp or mine by cosing the pause scene</summary>
    public void MineCampResume()
    {
        target = GameObject.FindGameObjectWithTag("CampSceneObjects");
        target.SetActive(true);
        SceneManager.UnloadSceneAsync(5);
        
    }

    /// <summary> brings up the pause menu in the castle scene( pauses game time to allow ease of player use)</summary>
    public void CastlePause()
    {
        Time.timeScale = 0f;
        AsyncOperation pauseMenuOpen =SceneManager.LoadSceneAsync(6);
    }

    /// <summary> returns the player to the castle by closing the pause scene(resumes game time)</summary>
    public void CastleResume()
    {
         SceneManager.UnloadSceneAsync(6);
        Time.timeScale = 1f;
    }

    /// <summary> brings up the options menu while paused in the castle  </summary>
    public void PauseOptions()
    {
       
        SceneManager.LoadSceneAsync(3);
    }

    /// <summary> brings up the credits menu while paused in the castle  </summary>
    public void PauseCredits()
    {
        
        SceneManager.LoadSceneAsync(4);
    }

    /// <summary> returns to the paused menu while paused in the castle  </summary>
    public void PauseOptionsBack()
    {
        SceneManager.UnloadSceneAsync(3);
    }

    /// <summary> returns to the paused menu while paused in the castle   </summary>
    public void PauseCreditsBack()
    {
        SceneManager.UnloadSceneAsync(4);
    }
    #endregion

    #region Gameplay Scene Navigation
    /// <summary>sends player to the mine scene </summary>
    public void Mine()
    {
        SceneManager.LoadScene(9);
    }
    /// <summary> closes the shop scene </summary>
    public void leaveMine()
    {
        SceneManager.LoadScene(7);
    }

    /// <summary> brings up the shop scene </summary>
    public void shop()
    {
        SceneManager.LoadScene(8);
    }

    /// <summary> closes the shop scene </summary>
    public void leaveShop()
    {
        SceneManager.LoadScene(7);
    }

    /// <summary>sends player to the castle scene </summary>
    public void Castle()
    {
        SceneManager.LoadScene(10);
    }



    #endregion
}
