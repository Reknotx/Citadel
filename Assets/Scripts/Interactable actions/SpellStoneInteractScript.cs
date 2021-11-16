using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class SpellStoneInteractScript : Item
    {
        public bool given = false;


        public override void Interact()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerInventory>().spellStone = true;
            base.Interact();
        }


        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                given = true;
                NewPlayer.Instance.inventory.spellStone = true;
                Destroy(this.gameObject);
            }
        }
    }
}