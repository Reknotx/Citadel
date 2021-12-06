using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class UndyingPickupInteractScript : Item
    {
        public bool given = false;

        public GameObject undyingGameObject;

        public override void Interact()
        {
            if(grounded)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInventory>().undying = true;
                player.GetComponent<Inventory_UI>().AddItem(undyingGameObject);
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
                    NewPlayer.Instance.inventory.undying = true;
                    Destroy(this.gameObject);
                }
                
            }
        }
    }
}