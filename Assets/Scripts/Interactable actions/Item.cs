
/*
 * Author: Chase O'Connor
 * Date: 10/5/2021
 * 
 * Brief: The base class for all Items in the game. If it's an
 * item it also needs to override and CALL the base function here in
 * Item.
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public abstract class Item : Interactable
    {

        /// <summary>
        /// The base interact function for all items which destroys them.
        /// </summary>
        public override void Interact()
        {
            Destroy(gameObject);
        }
    }
}
