using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class ShuuesPickupInteractScript : Item
    {

        public bool given = false;

        public GameObject shuuesGameObject;


        public override void Interact()
        {
            if(grounded)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInventory>().shuues = true;
                base.Interact();
            }
           
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                if(grounded)
                {
                    given = true;
                    NewPlayer.Instance.inventory.shuues = true;
                    NewPlayer.Instance.GetComponent<Inventory_UI>().AddItem(shuuesGameObject);
                    Destroy(this.gameObject);
                }
               
            }
        }
    }
}