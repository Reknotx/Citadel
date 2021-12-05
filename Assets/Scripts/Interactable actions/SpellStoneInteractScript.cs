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
            if(grounded)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInventory>().spellStone = true;
                base.Interact();
            }
            
        }


        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                if(grounded)
                {
                    given = true;
                    NewPlayer.Instance.inventory.spellStone = true;
                    Destroy(this.gameObject);
                }
               
            }
        }
    }
}