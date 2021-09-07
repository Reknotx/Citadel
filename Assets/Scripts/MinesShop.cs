/* Author: Andrew Nave
 * Date: 9/07/2021
 *
 * Brief: Tthis script allows the player to purchase miners and carts in the shop and increases the cost and flow of passive gold accordingly.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesShop : MonoBehaviour
{

    [SerializeField]
    private GoldHandler gold;

    [SerializeField]
    private int baseMinerCost;
    [SerializeField]
    private int minerCostIncrease;

    [SerializeField]
    private int baseCartCost;
    [SerializeField]
    private int cartCostIncrease;


    public void PurchaseMiner()
    {
        if( (int)gold.myHardGold >= baseMinerCost)
        {
            gold.numOfMiners += 1;
            gold.myHardGold -= baseMinerCost;
            baseMinerCost += (minerCostIncrease * gold.numOfMiners);
        }
    }

    public void purchaseCart()
    {
        if ((int)gold.myHardGold >= baseCartCost)
        {
            gold.numOfCarts += 1;
            gold.myHardGold -= baseCartCost;
            baseCartCost += (cartCostIncrease * gold.numOfCarts);
        }
    }
}
