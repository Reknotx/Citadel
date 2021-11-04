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
    static private int baseMinerCost = 50;

    /// <summary>
    /// Amount of gold that the cost of the miners increases by each time the player purchases one
    /// </summary>
    [SerializeField]
    private int minerCostIncrease = 25;

    /// <summary>
    /// Base cost of the mine cart
    /// </summary>
    [SerializeField]
    static private int baseCartCost = 1200;

    /// <summary>
    /// Amount of gold that the cost of the mine carts increases by each time the player purchases one
    /// </summary>
    [SerializeField]
    private int cartCostIncrease = 400;

    /// <summary>
    /// Base cost of the Magic pick
    /// </summary>
    [SerializeField]
    static private int basePickCost = 250;

    /// <summary>
    /// Amount of gold that the cost of the pick increases by each time the player purchases one
    /// </summary>
    [SerializeField]
    private int pickCostIncrease = 120;
    
    /// <summary>
    /// Base cost of the Giant Mole pick
    /// </summary>
    [SerializeField]
    static private int baseMoleCost = 5000;

    /// <summary>
    /// Amount of gold that the cost of the Giant Mole increases by each time the player purchases one
    /// </summary>
    [SerializeField]
    private int moleCostIncrease = 1250;
    
    /// <summary>
    /// Base cost of the Spelunking Wizard
    /// </summary>
    [SerializeField]
    static private int baseWizardCost = 10000;

    /// <summary>
    /// Amount of gold that the cost of the Spelunking Wizard increases by each time the player purchases one
    /// </summary>
    [SerializeField]
    private int wizardCostIncrease = 3000;

    public Text minecartCost;

    public Text minerCost;

    public Text pickCost;

    public Text moleCost;

    public Text wizardCost;

    private static MinesShop instance;

    public void Awake()
    {
        /*DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }*/
    }

    public void Update()
    {
        minecartCost.text = "Purchase Mine Cart: " + baseCartCost + " | Miner Carts Owned: " + gold.numOfCarts;
        minerCost.text = "Purchase Miner: " + baseMinerCost + " | Miners Owned: " + gold.numOfMiners;
        pickCost.text = "Purchase Magical Pickaxe: " + basePickCost + " | Pickaxes Owned: " + gold.numOfPicks;
        moleCost.text = "Purchase Giant Mole: " + baseMoleCost + " | Moles Owned: " + gold.numOfMoles;
        wizardCost.text = "Purchase Spelunking Wizard: " + baseWizardCost + " | Wizards Owned: " + gold.numOfWizards;
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
