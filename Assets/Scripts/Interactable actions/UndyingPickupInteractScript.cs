using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class UndyingPickupInteractScript : Item
    {
        public bool given = false;
        

        public override void Interact()
        {
            if(grounded)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInventory>().undying = true;
                base.Interact();
            }
           
        }


        public void OnTriggerEnter(Collider other)
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