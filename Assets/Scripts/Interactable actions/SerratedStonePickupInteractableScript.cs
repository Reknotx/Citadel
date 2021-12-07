using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Interactables
{
    public class SerratedStonePickupInteractableScript : Item
    {
        public bool given = false;

        public GameObject serratedStoneGameObject;

        public override void Interact()
        {
            if(grounded)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInventory>().serratedStone = true;
                base.Interact();
            }
            
        }


        public void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                if(grounded)
                {
                    given = true;
                    NewPlayer.Instance.inventory.serratedStone = true;
                    NewPlayer.Instance.GetComponent<Inventory_UI>().AddItem(serratedStoneGameObject);
                    Destroy(this.gameObject);
                }
               
            }
        }
    }
}
