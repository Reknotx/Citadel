using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{

    public class ManaPickupInteractableScript : Interactable
    {
        public int dropAmount = 1;

        private bool given = false;

        private NewPlayer player;
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>();
        }


        // Update is called once per frame
        void Update()
        {
            dropAmount = player.MaxMana /20;
        }

        public override void Interact()
        {
            
            if (player.Mana != player.MaxMana)
            {
                player.Mana += dropAmount;
                Destroy(this.gameObject);
            }
                
                


        }


        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                Interact();
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                Interact();
            }
        }


    }
}
