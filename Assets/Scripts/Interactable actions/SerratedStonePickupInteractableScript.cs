using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace Interactables
{
    public class SerratedStonePickupInteractableScript : Item
    {
        public bool given = false;


        public override void Interact()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerInventory>().serratedStone = true;
            base.Interact();
        }


        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                given = true;
                NewPlayer.Instance.inventory.serratedStone = true;
                Destroy(this.gameObject);
            }
        }
    }
}
