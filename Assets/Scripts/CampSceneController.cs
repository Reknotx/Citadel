using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampSceneController : MonoBehaviour
{
    public bool gameWasReset = true;


    public GameObject PlayerPrefab;

    public Transform playerSpawnPoint;

    // Update is called once per frame
    void Update()
    {
        if(gameWasReset == true)
        {
            gameWasReset = false;
            respawnPlayer();
        }
    }

    public void respawnPlayer()
    {
        var Player = (GameObject)Instantiate(PlayerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
    }
}
