using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay
{
    private List<ItemAssets> itemList;

    public InventoryDisplay()
    {
        itemList = new List<ItemAssets>();
    }

    public void AddItem(ItemAssets item)
    {
        itemList.Add(item);
    }

    public void RemoveItem(ItemAssets item)
    {
        itemList.Remove(item);
    }

    public List<ItemAssets> GetItemList()
    {
        return itemList;
    }
}
