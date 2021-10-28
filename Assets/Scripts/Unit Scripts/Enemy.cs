/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/1/2021
 * 
 * Brief:this script holds the stats and 
 * controls for the enemies of the game
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Unit
{
    public LootTable enemyLootTable;


    #region Enemy Stats

    #region Enemy's Base Stats/Important Controls
 

    ///<summary>This is the players Input system.</summary>
    private PlayerInputActions playerInputActions;

    ///<summary>This is the unit's private rigidbody.</summary>
    [SerializeField]
    protected Rigidbody _rigidBody;

    public Renderer m_render;
    #endregion
    #region Enemy's Ground/Directional Detection Stats

    ///<summary>This is the range of detection to the ground.</summary>
    private float _Reach = 1f;

    ///<summary>This tracks what direction the enemy is facing.</summary>
    [HideInInspector]
    public bool facingRightLocal;

    #endregion
    #region Enemy's Player Detection Stats

    [HideInInspector]
    public float yDistance;

    [HideInInspector]
    public float jumpVelocity;

    [HideInInspector]
    private bool canJump = true;

    [HideInInspector]
    public float jumpHeight;

    ///<summary>This is the range of detection to the player.</summary>
    [Range(0, 20)]
    private float _DetectionRange;
    
    ///<summary>This tracks what the ground detection raycast hits.</summary>
    RaycastHit hit;

    ///<summary>This targets the player for the Enemy.</summary>
    [HideInInspector]
    public GameObject player;

    #endregion
    #region Enemy AI Movement Stats
    [HideInInspector]
    public float followDistance;

    [HideInInspector]
    ///<summary>This is the distance from the player the enemy wills top at</summary>
    public float stoppingDistance;

    [HideInInspector]
    private float stopSpeed = 0f;

    [HideInInspector]
    private float normalSpeed;

    [HideInInspector]
    public float noJumpHeight;

    [HideInInspector]
    Vector2 currentDirection;

    [HideInInspector]
    public float distanceToPlayer;

    [HideInInspector]
    public AIPath Astar;

    [HideInInspector]
    public float nextWaypointDistance = 3f;

    //Path path;

    //int currentWaypoint = 0;

    //bool reachedEndOfPath = false;

    protected Seeker seeker;


    #endregion

    #endregion


    

    [HideInInspector]
    public bool seenByCamera = false;

    [Tooltip("Turn this on to disable all functions on the enemy for testing.")]
    public bool debug = false;

    [Tooltip("Activate this only to immediately kill the enemy.")]
    public bool killThis = false;

    public virtual void Start()
    {


        normalSpeed = speed;
        //Tyler Added code
        player = GameObject.FindGameObjectWithTag("Player");

        Astar = GetComponent<AIPath>();
        Health = maxHealth;
    }

    public override void Update()
    {

        if (killThis) TakeDamage(1000);

        if (debug) return;

        base.Update();
        

        #region Enemy AI Movement
        Move();
        #endregion

        yDistance = player.transform.position.y - transform.position.y;

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < followDistance)
        {
            if (Mathf.Abs(yDistance) < 9)
            {
                Astar.canMove = true;
            }
        }
        else
        {
            Astar.canMove = false;
        }

        if (isGrounded)
        {
            if (canJump)
            {
                //jump toward player

                //_rigidBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpVelocity));
                StartCoroutine(IsJumping());

            }

        }

        if (yDistance < noJumpHeight)
        {
            speed = stopSpeed;
        }
        else
        {
            speed = normalSpeed;
        }

        /*if (onPlatform == true)
        {
            _rigidBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpVelocity));
            StartCoroutine(Jumped());

        }*/



        #region Player Detection
        ///<summary>This sets the player as the target in the scene.</summary>
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");

        #endregion


        #region Ground/Platform detection
        ///<summary>This determines whether the unit is on a platform or not.</summary>
        var groundCheck = transform.TransformDirection(Vector3.down);
        Debug.DrawRay(transform.position, groundCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, groundCheck, out hit, _Reach) && hit.transform.tag == "platform")
        {
            onPlatform = true;

        }
        else
        {
            onPlatform = false;

        }

        ///<summary>This determines whether the unit is on the ground or not.</summary>
        Debug.DrawRay(transform.position, groundCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, groundCheck, out hit, _Reach) && hit.transform.tag == "ground")
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }


        var roofCheck = transform.TransformDirection(Vector3.up);
        Debug.DrawRay(transform.position, roofCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, roofCheck, out hit, _Reach) && hit.transform.tag == "platform")
        {
            throughPlatform = true;

        }
        else
        {
            throughPlatform = false;

        }

        ///<summary>this checks if the unit is trying to pass up through a platform and will assist.</summary>
        if (throughPlatform == true && justJumped == true)
        {
            //StartCoroutine(dropDown());
            _rigidBody.AddForce(Vector3.up * .03f, ForceMode.Impulse);
        }
        #endregion

        ///<summary> this damages the enemy over time if they are on fire</summary>
        if (onFire == true)
        {
            TakeDamage(onFireDamage * Time.deltaTime);
        }
    }

    #region Interactions with the Player
    public void Interact()
    {
        player.GetComponent<Player>().TakeDamage(1);
    }
    #endregion

    #region Enemy Actions

    ///<summary>this makes the unit move between points A and B.</summary>
    public void patrolAB()
    {
        return;
    }

    protected virtual void Move()
    {
        if (seenByCamera)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > stoppingDistance && Vector2.Distance(transform.position, player.transform.position) < followDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }

            if (transform.position.x - player.transform.position.x > 0)
            {
                facingRight = true;

            }

            if (transform.position.x - player.transform.position.x < 0)
            {
                facingRight = false;
            }
        }
    }
    #endregion

    private void OnBecameVisible()
    {
        seenByCamera = true;
    }

    #region Collision Detection
    ///<summary>These track the collisions between the enemy and in-game objects .</summary>
    public void OnTriggerEnter(Collider other)
    {
        ///<summary>This triggers when the enemy is hit with the light attack.</summary>
        if (other.gameObject.tag=="swordLight")
        {
            TakeDamage(player.GetComponent<Player>().meleeAttackDamage);
            //Tyler Added code
            if(Health <= 0)
            {
                //add drop stuff here


                Destroy(this.gameObject);
            }
            //end of Tyler code
            hitOnRight = player.GetComponent<Player>().facingRightLocal ;

            //if you turn on the bellow code, it will apply knockback to the light attack
            /*
            if (hitOnRight == true)
            {
                _rigidBody.AddForce(new Vector3(1, 0, 0) * 1f, ForceMode.Impulse);

            }
            else
            {
                _rigidBody.AddForce(new Vector3(-1, 0, 0) * 1f, ForceMode.Impulse);
            }
            */
        }

        ///<summary>This triggers when the enemy is hit with the heavy attack.</summary>
        if (other.gameObject.tag == "swordHeavy")
        {
            
            TakeDamage(player.GetComponent<Player>().meleeAttackDamage * 2);
            hitOnRight = player.GetComponent<Player>().facingRightLocal;
            

            if(hitOnRight == true)
            {
                _rigidBody.AddForce(new Vector3(player.GetComponent<Player>().knockbackForce, 0, 0) * 1f, ForceMode.Impulse);
             
            }
            else
            {
                _rigidBody.AddForce(new Vector3(-player.GetComponent<Player>().knockbackForce, 0, 0) * 1f, ForceMode.Impulse);
               
            }
        }
        if (other.gameObject.tag == "Player")
        {
            //TakeDamage(player.GetComponent<Player>().playerCollisionDamage);
            return;
        }

        if (other.gameObject.tag == "FireWallCast")
        {
            if(fireDamageTaken == false)
            {
                TakeDamage(other.GetComponent<FireWallSpellScript>().fireWallCollideDamage);
                fireDamageTaken = true;
            }
        }

        if (other.gameObject.tag == "FireWallWall")
        {
            if(onFire == false)
            {
                onFireDuration = 5f;
                onFireDamage = 3;
                StartCoroutine(onFireCoroutine());
            }
        }
    }

    IEnumerator IsJumping()
    {
        if (yDistance >= jumpHeight)
        {

            canJump = false;
            _rigidBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpVelocity));
            
            yield return new WaitForSeconds(3f);
            canJump = true;

        }
    }

    #endregion

    [Range(0f, 100f)]
    public float percentChanceToDropItem = 40f;

    public override void TakeDamage(float amount)
    {
        if (Health - amount <= 0)
        {
            float dropYes = Random.Range(0f, 100f);

            if (dropYes >= percentChanceToDropItem) return;

            GameObject item = enemyLootTable.Drop();

            if (item != null)
            {
                //Debug.Log("Success");
                Instantiate(item, transform.position, Quaternion.identity);
            }
        }

        base.TakeDamage(amount);
    }

}
