/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/9/2021
 * 
 * Brief:This scripts controls the behavior 
 * of the scene manager 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    #region Bool Determinates 
    /// <summary> this keeps track of if the player is in the camp shop or not  </summary>
    public bool inCampShop = false;

    /// <summary> this keeps track of if the player is in the mine shop or not  </summary>
    public bool inMineShop = false;

    /// <summary> this stores the ingame objects of the camp ui buttons  </summary>
    public GameObject campButtons;

    /// <summary> this stroes the ingame objects of the camp shop ui buttons  </summary>
    public GameObject campShopButtons;

    /// <summary> this stroes the ingame objects of the mine ui buttons  </summary>
    public GameObject mineButtons;

    /// <summary> this stroes the ingame objects of the mine shop ui buttons  </summary>
    public GameObject mineShopButtons;

    /// <summary> this tracks what the active scene name is  </summary>
    public Scene activeScene;
    public string activeSceneName;
    #endregion


    private void Update()
    {
        /// <summary> this tracks what the active scene name is  </summary>
        activeScene = SceneManager.GetActiveScene();
        activeSceneName = activeScene.name;

      
    }

    #region button functions 

    /// <summary> this takes the player to the camp scene</summary>
    public void goToCamp()
    {
        SceneManager.LoadScene(3);
    }

    /// <summary> this takes the player to the options scene </summary>
    public void goToOptions()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary> this takes the player to the credits scene </summary>
    public void goToCredits()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary> this takes the player to the camp shop scene </summary>
    public void goToCampShop()
    {
        inCampShop = true;
        campButtons.SetActive(false);
        campShopButtons.SetActive(true);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().canMove = false;
    } 
    
    /// <summary> this takes the player to the mine scene </summary>
    public void goToMine()
    {
        SceneManager.LoadScene(5);
    }

    /// <summary> this takes the player to the camp shop scene </summary>
    public void goToMineShop()
    {
        inMineShop = true;
        mineButtons.SetActive(false);
        mineShopButtons.SetActive(true);
    }

    /// <summary> this takes the player to the castle scene </summary>
    public void goToCastle()
    {
        SceneManager.LoadScene(6);
    }

    /// <summary> this takes the player back to the main menu scene </summary>
    public void backToMainmenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary> this takes the player back to the camp scene </summary>
    public void backToCamp()
    {
        if (inCampShop == true)
        {
            campButtons.SetActive(true);
            campShopButtons.SetActive(false);
            inCampShop = false;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Player>().canMove = true;
        }
        else
        {
            SceneManager.LoadScene(3);
        }
        
    }

    /// <summary> this takes the player back to the camp scene </summary>
    public void backToMine()
    {
        if (inMineShop == true)
        {
            mineButtons.SetActive(true);
            mineShopButtons.SetActive(false);
            inMineShop = false;
        }
       

    }

    /// <summary> this closes the game  </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
}
