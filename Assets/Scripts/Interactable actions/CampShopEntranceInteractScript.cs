using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampShopEntranceInteractScript : MonoBehaviour
{
    public void Interact()
    {
        GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
        sceneManager.GetComponent<SceneManagerScript>().goToCampShop();
    }
}
