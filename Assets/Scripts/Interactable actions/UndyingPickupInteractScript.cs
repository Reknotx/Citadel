using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndyingPickupInteractScript : MonoBehaviour
{
    private bool given = false;
    public GameObject player;
    public bool playerInteracting = false;
    public bool colliding = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        playerInteracting = player.GetComponent<Player>().Interacting;

        if (playerInteracting == true && colliding == true)
        {

            Interact();
            Destroy(this.gameObject);

        }
    }

    public void Interact()
    {
        if (given == false)
        {

            player.GetComponent<Player>().undying = true;
            given = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            colliding = true;

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            colliding = false;

        }
    }
}
