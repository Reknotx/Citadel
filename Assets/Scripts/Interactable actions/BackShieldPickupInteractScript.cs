using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class BackShieldPickupInteractScript : Item
    {
        public override void Interact()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Player>().floatingShield = true;
            base.Interact();
        }
    }
}
