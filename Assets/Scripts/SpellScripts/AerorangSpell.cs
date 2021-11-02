using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerorangSpell : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;


    /// <summary>the spells transform that is affected to make it move </summary>
    [Tooltip("the spells transform that is affected to make it move")]
    public Transform myTransform;

    [HideInInspector]
    public Vector3 startingPos;

    [HideInInspector]
    public Vector3 currentPos;

    [HideInInspector]
    public Vector3 targetPos;

    [HideInInspector]
    public bool facingRight;

    [HideInInspector]
    public bool goingBack = false;


    /// <summary>the spells movement speed </summary>
    [Tooltip("the spells movement speed")]
    public  float speed = 5f;

   
    [HideInInspector]
    public float zPos;

    [HideInInspector]
    public float yPos;

    [HideInInspector]
    public float xPos;
    
   
    /// <summary>the distance the spell will do before returning to the player</summary>
    [Tooltip ("the distance the spell will do before returning to the player")]
    public float travelDistance = 6;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        facingRight = player.GetComponent<Player>().facingRight;
        startingPos = player.transform.position;
        currentPos = startingPos;
        this.transform.position = currentPos;

        if(facingRight)
        {
            targetPos = new Vector3(startingPos.x + travelDistance, startingPos.y, startingPos.z);

            targetPos.z = Mathf.Round(targetPos.z * 10f) / 10f;
            targetPos.x = Mathf.Round(targetPos.x * 10f) / 10f;
            targetPos.y = Mathf.Round(targetPos.y * 10f) / 10f;
        }
        else
        {
            targetPos = new Vector3(startingPos.x - travelDistance, startingPos.y, startingPos.z);

            targetPos.z = Mathf.Round(targetPos.z * 10f) / 10f;
            targetPos.x = Mathf.Round(targetPos.x * 10f) / 10f;
            targetPos.y = Mathf.Round(targetPos.y * 10f) / 10f;
        }
    }


    private void Update()
    {

        
        currentPos = this.transform.position;


        zPos = 0f;
        currentPos.z = zPos;

        yPos = currentPos.y;
        yPos = Mathf.Round(yPos * 10f) / 10f;
        currentPos.y = yPos;

        xPos = currentPos.x;
        xPos = Mathf.Round(xPos * 10f) / 10f;
        currentPos.x = xPos;



        if (!goingBack)
        {
            myTransform.LookAt(targetPos);
            myTransform.Translate(Vector3.forward*Time.deltaTime*speed);
         

            if (currentPos == targetPos)
            {
                goingBack = true;
            }
        }
        else
        {
            targetPos = player.transform.position;

            targetPos.z = Mathf.Round(targetPos.z * 10f) / 10f;
            targetPos.x = Mathf.Round(targetPos.x * 10f) / 10f;
            targetPos.y = Mathf.Round(targetPos.y * 10f) / 10f;

            myTransform.LookAt(targetPos);
            myTransform.Translate(Vector3.forward * Time.deltaTime*speed);
           
            

            if (currentPos == targetPos)
            {
                Destroy(this.gameObject);
            }
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(goingBack)
            {
                Destroy(this.gameObject);
            }
        }
    }

}
