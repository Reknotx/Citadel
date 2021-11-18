using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReboundSpell : Spell
{
    [HideInInspector]
    public GameObject player;

    //[HideInInspector]
    public bool facingRight;

    ///<summary>This is the range of detection to the ground.</summary>
    private float _Reach = .6f;

    ///<summary>This tracks what the ground detection raycast hits.</summary>
    private RaycastHit hit;

    /// <summary>the spells movement speed </summary>
    [Tooltip("the spells movement speed")]
    public float speed = 5f;


    // Vector3[] PossibleDirections;

    public Transform myTransform;

    public List<Vector3> mydirections;


    public Vector3 CurrentDirection;

    public int bounceCount = 0;


    [HideInInspector]
    public Vector3 up = Vector3.up;
    [HideInInspector]
    public Vector3 down = Vector3.down;
    [HideInInspector]
    public Vector3 left = Vector3.left;
    [HideInInspector]
    public Vector3 right = Vector3.right;
    [HideInInspector]
    public Vector3 upRight = Vector3.up + Vector3.right;
    [HideInInspector]
    public Vector3 upLeft = Vector3.up + Vector3.left;
    [HideInInspector]
    public Vector3 downRight = Vector3.down + Vector3.right;
    [HideInInspector]
    public Vector3 downLeft = Vector3.down + Vector3.left;


    [HideInInspector]
    public bool upRightAdded = false;
    [HideInInspector]
    public bool upLeftAdded = false;
    [HideInInspector]
    public bool downRightAdded = false;
    [HideInInspector]
    public bool downLeftAdded = false;
    [HideInInspector]
    public bool rightAdded = false;
    [HideInInspector]
    public bool leftAdded = false;
    [HideInInspector]
    public bool upAdded = false;
    [HideInInspector]
    public bool downAdded = false;

    [HideInInspector]
    public int damage;

    [HideInInspector]
    public int manaCost;

    public bool canAdd = false;


    // Start is called before the first frame update
    void Awake()
    {
        movingSpell = false;
        player = GameObject.FindGameObjectWithTag("Player");
        facingRight = player.GetComponent<NewPlayer>().facingRight;

        damage = stats.damage;
        manaCost = stats.manaCost;

        if (facingRight == true)
        {
            CurrentDirection = right;
        }
        else
        {
            CurrentDirection = left;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myTransform.Translate(CurrentDirection*Time.deltaTime*speed);

        if(bounceCount >= 8)
        {
            Destroy(this.gameObject);
        }

        #region possible direction detection
        var upRightCheck = transform.TransformDirection(upRight);
        Debug.DrawRay(transform.position, upRightCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, upRightCheck, out hit, _Reach) && (hit.transform.tag == "ground" || hit.transform.tag == "platform"))
        {
            mydirections.Remove(upRight);
            upRightAdded = false;
        }
        else
        {
            if(upRightAdded == false)
            {
                upRightAdded = true;
                mydirections.Add(upRight);
            }
           
        }

        var downRightCheck = transform.TransformDirection(downRight);
        Debug.DrawRay(transform.position, downRightCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, downRightCheck, out hit, _Reach) && (hit.transform.tag == "ground" || hit.transform.tag == "platform"))
        {
            mydirections.Remove(downRight);
            downRightAdded = false;
        }
        else
        {
            if (downRightAdded == false)
            {
                downRightAdded = true;
                mydirections.Add(downRight);
            }

        }

        var rightCheck = transform.TransformDirection(right);
        Debug.DrawRay(transform.position, rightCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, rightCheck, out hit, _Reach) && (hit.transform.tag == "ground" || hit.transform.tag == "platform"))
        {
            mydirections.Remove(right);
            rightAdded = false;
        }
        else
        {
            if (rightAdded == false)
            {
                rightAdded = true;
                mydirections.Add(right);
            }

        }

        var leftCheck = transform.TransformDirection(left);
        Debug.DrawRay(transform.position, leftCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, leftCheck, out hit, _Reach) && (hit.transform.tag == "ground" || hit.transform.tag == "platform"))
        {
            mydirections.Remove(left);
            leftAdded = false;
        }
        else
        {
            if (leftAdded == false)
            {
                leftAdded = true;
                mydirections.Add(left);
            }

        }

        var upLefttCheck = transform.TransformDirection(upLeft);
        Debug.DrawRay(transform.position, upLefttCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, upLefttCheck, out hit, _Reach) && (hit.transform.tag == "ground" || hit.transform.tag == "platform"))
        {
            mydirections.Remove(upLeft);
            upLeftAdded = false;
        }
        else
        {
            if (upLeftAdded == false)
            {
                upLeftAdded = true;
                mydirections.Add(left);
            }

        }

        var downLeftCheck = transform.TransformDirection(downLeft);
        Debug.DrawRay(transform.position, downLeftCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, downLeftCheck, out hit, _Reach) && (hit.transform.tag == "ground" || hit.transform.tag == "platform"))
        {
            mydirections.Remove(downLeft);
            downLeftAdded = false;
        }
        else
        {
            if (downLeftAdded == false)
            {
                downLeftAdded = true;
                mydirections.Add(downLeft);
            }

        }

        var upCheck = transform.TransformDirection(up);
        Debug.DrawRay(transform.position, upCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, upCheck, out hit, _Reach) && (hit.transform.tag == "ground" || hit.transform.tag == "platform"))
        {
            mydirections.Remove(up);
            upAdded = false;
        }
        else
        {
            if (upAdded == false)
            {
                upAdded = true;
                mydirections.Add(up);
            }

        }

        var downCheck = transform.TransformDirection(down);
        Debug.DrawRay(transform.position, downCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, downCheck, out hit, _Reach) && (hit.transform.tag == "ground" || hit.transform.tag == "platform"))
        {
            mydirections.Remove(down);
            downAdded = false;
        }
        else
        {
            if (downAdded == false)
            {
                downAdded = true;
                mydirections.Add(down);
            }

        }


        
        
        

        #endregion



    }

    public void changeDirection()
    {
        int randomDirection = Random.Range(0, mydirections.Count);
        CurrentDirection = mydirections[randomDirection];
    }


    public override void TriggerSpell(GameObject target)
    {
       
        return;
    }

    public override void Move()
    {
        ///Activate the movement logic here
        return;
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 31)
        {
           
            changeDirection();
        }

        

        if(other.gameObject.layer == 8)
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(this.gameObject);
        }

        if (other.gameObject.layer == 31)
            return;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 31)
        {
            if (canAdd)
            {
               
                canAdd = false;
            }

        }
        
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 31)
        {
            if(!canAdd)
            {
                bounceCount++;
                canAdd = true;
            }
           
        }

       


    }


    

}
