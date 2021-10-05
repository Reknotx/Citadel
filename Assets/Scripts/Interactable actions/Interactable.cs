/*
 * Author: Chase O'Connor
 * Date: 10/5/2021
 * 
 * Brief: This is the base class for all interactable items in the game.
 * There is a singular abstract function that all interactables need to call
 * in order to execute their specific logic in a more generic format to reduce
 * code.
 * 
 */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Namespace for all interactables
/// </summary>
namespace Interactables
{
    /// <summary>
    /// The base class for all of the interactable items we have in the game.
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        /// <summary>
        /// Called to execute the interactable logic for an interactable.
        /// </summary>
        public abstract void Interact();
    }
}
