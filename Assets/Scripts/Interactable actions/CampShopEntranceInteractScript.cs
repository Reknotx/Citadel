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

public class CampShopEntranceInteractScript : MonoBehaviour
{
    public GameObject shopUI;

    //public void Interact()
    //{
    //    GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
    //    sceneManager.GetComponent<SceneManagerScript>().goToCampShop();
    //}

    public void Interact()
    {
        if (shopUI != null) shopUI.SetActive(true);
    }

    //public override void Interact()
    //{
    //    ///Turn on the shop
    //    if (shopUI != null) shopUI.SetActive(true);
    //}
}
