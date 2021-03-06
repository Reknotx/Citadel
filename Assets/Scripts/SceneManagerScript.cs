/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/9/2021
 * 
 * Brief:This scripts controls the behavior 
 * of the scene manager 
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    #region Bool Determinates 


    /// <summary> this stores the ingame objects of the camp ui buttons  </summary>
    public GameObject campButtons;
    public GameObject campShopEntranceButton;

    /// <summary> this stroes the ingame objects of the camp shop ui buttons  </summary>
    public GameObject campShopButtons;

    /// <summary> this stroes the ingame objects of the mine ui buttons  </summary>
    public GameObject mineButtons;

    /// <summary> this stroes the ingame objects of the mine shop ui buttons  </summary>
    public GameObject mineShopButtons;

    /// <summary> this tracks what the active scene name is  </summary>
    public Scene activeScene;
    public string activeSceneName;

    ///<summary> Makes it so camp shop tutorial only appears when the player opens the camp shop</summary>
    public CampShopTutorial CST;
    #endregion

    

    GameObject player;
   public GameObject shopUI;

    //Tyler Code here
    public GameObject NextScrenH2P;
    //End Tyler Code

    private GameObject toMineShopBTN;

    /* will make this a singleton
    private static SceneManagerScript _instance;

    public static SceneManagerScript Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    */

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");

    }

    private void Update()
    {
        /// <summary> this tracks what the active scene name is  </summary>
        activeScene = SceneManager.GetActiveScene();
        activeSceneName = activeScene.name;
        GameObject player = GameObject.FindWithTag("Player");
        //  shopUI = GameObject.FindGameObjectWithTag("CampShop");

        sceneChecker();

    }

    public void sceneChecker()
    {
        if(activeScene.name == "MineScene")
        {
            shopUI = null;
        }

        if(activeScene.name == "CampScene")
        {
            mineButtons = null;
            mineShopButtons = null;

        }
    }

    #region button functions 

    //Tyler Code here
    public void HowToPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void ButtonControl()
    {
        SceneManager.LoadScene(8);
    }
    //End Tyler Code

    /// <summary> this takes the player to the camp scene</summary>
    public void goToCamp()
    {
        SceneManager.LoadScene(4);
    }

    /// <summary> this takes the player to the options scene </summary>
    public void goToOptions()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary> this takes the player to the credits scene </summary>
    public void goToCredits()
    {
        SceneManager.LoadScene(3);
    }

    /// <summary> this takes the player to the camp shop scene </summary>
    public void goToCampShop()
    {

        // CST.openCampShop = true;


        campShopEntranceButton.SetActive(false);
        campShopButtons.SetActive(true);   
      
        shopUI.SetActive(true);


       
    } 
    
    /// <summary> this takes the player to the mine scene </summary>
    public void goToMine()
    {
        
         SceneManager.LoadScene(6);
    }

    /// <summary> this takes the player to the camp shop scene </summary>
    public void goToMineShop()
    {
        //player.GetComponent<Player>().inMineShop = true;
        mineButtons.SetActive(false);
        mineShopButtons.SetActive(true);
    }

    /// <summary> this takes the player to the castle scene </summary>
    public void goToCastle()
    {
        SceneManager.LoadScene(7);
    }

    /// <summary> this takes the player back to the main menu scene </summary>
    public void backToMainmenu()
    {
        SceneManager.LoadScene(0);
    }

   

    /// <summary> this takes the player back to the camp scene </summary>
    public void backToMine()
    {
        
            mineButtons.SetActive(true);
            mineShopButtons.SetActive(false);
           // player.GetComponent<Player>().inMineShop = false;
        
       

    }

    /// <summary> this closes the game  </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void toBoss()
    {
        SceneManager.LoadScene(9);
    }

  
    
    #endregion
}
