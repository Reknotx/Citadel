using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryController : MonoBehaviour
{
    public GameObject player;
    private InventoryDisplay ID;

    public PlayerInventory PI;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<PlayerInventory>().shuues == true)
        {
            ID.AddItem(new ItemAssets { itemType = ItemAssets.ItemType.shuues, amount = 1 });
        }
        else
        {
            //remove
        }

        if (player.GetComponent<PlayerInventory>().undying == true)
        {
            ID.AddItem(new ItemAssets { itemType = ItemAssets.ItemType.undying, amount = 1 });
        }
        else
        {
            //remove
        }

        if (player.GetComponent<PlayerInventory>().floatingShield == true)
        {
            ID.AddItem(new ItemAssets { itemType = ItemAssets.ItemType.floatingShield, amount = 1 });
        }
        else
        {
            //remove
        }

        if (player.GetComponent<PlayerInventory>().medicineStash == true)
        {
            ID.AddItem(new ItemAssets { itemType = ItemAssets.ItemType.medicineStash, amount = 1 });
        }
        else
        {
            //remove
        }

        if (player.GetComponent<PlayerInventory>().compass == true)
        {
            ID.AddItem(new ItemAssets { itemType = ItemAssets.ItemType.compass, amount = 1 });
        }
        else
        {
            //remove
        }

        if (player.GetComponent<PlayerInventory>().serratedStone == true)
        {
            ID.AddItem(new ItemAssets { itemType = ItemAssets.ItemType.serratedStone, amount = 1 });
        }
        else
        {
            //remove
        }

        if (player.GetComponent<PlayerInventory>().spellStone == true)
        {
            ID.AddItem(new ItemAssets { itemType = ItemAssets.ItemType.spellStone, amount = 1 });
        }
        else
        {
            //remove
        }
    }
}
