using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class HealthPickupInteractableScript : Interactable
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

            if (player.GetComponent<NewPlayer>().Health !=player.GetComponent<NewPlayer>().MaxHealth)
            {
                if(player.GetComponent<NewPlayer>().Health + dropAmount <= player.GetComponent<NewPlayer>().MaxHealth)
                {
                    player.GetComponent<NewPlayer>().Health += dropAmount;
                }
                else
                {
                    player.GetComponent<NewPlayer>().Health = player.GetComponent<NewPlayer>().MaxHealth;
                }
               

                

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