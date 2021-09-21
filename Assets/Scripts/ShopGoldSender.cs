/* Author: Andrew Nave
 * Date: 9/03/2021
 *
 * Brief: This script manages the clicekr button in the mine,
 * adding a proportional amount of gold depending on the number of miners the players has purchased.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGoldSender : MonoBehaviour
{
    /// <summary>
    /// Reference to GoldHandler
    /// </summary>
    public GoldHandler gold;

    /// <summary>
    /// Sends the soft gold the player has to the "surface" by converting it into their hard gold.
    /// </summary>
    public void SendGoldToSurface()
    {
        gold.AddHardGold((int)gold.mySoftGold);
        gold.mySoftGold = 0;
    }
}
