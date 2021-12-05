using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    public GameObject player;

    private void Awake()
    {
        Instance = this;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
    }

    public Sprite healthPotSprite;
    public Sprite manaPotSprite;
    public Sprite shuuesSprite;
    public Sprite undyingSprite;
    public Sprite floatingShieldSprite;
    public Sprite medicineStashSprite;
    public Sprite compassSprite;
    public Sprite serratedStoneSprite;
    public Sprite spellStoneSprite;
    public Sprite defaultSprite;

    

    public enum ItemType
    {
        shuues,
        undying,
        spellStone,
        floatingShield,
        medicineStash,
        compass,
        serratedStone,
        manaPot,
        healthPot,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
            

        if(player.GetComponent<PlayerInventory>().shuues == true)
        {
            return ItemAssets.Instance.shuuesSprite;
        }

        if (player.GetComponent<PlayerInventory>().undying == true)
        {
            return ItemAssets.Instance.undyingSprite;
        }

        if (player.GetComponent<PlayerInventory>().spellStone == true)
        {
            return ItemAssets.Instance.spellStoneSprite;
        }

        if (player.GetComponent<PlayerInventory>().floatingShield == true)
        {
            return ItemAssets.Instance.floatingShieldSprite;
        }

        if (player.GetComponent<PlayerInventory>().medicineStash == true)
        {
            return ItemAssets.Instance.medicineStashSprite;
        }

        if (player.GetComponent<PlayerInventory>().compass == true)
        {
            return ItemAssets.Instance.compassSprite;
        }

        if (player.GetComponent<PlayerInventory>().serratedStone == true)
        {
            return ItemAssets.Instance.serratedStoneSprite;
        }

        return defaultSprite;


        /*switch (itemType)
        {
            default:
            case ItemType.shuues: return ItemAssets.Instance.shuuesSprite;
            case ItemType.undying: return ItemAssets.Instance.undyingSprite;
            case ItemType.spellStone: return ItemAssets.Instance.spellStoneSprite;
            case ItemType.floatingShield: return ItemAssets.Instance.floatingShieldSprite;
            case ItemType.medicineStash: return ItemAssets.Instance.medicineStashSprite;
            case ItemType.compass: return ItemAssets.Instance.compassSprite;
            case ItemType.serratedStone: return ItemAssets.Instance.serratedStoneSprite;
            case ItemType.manaPot: return ItemAssets.Instance.manaPotSprite;
            case ItemType.healthPot: return ItemAssets.Instance.healthPotSprite;

        }*/
    }
}
