using UnityEngine;

namespace Interactables
{
    public class GoldPickupInteractableScript : Interactable
    {
        public int dropAmount = 1;

       // private bool given = false;

        // Update is called once per frame
        void Update()
        {

        }

        public override void Interact()
        {
            
                
            NewPlayer.Instance.inventory.goldStorage.MineGoldToPermGold(dropAmount);
            
            Destroy(gameObject);



        }


        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                Interact();
            }
        }


    }
}
