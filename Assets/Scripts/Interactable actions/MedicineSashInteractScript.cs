using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Interactables
{
    public class MedicineSashInteractScript : Item
    {
        public bool given = false;

        public GameObject medicineSashGameObject;

        public override void Interact()
        {
            if(grounded)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInventory>().medicineStash = true;
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
                    NewPlayer.Instance.inventory.medicineStash = true;
                    NewPlayer.Instance.GetComponent<Inventory_UI>().AddItem(medicineSashGameObject);
                    Destroy(this.gameObject);
                }
               
            }
        }
    }
}
