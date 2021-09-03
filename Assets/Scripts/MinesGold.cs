/*
 Author: Andrew Nave
 * Date: 9/03/2021
 * 
 * Brief: This script manages the clicekr button in the mine, 
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

    public void GoldPerClick()
    {
        gold.AddHardGold(numOfMiners * baseGoldGain);
    }
}
