/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/14/2021
 * 
 * Brief:This scripts controls the mine 
 * entrance interactions with the player 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class MineEntranceInteractScript : Interactable
    {
        public override void Interact()
        {
            GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
            sceneManager.GetComponent<SceneManagerScript>().goToMine();
        }
    }
}