/*
 Author: Andrew Nave
 * Date: 9/03/2021
 * 
 * Brief: This script manages the clicker button in the mine, 
 * adding a proportional amount of gold depending on the number of miners the players has purchased.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesGold : MonoBehaviour
{
    public int numOfMiners;
    public int baseGoldGain;
    public float clickPercentage;

    public GoldHandler gold;

    /// <summary>
    /// Gives the player a percentage of their passive gold gain when they click int he mines.
    /// </summary>
    public void GoldPerClick()
    {
        gold.AddHardGold((int)(gold.revenue * clickPercentage));
    }
}
