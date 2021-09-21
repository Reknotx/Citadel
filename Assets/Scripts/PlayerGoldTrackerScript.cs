using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGoldTrackerScript : MonoBehaviour
{

    public GameObject goldHandler;



    private static PlayerGoldTrackerScript _instance;

    public static PlayerGoldTrackerScript Instance { get { return _instance; } }

    public float playerSoftGold;
    public float playerHardGold;

    public int numCart;
    public int numMiner;


    public bool updated = false;
    public bool sceneChanged = false;

    public string currentSceneName;
    public string lastSceneName;


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
        DontDestroyOnLoad(this.gameObject);
    }



    // Update is called once per frame
    void Update()
    {
      
       goldHandler = GameObject.FindGameObjectWithTag("PlayerGoldHandler");
        currentSceneName = SceneManager.GetActiveScene().name;

        if(currentSceneName!=lastSceneName)
        {

            sceneChanged = true;
        }


        if(sceneChanged == true)
        {
            updated = false;
            updateGold();
            lastSceneName = currentSceneName;
            sceneChanged = false;
        }
        

        trackGold();

    }


    public void updateGold()
    {
        if (updated == false)
        {
            goldHandler.GetComponent<GoldHandler>().myHardGold = playerHardGold;
            goldHandler.GetComponent<GoldHandler>().mySoftGold = playerSoftGold;
            goldHandler.GetComponent<GoldHandler>().numOfCarts = numCart;
            goldHandler.GetComponent<GoldHandler>().numOfMiners = numMiner;

            updated = true;
        }
        
    }

    public void trackGold()
    {
        playerHardGold = goldHandler.GetComponent<GoldHandler>().myHardGold;
        playerSoftGold = goldHandler.GetComponent<GoldHandler>().mySoftGold;
        numCart = goldHandler.GetComponent<GoldHandler>().numOfCarts;
        numMiner = goldHandler.GetComponent<GoldHandler>().numOfMiners;
    }
}
