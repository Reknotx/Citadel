/*
 * Author: Andrew Nave
 * Date: 9/02/2021
 * 
 * Brief: This script manages the values for the player's soft and hard gold amount and 
 * updates the text accordingly
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoldHandler : MonoBehaviour
{
    /// <summary>
    /// Text displayed for player's soft gold
    /// </summary>
    public Text softGoldText;

    /// <summary>
    /// text displayed for players hard gold
    /// </summary>
    public Text hardGoldText;

    /// <summary>
    /// player's current soft gold
    /// </summary>
    public float mySoftGold;

    /// <summary>
    /// player's current hard gold
    /// </summary>
    public float myHardGold;

    /// <summary>
    /// GameObject that holds the UI element for soft gold
    /// </summary>
    public GameObject softGold;

    /// <summary>
    /// GameObject that holds the UI element for hard gold
    /// </summary>
    public GameObject hardGold;

    /// <summary>
    /// Reference to the current scene the player resides in
    /// </summary>
    private Scene currentScene;

    /// <summary>
    /// name of the castle scene
    /// </summary>
    public string castleSceneName;

    /// <summary>
    /// name of the main menu scene
    /// </summary>
    public string mainMenuName;
    
    /// <summary>
    /// number of miners the player currently owns
    /// </summary>
    public int numOfMiners;

    /// <summary>
    /// The base increase the player gets passively
    /// </summary>
    public int baseGoldIncrease;

    /// <summary>
    /// Number of minecarts the player currently owns
    /// </summary>
    public int numOfCarts;

    /// <summary>
    /// The increase in the player's revenue for every miner they own
    /// </summary>
    [SerializeField]
    private int minerIncrease;

    /// <summary>
    /// The increase in the player's revenue for every mine cart they own
    /// </summary>
    [SerializeField]
    private int cartIncrease;

    /// <summary>
    /// The player's current revenue (passively gained gold)
    /// </summary>
    public int revenue;

    /// <summary>
    /// Variable to keep track of time passed for use with player's passive gold gain
    /// </summary>
    private float elapsed = 0f;

    /// <summary>
    /// Start is called when the scene loads in
    /// </summary>
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    /// <summary>
    /// Update is called every frame
    /// </summary>
    void Update()
    {
        ///Updates soft and hard gold text object
        softGoldText.text = "Soft Gold: " + (int)mySoftGold;

        hardGoldText.text = "Hard Gold: " + (int)myHardGold;

        ///Checks which scene is active and displays the correct gold value
        if ( castleSceneName == currentScene.name )
        {
            hardGold.SetActive(false);
            softGold.SetActive(true);
        }
        else
        {
            softGold.SetActive(false);
            hardGold.SetActive(true);
        }

        ///Keeps track of time elapsed and updates the players passive gold according to the interval. Gives passive gold based off the player's revenue
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            if (!(mainMenuName == currentScene.name))
            {
                revenue = baseGoldIncrease + (numOfMiners * minerIncrease) + (numOfCarts * cartIncrease);
                AddHardGold(revenue);
            }
            elapsed = 0f;
        }

    }

    /// <param name="softGold">Increase amount of soft gold by given integer</param>
    public void AddSoftGold(int softGold)
    {
        mySoftGold += softGold;
    }

    /// <param name="hardGold">Increase amount of hard gold by given integer</param>
    public void AddHardGold(int hardGold)
    {
        myHardGold += hardGold;
    }
}
