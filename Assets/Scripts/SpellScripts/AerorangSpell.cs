using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerorangSpell : Spell
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
    public float speed = 5f;


    [HideInInspector]
    public float zPos;

    [HideInInspector]
    public float yPos;

    [HideInInspector]
    public float xPos;


    /// <summary>the distance the spell will do before returning to the player</summary>
    [Tooltip("the distance the spell will do before returning to the player")]
    public float travelDistance = 6;

    public float spellDamage = 10;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        facingRight = player.GetComponent<Player>().facingRight;
        startingPos = player.transform.position;
        //this.transform.position = currentPos;
        currentPos = startingPos;

        if (facingRight)
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


    private void FixedUpdate()
    {


        currentPos = this.transform.position;

        if (!goingBack)
        {
            zPos = 0f;
            currentPos.z = zPos;

            yPos = currentPos.y;
            yPos = Mathf.Round(yPos * 10f) / 10f;
            currentPos.y = yPos;

            xPos = currentPos.x;
            xPos = Mathf.Round(xPos * 10f) / 10f;
            currentPos.x = xPos;

            myTransform.LookAt(targetPos);
            myTransform.Translate(Vector3.forward * Time.deltaTime * speed);

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
            myTransform.Translate(Vector3.forward * Time.deltaTime * speed);



            if (currentPos == targetPos)
            {
                Destroy(this.gameObject);
            }
        }
    }


    public override void TriggerSpell(GameObject target)
    {
        target.GetComponent<IDamageable>().TakeDamage(stats.damage);
    }

    public override void Move()
    {
        ///Activate the movement logic here
        return;
    }


    public override void  OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (goingBack)
            {
                Destroy(this.gameObject);
            }

        }
    }
}
