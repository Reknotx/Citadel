using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerorangSpell : MonoBehaviour
{
    public GameObject player;

    public Transform myTransform;

    public Vector3 startingPos;

    public Vector3 currentPos;

    public Vector3 targetPos;

    public bool facingRight;

    public bool goingBack = false;

    public  float speed = 5f;

    public float distance;
    
    public  float travel;

    public float travelDistance = 6;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        facingRight = player.GetComponent<Player>().facingRight;
        startingPos = player.transform.position;
        this.transform.position = currentPos;

        if(facingRight)
        {
            targetPos = new Vector3(startingPos.x + travelDistance, startingPos.y, startingPos.z);
        }
        else
        {
            targetPos = new Vector3(startingPos.x - travelDistance, startingPos.y, startingPos.z);
        }
    }


    private void Update()
    {
        travel = Time.deltaTime * speed;
        currentPos = this.transform.position;

        if(!goingBack)
        {
            if(facingRight)
            {
                Vector3.MoveTowards(currentPos, targetPos, speed);
                distance += travel;
            }
            else
            {
                Vector3.MoveTowards(currentPos, targetPos, speed);
                distance += travel;
            }

            if(currentPos == targetPos)
            {
                goingBack = true;
            }
        }
        else
        {
            targetPos = player.transform.position;
            if (facingRight)
            {
                Vector3.MoveTowards(currentPos, targetPos, speed);
                distance -= travel;
            }
            else
            {
                Vector3.MoveTowards(currentPos, targetPos, speed);
                distance -= travel;
            }

            if (currentPos == targetPos)
            {
                Destroy(this.gameObject);
            }
        }
    }

}
