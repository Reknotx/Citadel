using UnityEngine;

namespace Interactables
{
    public class TreasureChestInteractScript : Interactable
    {

        public bool opened = false;
        public bool dropped = false;

        public GameObject lid;

        public Transform dropSpawnPos;

        public GameObject shuuesPickup;
        public GameObject undyingPickup;
        public GameObject spellStonePickup;
        public GameObject backShieldPickup;
        public GameObject MedicineSashPickup;
        public GameObject compassPickup;
        public GameObject serratedStonePickup;

        private Collider myCollider;


        // Start is called before the first frame update
        void Awake()
        {
            myCollider = GetComponent<Collider>();
            //GameObject player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (opened == true)
            {
                if (dropped == false)
                {
                    myCollider.enabled = false;
                    lid.transform.localRotation = Quaternion.Euler(-60, 0, 0);
                    randomSpawn();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                opened = true;
                
            }
        }


        public override void Interact()
        {
            opened = true;
            
        }

        private void randomSpawn()
        {
            var number = Random.Range(1, 8);

            if (number == 1)
            {
                var shuues = (GameObject)Instantiate(shuuesPickup, dropSpawnPos.position, dropSpawnPos.rotation);
            }
            if (number == 2)
            {
                var undying = (GameObject)Instantiate(undyingPickup, dropSpawnPos.position, dropSpawnPos.rotation);
            }
            if (number == 3)
            {
                var spellStone = (GameObject)Instantiate(spellStonePickup, dropSpawnPos.position, dropSpawnPos.rotation);
            }
            if (number == 4)
            {
                var backShield = (GameObject)Instantiate(backShieldPickup, dropSpawnPos.position, dropSpawnPos.rotation);
            }
            if (number == 5)
            {
                var medicineSash = (GameObject)Instantiate(MedicineSashPickup, dropSpawnPos.position, dropSpawnPos.rotation);
            }
            if (number ==6 )
            {
                var comapss = (GameObject)Instantiate(compassPickup, dropSpawnPos.position, dropSpawnPos.rotation);
            }
            if (number ==7 )
            {
                var serrated = (GameObject)Instantiate(serratedStonePickup, dropSpawnPos.position, dropSpawnPos.rotation);
            }

            dropped = true;

        }

        

    }
}