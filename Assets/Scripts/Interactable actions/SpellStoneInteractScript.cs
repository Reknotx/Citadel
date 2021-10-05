using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class SpellStoneInteractScript : Item
    {


        public override void Interact()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Player>().spellStone = true;
            base.Interact();
        }
    }
}