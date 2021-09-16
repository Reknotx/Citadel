using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleEntranceInteractScript : MonoBehaviour
{
   public void Interact()
    {
        GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
        sceneManager.GetComponent<SceneManagerScript>().goToCastle();
    }
}
