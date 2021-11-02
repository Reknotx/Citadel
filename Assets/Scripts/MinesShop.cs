/* Author: Andrew Nave
 * Date: 9/07/2021
 *
 * Brief: Tthis script allows the player to purchase miners and carts in the shop and increases the cost and flow of passive gold accordingly.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinesShop : MonoBehaviour
{
    /// <summary>
    /// This is the object which has access to the gold functions, used to reference the GoldHandler members and functions for updating gold
    /// </summary>
    [SerializeField]    
    private GoldHandler gold;

    /// <summary>
    /// Base cost for the miner
    /// </summary>
    [SerializeField]
    private int baseMinerCost = 50;

    /// <summary>
    /// Amount of gold that the cost of the miners increases by each time the player purchases one
    /// </summary>
    [SerializeField]
    private int minerCostIncrease = 25;

    /// <summary>
    /// Base cost of the mine cart
    /// </summary>
    [SerializeField]
    private int baseCartCost = 150;

    /// <summary>
    /// Amount of gold that the cost of the mine carts increases by each time the player purchases one
    /// </summary>
    [SerializeField]
    private int cartCostIncrease = 50;

    public Text minecartCost;

    public Text minerCost;

    public void Update()
    {
        minecartCost.text = "Purchase Mine Cart: " + baseCartCost;
        minerCost.text = "Purchase Miner: " + baseMinerCost;
    }

    public void PurchaseMiner()
    {
        if( (int)gold.MyHardGold >= baseMinerCost)
        {
            gold.numOfMiners += 1;
            gold.MyHardGold -= baseMinerCost;
            baseMinerCost += (minerCostIncrease * gold.numOfMiners);
        }
    }

    /// <summary>
    /// Called when the player purchases a mine cart, increases the player's amount of mine carts, subtracts the cost of the cart from the player's gold, and increases the cost of the next cart by the base cost.
    /// </summary>
    public void PurchaseCart()
    {
        if ((int)gold.MyHardGold >= baseCartCost)
        {
            gold.numOfCarts += 1;
            gold.MyHardGold -= baseCartCost;
            baseCartCost += (cartCostIncrease * gold.numOfCarts);
        }
    }
}
