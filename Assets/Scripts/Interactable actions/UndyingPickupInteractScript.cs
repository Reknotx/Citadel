using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class UndyingPickupInteractScript : Item
    {
        //private bool given = false;
        //public GameObject player;
        //public bool playerInteracting = false;
        //public bool colliding = false;

        //private void Awake()
        //{
        //    player = GameObject.FindGameObjectWithTag("Player");
        //}

        //public void Update()
        //{
        //    playerInteracting = player.GetComponent<Player>().Interacting;

        //    if (playerInteracting == true && colliding == true)
        //    {

        //        Interact();
        //        Destroy(this.gameObject);

        //    }
        //}

        public override void Interact()
        {
            //given = true;
            Player.Instance.GetComponent<Player>().undying = true;
            base.Interact();
        }
    }
}