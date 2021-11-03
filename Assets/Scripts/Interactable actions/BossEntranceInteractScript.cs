using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class BossEntranceInteractScript : Interactable
    {


        public override void Interact()
        {
            GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
            sceneManager.GetComponent<SceneManagerScript>().toBoss();
        }
    }
}
