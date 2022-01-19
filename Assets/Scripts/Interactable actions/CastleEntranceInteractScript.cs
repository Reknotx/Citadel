/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/14/2021
 * 
 * Brief:This scripts controls the castle 
 * entrance interactions with the player 
 */

using UnityEngine;

namespace Interactables
{
    public class CastleEntranceInteractScript : Interactable
    {
        public override void Interact()
        {
            GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
            sceneManager.GetComponent<SceneManagerScript>().goToCastle();
        }
    }
}
