using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestInteractScript : MonoBehaviour
{

    public bool opened = false;
    public bool dropped = false;

    public GameObject lid;

    public Transform dropSpawnPos;

    public GameObject shuuesPickup;
    public GameObject undyingPickup;
    public GameObject spellStonePickup;
    public GameObject backShieldPickup;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (opened == true)
        {
            if(dropped == false)
            {
                randomSpawn();
                lid.transform.rotation = Quaternion.Euler(60, 0, 0);
            }
            
        }
    }


    public void Interact()
    {
        opened = true;
    }

    private void randomSpawn()
    {
        var number = Random.Range(1, 5);

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

        dropped = true;

    }

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if( other.GetComponent<Player>().Interacting == true)
            {
                Interact();
                other.GetComponent<Player>().Interacting = false;
            }
        }
    }

}
