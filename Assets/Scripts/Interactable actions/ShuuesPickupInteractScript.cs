using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class ShuuesPickupInteractScript : Item
    {
        /*
        private bool given = false;
        public GameObject player;
        public bool playerInteracting = false;
        public bool colliding = false;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void Update()
        {
            playerInteracting = player.GetComponent<Player>().Interacting;

            if (playerInteracting == true && colliding == true)
            {

                Interact();
                Destroy(this.gameObject);

            }

        }

        */

        public override void Interact()
        {

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Player>().shuues = true;
            base.Interact();
        }

        //public void OnTriggerEnter(Collider other)
        //{
        //    if (other.gameObject.tag == "Player")
        //    {

        //        colliding = true;

        //    }
        //}

        //public void OnTriggerExit(Collider other)
        //{
        //    if (other.gameObject.tag == "Player")
        //    {

        //        colliding = false;

        //    }
        //}
    }
}