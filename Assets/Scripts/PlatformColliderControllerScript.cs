using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColliderControllerScript : MonoBehaviour
{
    

    public float passTime = .5f;

    public GameObject Player;

    public GameObject topPos;
    public Transform downPos;

    public bool dropPressed = false;
    public bool jumpPressed = false;

    public bool isColliding = false;
    public bool canPass = true;
    

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        dropPressed = Player.GetComponent<Player>().isDropPressed;
        jumpPressed = Player.GetComponent<Player>().isJumpPressed;
        

        if (isColliding && dropPressed == true && canPass)
        {

            StartCoroutine(PassThroughCoroutine());
            Player.transform.position = new Vector3(Player.transform.position.x, downPos.position.y, Player.transform.position.z);

            
            Player.GetComponent<Player>().myVelocity = new Vector2(Player.GetComponent<Player>().myVelocity.x, -10);
        }

    }

    IEnumerator PassThroughCoroutine()
    {

        canPass = false;
        yield return new WaitForSeconds(passTime);
        canPass = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isColliding = true;
            
            if ( jumpPressed )
            {
                Player.transform.position = new Vector3(Player.transform.position.x, topPos.transform.position.y, Player.transform.position.z);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            isColliding = false;
           
        }
    }


}
