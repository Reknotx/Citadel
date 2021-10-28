using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColliderControllerScript : MonoBehaviour
{
    public bool isPassing = false;
    public bool passingComplete = false;

    public Collider myCollider;

    public float passTime = .5f;

    public GameObject Player;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PassThroughCoroutine()
    {
        myCollider.enabled = false;
        if (Player.GetComponent<Player>().dropping && Player.GetComponent<Player>().isDropPressed)
        {
            
            Player.GetComponent<Player>().myVelocity = new Vector2(Player.GetComponent<Player>().myVelocity.x, -16);
        }
        yield return new WaitForSeconds(passTime);
        myCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if ( Player.GetComponent<Player>().isDropPressed)
            {
                myCollider.enabled = false;
                Player.GetComponent<Player>().myVelocity = new Vector2(Player.GetComponent<Player>().myVelocity.x, -16);
            }

            if (Player.GetComponent<Player>().throughPlatform && Player.GetComponent<Player>().isJumpPressed)
            {
                myCollider.enabled = false;
                Player.GetComponent<Player>().myVelocity = new Vector2(Player.GetComponent<Player>().myVelocity.x, 6);
            }
        }
    }

    
}
