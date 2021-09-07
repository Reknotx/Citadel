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

    public GoldHandler gold;

    /// <summary>
    /// Multiplies the gold earned for clicking int he mine by the amount of miners the player has purchased.
    /// </summary>
    public void GoldPerClick()
    {
        gold.AddHardGold(numOfMiners * baseGoldGain);
    }
}
