using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Interactables
{
    public class CompassPickupInteractScript : Item
    {
        public bool given = false;


        public override void Interact()
        {
            if(grounded)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInventory>().compass = true;
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
                    NewPlayer.Instance.inventory.compass = true;
                    Destroy(this.gameObject);
                }
               
            }
        }
    }
}
