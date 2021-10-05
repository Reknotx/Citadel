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
            if (given == false)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<Player>().Health += dropAmount;
                given = true;
            }


        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Interact();
                Destroy(this.gameObject);
            }
        }
    }
}