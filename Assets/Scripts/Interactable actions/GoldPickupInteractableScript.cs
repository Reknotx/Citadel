using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class GoldPickupInteractableScript : Interactable
    {
        public float dropAmount = 1;

        private bool given = false;

        // Update is called once per frame
        void Update()
        {

        }

        public override void Interact()
        {
            if (given == false)
            {
                GameObject goldHandler = GameObject.FindGameObjectWithTag("PlayerGoldHandler");
                goldHandler.GetComponent<GoldHandler>()._mySoftGold += dropAmount;
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
