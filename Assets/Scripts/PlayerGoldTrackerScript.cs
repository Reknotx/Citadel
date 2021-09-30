using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGoldTrackerScript : MonoBehaviour
{

    public GameObject goldHandler;
    public GameObject player;


    private static PlayerGoldTrackerScript _instance;

    public static PlayerGoldTrackerScript Instance { get { return _instance; } }

    public float playerSoftGold;
    public float playerHardGold;
    public float startingSoftGold = 100f;
    public float startingHardGold = 2000f;

    public int numCart;
    public int numMiner;
    public int startingNumCart;
    public int startingNumMiner = 2;

    public bool goldUpdated = false;
    public bool statsUpdated = false;
    public bool sceneChanged = false;
    public bool playerDead = false;

    public string currentSceneName;
    public string lastSceneName;


    public float playerSpeed;
    public float playerMaxHealth;
    public float playerMaxMana;
    public int playerAttackDamage;
    public float playerAttackRange;

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = this;

        DontDestroyOnLoad(this.gameObject);
    }



    // Update is called once per frame
    void Update()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        goldHandler = GameObject.FindGameObjectWithTag("PlayerGoldHandler");
        currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName != lastSceneName)
        {

            sceneChanged = true;
        }


        if (sceneChanged == true)
        {
            goldUpdated = false;
            statsUpdated = false;
            updateGold();
            updatePlayerStats();
            lastSceneName = currentSceneName;
            sceneChanged = false;
        }




        trackGold();
        trackStats();


    }


    public void updateGold()
    {

        if (goldUpdated == false && currentSceneName != "MainMenuScene")
        {
            if (playerDead == true)
            {
                //playerHardGold = startingHardGold;
                playerSoftGold = startingSoftGold;
                //numCart = startingNumCart;
                // numMiner = startingNumMiner;
                playerDead = false;
            }

            goldHandler.GetComponent<GoldHandler>()._myHardGold = playerHardGold;
            goldHandler.GetComponent<GoldHandler>()._mySoftGold = playerSoftGold;
            goldHandler.GetComponent<GoldHandler>().numOfCarts = numCart;
            goldHandler.GetComponent<GoldHandler>().numOfMiners = numMiner;

            goldUpdated = true;
        }

    }

    public void trackGold()
    {
        if (currentSceneName != "MainMenuScene")
        {
            playerHardGold = goldHandler.GetComponent<GoldHandler>().MyHardGold;
            playerSoftGold = goldHandler.GetComponent<GoldHandler>().MySoftGold;
            numCart = goldHandler.GetComponent<GoldHandler>().numOfCarts;
            numMiner = goldHandler.GetComponent<GoldHandler>().numOfMiners;
        }

    }


    public void updatePlayerStats()
    {
        if (statsUpdated == false && currentSceneName != "MainMenuScene" && currentSceneName != "MineScene")
        {

            player.GetComponent<Player>().maxHealth = playerMaxHealth;
            player.GetComponent<Player>().maxMana = playerMaxMana;
            player.GetComponent<Player>().speed = playerSpeed;
            player.GetComponent<Player>().meleeAttackDamage = playerAttackDamage;
            player.GetComponent<Player>().meleeAttackRange = playerAttackRange;

            statsUpdated = true;
        }
    }

    public void trackStats()
    {
        if (currentSceneName != "MainMenuScene" && currentSceneName != "MineScene")
        {
            playerMaxHealth = player.GetComponent<Player>().maxHealth;
            playerMaxMana = player.GetComponent<Player>().maxMana;
            playerSpeed = player.GetComponent<Player>().speed;
            playerAttackDamage = player.GetComponent<Player>().meleeAttackDamage;
            playerAttackRange = player.GetComponent<Player>().meleeAttackRange;
        }
    }
}
