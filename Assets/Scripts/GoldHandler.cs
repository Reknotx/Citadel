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

public class GoldHandler : MonoBehaviour
{

    public Text softGoldText;
    public Text hardGoldText;

    public float mySoftGold;
    public float myHardGold;

    void Update()
    {
        softGoldText.text = "" + (int)mySoftGold;

        hardGoldText.text = "" + (int)myHardGold;
    }

    public void AddSoftGold(int softGold)
    {
        mySoftGold += softGold;
    }

    public void AddHardGold(int hardGold)
    {
        myHardGold += hardGold;
    }
}
