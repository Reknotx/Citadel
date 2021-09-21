using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPickupInteractableScript : MonoBehaviour
{
    public float dropAmount = 1;

    private bool given = false;

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        if (given == false)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Player>().myMana += dropAmount;
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
