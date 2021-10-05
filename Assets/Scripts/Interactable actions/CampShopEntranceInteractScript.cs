/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/14/2021
 * 
 * Brief:This scripts controls the camp shop 
 * entrance interactions with the player 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class CampShopEntranceInteractScript : Interactable
    {
        public GameObject shopUI;

        public override void Interact()
        {
            if (shopUI != null) shopUI.SetActive(true);
            var sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
            sceneManager.GetComponent<SceneManagerScript>().goToCampShop();
            //Player.Instance.canMove = false;
        }

        //public override void Interact()
        //{
        //    ///Turn on the shop
        //    if (shopUI != null) shopUI.SetActive(true);
        //}
       }
}
