using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{

    public class ManaPickupInteractableScript : Interactable
    {
        public int dropAmount = 1;

        private bool given = false;

        // Update is called once per frame
        void Update()
        {

        }

        public override void Interact()
        {
            
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<NewPlayer>().Mana += dropAmount;
            Destroy(this.gameObject);


        }

        
       

        
    }
}
