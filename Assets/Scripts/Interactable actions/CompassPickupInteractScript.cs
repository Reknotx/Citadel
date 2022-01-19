using UnityEngine;



namespace Interactables
{
    public class CompassPickupInteractScript : Item
    {
        public bool given = false;

        public GameObject compassGameObject;


        public override void Interact()
        {
            if(grounded)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInventory>().compass = true;
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
                    NewPlayer.Instance.inventory.compass = true;
                    NewPlayer.Instance.GetComponent<Inventory_UI>().AddItem(compassGameObject);
                    Destroy(gameObject);
                }
               
            }
        }
    }
}
