using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteract : Interactable
{
    public GameObject shopUI;

	public override void Interact()
    {
        ///Turn on the shop
        shopUI.SetActive(true);
    }
}
