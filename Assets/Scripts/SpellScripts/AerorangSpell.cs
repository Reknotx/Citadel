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

    public GameObject model;

    [HideInInspector]
    public Vector3 startingPos;

    [SerializeField]
    public Vector3 currentPos;

    [SerializeField]
    public Vector3 targetPos;

    [SerializeField]
    public bool facingRight;

    [SerializeField]
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

    [HideInInspector]
    public int damage;


    [HideInInspector]
    public int manaCost;


    /// <summary>the distance the spell will do before returning to the player</summary>
    [Tooltip("the distance the spell will do before returning to the player")]
    public float travelDistance = 6;

    


    private void Awake()
    {
        movingSpell = false;
        player = GameObject.FindGameObjectWithTag("Player");
        facingRight = player.GetComponent<NewPlayer>().facingRight;
        PlayerAnimationManager.Instance.ActivateTrigger("castFirewall");
        startingPos = player.transform.position;
        //this.transform.position = currentPos;
        currentPos = startingPos;

        damage = stats.damage;
        manaCost = stats.manaCost;
        model.transform.eulerAngles = new Vector3(0f, 270f, 0f);
        if (facingRight)
        {
            targetPos = new Vector3(startingPos.x + travelDistance, startingPos.y + 1, 0);

            targetPos.z = Mathf.Round(targetPos.z * 10f) / 10f;
            targetPos.x = Mathf.Round(targetPos.x * 10f) / 10f;
            targetPos.y = Mathf.Round(targetPos.y * 10f) / 10f;

           
        }
        else
        {
            targetPos = new Vector3(startingPos.x - travelDistance, startingPos.y + 1,0);

            targetPos.z = Mathf.Round(targetPos.z * 10f) / 10f;
            targetPos.x = Mathf.Round(targetPos.x * 10f) / 10f;
            targetPos.y = Mathf.Round(targetPos.y * 10f) / 10f;

            //model.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        }

       
    }


    private void FixedUpdate()
    {

        facingRight = player.GetComponent<NewPlayer>().facingRight;
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
            if (model.transform.position.x - player.transform.position.x > 0)
            {
                model.transform.localEulerAngles = new Vector3(0f, 270f, 0f);

            }

            if (model.transform.position.x - player.transform.position.x < 0)
            {
                model.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
            }
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
        //target.GetComponent<Enemy>().TakeDamage(damage);
        Destroy(this.gameObject);
    }

    public override void Move()
    {
        ///Activate the movement logic here
        return;
    }


    public override void  OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if (goingBack)
            {
                Destroy(this.gameObject);
            }

        }

        if (other.gameObject.layer == 8)
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
           
        }


         
        if (other.gameObject.layer == 31)
        {
            return;
        }
    
    }
}
