using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotInteractableScript : MonoBehaviour
{
    public GameObject healthPickup;
    public GameObject manaPickup;
    public GameObject goldPickup;

    public Transform dropSpawnPos;

    private bool isBroken = false;

    // Update is called once per frame
    void Update()
    {
        if(isBroken == true)
        {
            randomSpawn();
            Destroy(this.gameObject);
        }
    }

    public void Interact()
    {
        isBroken = true;
    }

    private void randomSpawn()
    {
        var number = Random.Range(1, 6);
       
        if (number == 2)
        {
            var HealthPickup = (GameObject)Instantiate(healthPickup, dropSpawnPos.position, dropSpawnPos.rotation)  ;
        }
        if (number == 3)
        {
            var ManaPickup = (GameObject)Instantiate(manaPickup, dropSpawnPos.position, dropSpawnPos.rotation);
        }
        if (number == 4)
        {
            var GoldPickup = (GameObject)Instantiate(goldPickup, dropSpawnPos.position, dropSpawnPos.rotation);
        }
        else
        {
            return;
        }
       
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag =="swordLight")
        {
            Interact();
        }

        if (other.gameObject.tag == "swordHeavy")
        {
            Interact();
        }

        if (other.gameObject.tag == "FireWallCast")
        {
            Interact();
        }
    }
}
