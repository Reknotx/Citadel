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
           
            respawnPlayer();
        }
    }

    public void respawnPlayer()
    {
         gameWasReset = false;
        var Player = (GameObject)Instantiate(PlayerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);

    }
}
