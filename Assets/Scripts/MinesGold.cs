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
    /// <summary>
    /// number of miners the player currently owns
    /// </summary>
    public int numOfMiners;

    /// <summary>
    /// The base amount of gold the player will gain with each click
    /// </summary>
    public int baseGoldGain;

    /// <summary>
    /// The percentage of the players passive gold gain applied to each click
    /// </summary>
    public float clickPercentage;

    /// <summary>
    /// Reference to GoldHandler
    /// </summary>
    public GoldHandler gold;

    /// <summary>
    /// Gives the player a percentage of their passive gold gain when they click int he mines.
    /// </summary>
    public void GoldPerClick()
    {
        gold.AddHardGold((int)(gold.revenue * clickPercentage));
    }
}
