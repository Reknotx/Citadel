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
    private int baseCartCost = 5000;

    [SerializeField]
    private int basePickCost = 500;

    [SerializeField]
    private int baseMoleCost = 20000;

    [SerializeField]
    private int baseWizardCost = 100000;

    /// <summary>
    /// Amount of gold that the cost of the mine carts increases by each time the player purchases one
    /// </summary>
    [SerializeField]
    private int cartCostIncrease = 50;

    [SerializeField]
    private int pickCostIncrease = 50;

    [SerializeField]
    private int moleCostIncrease = 50;

    [SerializeField]
    private int wizardCostIncrease = 50;

    public Text minecartCost;

    public Text minerCost;

    public Text pickCost;

    public Text moleCost;

    public Text wizardCost;

    public void Update()
    {
        minecartCost.text = "Purchase Mine Cart: " + baseCartCost;
        minerCost.text = "Purchase Miner: " + baseMinerCost;
        minerCost.text = "Purchase Magical Pick: " + basePickCost;
        minerCost.text = "Purchase Giant Mole: " + baseMoleCost;
        minerCost.text = "Purchase Spelunking Wizard: " + baseWizardCost;
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

    public void PurchasePick()
    {
        if ((int)gold.MyHardGold >= basePickCost)
        {
            gold.numOfPicks += 1;
            gold.MyHardGold -= basePickCost;
            basePickCost += (pickCostIncrease * gold.numOfPicks);
        }
    }

    public void PurchaseMole()
    {
        if ((int)gold.MyHardGold >= baseMoleCost)
        {
            gold.numOfMoles += 1;
            gold.MyHardGold -= baseMoleCost;
            baseMoleCost += (moleCostIncrease * gold.numOfMoles);
        }
    }

    public void PurchaseWizard()
    {
        if ((int)gold.MyHardGold >= baseWizardCost)
        {
            gold.numOfWizards += 1;
            gold.MyHardGold -= baseWizardCost;
            baseWizardCost += (wizardCostIncrease * gold.numOfWizards);
        }
    }
}
