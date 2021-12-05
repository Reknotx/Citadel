using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;
    private GameObject[] tracker;
    //public GameObject itemIMG;

    public void AddItem(GameObject itemIMG)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(isFull[i] == false)
            {
                //add item
                isFull[i] = true;
                tracker[i] = itemIMG;
                Instantiate(itemIMG, slots[i].transform, false);
                break;
            }
        }
    }

    public void RemoveItem(GameObject itemIMG /*item to remove*/ )
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(isFull[i] == true)
            {
                if(itemIMG.tag == tracker[i].tag)
                {
                    isFull[i] = false;
                    foreach (Transform child in transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                }
            }
        }
    }
}
