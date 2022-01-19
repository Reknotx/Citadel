using UnityEngine;

namespace Interactables
{
    public class BackShieldPickupInteractScript : Item
    {

        public bool given = false;

        public GameObject floatingShieldGameObject;

        public override void Interact()
        {
            if(grounded)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInventory>().floatingShield = true;
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
                    NewPlayer.Instance.inventory.floatingShield = true;
                    NewPlayer.Instance.GetComponent<Inventory_UI>().AddItem(floatingShieldGameObject);
                    Destroy(gameObject);
                }
                
            }
        }
    }
}
