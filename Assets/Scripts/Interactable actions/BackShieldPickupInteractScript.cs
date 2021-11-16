using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class BackShieldPickupInteractScript : Item
    {

        public bool given = false;

        public override void Interact()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerInventory>().floatingShield = true;
            base.Interact();
        }


        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                given = true;
                other.GetComponent<PlayerInventory>().floatingShield = true;
                Destroy(this.gameObject);
            }
        }
    }
}
