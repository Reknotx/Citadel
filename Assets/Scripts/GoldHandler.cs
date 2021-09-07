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
    public Text softGoldText;
    public Text hardGoldText;

    public float mySoftGold;
    public float myHardGold;

    public GameObject softGold;
    public GameObject hardGold;

    private Scene currentScene;
    public string castleSceneName;
    public string mainMenuName;
    
    public int numOfMiners;
    public int baseGoldIncrease;
    public int numOfCarts;

    [SerializeField]
    private int minerIncrease;
    [SerializeField]
    private int cartIncrease;

    private float elapsed = 0f;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        softGoldText.text = "Soft Gold: " + (int)mySoftGold;

        hardGoldText.text = "Hard Gold: " + (int)myHardGold;

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

        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            if (!(mainMenuName == currentScene.name))
            {
                AddHardGold(baseGoldIncrease + (numOfMiners * minerIncrease) + (numOfCarts * cartIncrease));
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
