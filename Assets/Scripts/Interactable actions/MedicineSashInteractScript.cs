using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Interactables
{
    public class MedicineSashInteractScript : Item
    {
        public override void Interact()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Player>().medicineStash = true;
            base.Interact();
        }
    }
}
