using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotatorScript : MonoBehaviour
{

    public GameObject player;
    public bool facingRightLocal = true;

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");


        facingRightLocal = player.GetComponent<Player>().facingRight;

        checkDirection();
    }


    public void checkDirection()
    {
        if (facingRightLocal == false)
        {
            this.gameObject.transform.eulerAngles = new Vector3(0f, -90f, 0f);
        }
        else
        {
            this.gameObject.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        }
    }
}
