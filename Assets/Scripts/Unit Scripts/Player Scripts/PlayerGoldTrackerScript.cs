using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGoldTrackerScript : MonoBehaviour
{

    public GameObject goldHandler;
    public GameObject player;


    private static PlayerGoldTrackerScript _instance;

    [SerializeField]
    public static PlayerGoldTrackerScript Instance { get { return _instance; } }

    public float playerSoftGold;
    public float playerHardGold;
    public float startingSoftGold = 100f;
    public float startingHardGold = 2000f;

    public int numCart;
    public int numMiner;
    public int startingNumCart;
    public int startingNumMiner = 2;
    public int numMoles;
    public int startingNumMolest;
    public int numPicks;
    public int startingNumPicks;
    public int numWizard;
    public int startingNumWizard;

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

    public float startingSpeed;
    public float startingMaxHealth;
    public float startingMaxMana;
    public int startingAttackDamage;
    public float startingAttackRange;



    public string attack1;
    public string attack2;
    public string attack3;


    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else if(_instance == null)
        _instance = this;

        

        DontDestroyOnLoad(_instance);

        findReference();
        /*player.GetComponent<Player>().Attack1 = attack1;
        player.GetComponent<Player>().Attack2 = attack2;
        player.GetComponent<Player>().Attack3 = attack3;
        Edited out by Tyler to remove NRE with new player
        */
    }



    // Update is called once per frame
    void FixedUpdate()
    {

        _instance = this;
        findReference();
        currentSceneName = SceneManager.GetActiveScene().name;
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }



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



        findReference();
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
                 //numMiner = startingNumMiner;
                playerDead = false;
            }

            goldHandler.GetComponent<GoldHandler>()._myHardGold = playerHardGold;
            goldHandler.GetComponent<GoldHandler>()._mySoftGold = playerSoftGold;
            goldHandler.GetComponent<GoldHandler>().numOfCarts = numCart;
            goldHandler.GetComponent<GoldHandler>().numOfMiners = numMiner;
            goldHandler.GetComponent<GoldHandler>().numOfMoles = numMoles;
            goldHandler.GetComponent<GoldHandler>().numOfPicks = numPicks;
            goldHandler.GetComponent<GoldHandler>().numOfWizards =numWizard;

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

            numMoles = goldHandler.GetComponent<GoldHandler>().numOfMoles;
            numPicks = goldHandler.GetComponent<GoldHandler>().numOfPicks;
            numWizard = goldHandler.GetComponent<GoldHandler>().numOfWizards;

        }

    }

    public void findReference()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        goldHandler = GameObject.FindGameObjectWithTag("PlayerGoldHandler");
        currentSceneName = SceneManager.GetActiveScene().name;
    }


    public void updatePlayerStats()
    {
        if (statsUpdated == false && currentSceneName != "MainMenuScene" && currentSceneName != "MineScene")
        {
            if (playerDead == true)
            {
                    playerSpeed =  startingSpeed;
                    playerMaxHealth =  startingMaxHealth;
                    playerMaxMana = startingMaxMana;
                    playerAttackDamage =startingAttackDamage ;
                    playerAttackRange =  startingAttackRange;
                    playerDead = false;
            }


            //player.GetComponent<NewPlayer>().MaxHealth = playerMaxHealth;
            player.GetComponent<Player>().maxMana = playerMaxMana;
            player.GetComponent<Player>().speed = playerSpeed;
            player.GetComponent<Player>().meleeAttackDamage = playerAttackDamage;
            player.GetComponent<Player>().meleeAttackRange = playerAttackRange;
            player.GetComponent<Player>().Attack1 = attack1;
            player.GetComponent<Player>().Attack2 = attack2;
            player.GetComponent<Player>().Attack3 = attack3;

            statsUpdated = true;
        }
    }

    public void trackStats()
    {
        if (currentSceneName != "MainMenuScene" && currentSceneName != "MineScene")
        {
            playerMaxHealth = player.GetComponent<NewPlayer>().MaxHealth;
            playerMaxMana = player.GetComponent<Player>().maxMana;
            playerSpeed = player.GetComponent<Player>().speed;
            playerAttackDamage = player.GetComponent<Player>().meleeAttackDamage;
            playerAttackRange = player.GetComponent<Player>().meleeAttackRange;
            attack1 = player.GetComponent<Player>().Attack1;
            attack2 = player.GetComponent<Player>().Attack2;
            attack3 = player.GetComponent<Player>().Attack3;
        }
    }
}
