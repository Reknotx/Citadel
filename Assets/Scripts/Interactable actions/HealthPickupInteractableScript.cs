using UnityEngine;

namespace Interactables
{
    public class HealthPickupInteractableScript : Interactable
    {


        public float dropAmount = 1;

        private bool given = false;


        private NewPlayer player;
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>();
        }


        // Update is called once per frame
        void Update()
        {
            dropAmount = player.MaxHealth * .025f;
        }

        public override void Interact()
        {

            

            if (player.Health !=player.MaxHealth)
            {
                if(player.Health + dropAmount <= player.MaxHealth)
                {
                    player.Health += dropAmount;
                }
                else
                {
                    player.Health = player.MaxHealth;
                }
               

                

                    Destroy(gameObject);
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