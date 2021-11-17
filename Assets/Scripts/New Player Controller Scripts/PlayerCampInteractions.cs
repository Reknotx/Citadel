using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactables;


public class PlayerCampInteractions : MonoBehaviour
{

    [HideInInspector]
    public bool Interacting = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   


    public void OnTriggerStay(Collider other)
    {


        #region Camp Collisions

        if (other.GetComponent<Interactable>() != null)
        {
            Interactable localInteractRef = other.GetComponent<Interactable>();

            if (localInteractRef is CastleEntranceInteractScript)
            {
                GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
                buttonController.GetComponent<SceneButtonControllerScript>().enterCastleBTN.SetActive(true);
            }

            if (localInteractRef is MineEntranceInteractScript)
            {
                GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
                buttonController.GetComponent<SceneButtonControllerScript>().enterMineBTN.SetActive(true);
            }

            if (localInteractRef is CampShopEntranceInteractScript)
            {
                GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
                buttonController.GetComponent<SceneButtonControllerScript>().enterCampShopBTN.SetActive(true);
            }
            ///turn on button manager

            if (Interacting)
                other.GetComponent<Interactable>().Interact();
        }
    }

    public void OnTriggerExit(Collider other)
    {


        if (other.gameObject.tag == "MineEntrance")
        {
            GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
            buttonController.GetComponent<SceneButtonControllerScript>().enterMineBTN.SetActive(false);
        }

        if (other.gameObject.tag == "CastleEntrance")
        {
            GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
            buttonController.GetComponent<SceneButtonControllerScript>().enterCastleBTN.SetActive(false);
        }

        if (other.gameObject.tag == "CampShopEntrance")
        {
            GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
            buttonController.GetComponent<SceneButtonControllerScript>().enterCampShopBTN.SetActive(false);
        }
    }

    #endregion
}
