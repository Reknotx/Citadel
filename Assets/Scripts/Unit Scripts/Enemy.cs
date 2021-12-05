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
using UnityEngine.UI;

public class Enemy : Unit
{
    public LootTable enemyLootTable;

    [HideInInspector]
    ///<summary>This determines whether the unit is on a platform or not.</summary>
    public bool onPlatform;

    [HideInInspector]
    ///<summary>This determines whether the unit is going through a platform or not.</summary>
    public bool throughPlatform;

    #region Enemy Stats
    
    ///<summary>This determines what direction the unit hit another unit.</summary>
    protected bool hitOnRight;

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

    public int contactDamage;

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
    //[HideInInspector]
    public GameObject player;

    public bool GoblinSpotted = false;

    #endregion
    #region Enemy AI Movement Stats
    [HideInInspector]
    public float followDistance;

    [HideInInspector]
    public float goblinFollowDistance = 150f;

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
    #region Enemy Animations
    public Animator animator;
    public bool isAttacking;
    public bool isDead;
    public bool isMoving;

    #endregion

    private bool lootDropped = false;
    #endregion




    [HideInInspector]
    public bool seenByCamera = false;

    [Tooltip("Turn this on to disable all functions on the enemy for testing.")]
    public bool debug = false;

    [Tooltip("Activate this only to immediately kill the enemy.")]
    public bool killThis = false;

    public Image enemyHealth;

    public Image HealthIMG;

    private float calculateHealth;

    [SerializeField]
   // private Transform pfDamagePopup;


    public override float Health
    {
        get => _health;
        set
        {
            _health = value;

            if (_health <= 0)
            {
                isDead = true;
                
            }
        }
    }

    public override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        Astar = GetComponent<AIPath>();
        normalSpeed = speed;
        HealthIMG.gameObject.SetActive(false);
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
            animator.SetBool("isAttacking", isAttacking);
            animator.SetBool("isDead", isDead);
        }
    }

    public override void Start()
    {
        base.Start();

        normalSpeed = speed;
        //Tyler Added code
        player = GameObject.FindGameObjectWithTag("Player");

        Astar = GetComponent<AIPath>();

        HealthIMG.gameObject.SetActive(false);

        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
            animator.SetBool("isAttacking", isAttacking);
            animator.SetBool("isDead", isDead);
        }
    }

    public override void Update()
    {
        if(isDead == true)
        {
            Collider[] colliders = GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
                this.GetComponent<Collider>().enabled = false;
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            StartCoroutine(deathCoroutine());
        }

        if (killThis) TakeDamage(1000);

        if (debug) return;

        base.Update();

        if (Health < MaxHealth)
        {
            HealthIMG.gameObject.SetActive(true);
            calculateHealth = (float)Health / MaxHealth;
            enemyHealth.fillAmount = Mathf.MoveTowards(enemyHealth.fillAmount, calculateHealth, Time.deltaTime);
        }
        else
        {
            HealthIMG.gameObject.SetActive(false);
        }


        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
            animator.SetBool("isAttacking", isAttacking);
            animator.SetBool("isDead", isDead);
        }

        #region Enemy AI Movement
        Move();
        #endregion

        

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        yDistance = player.transform.position.y - transform.position.y;

        if (!GoblinSpotted)
        {
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
        }
        else if (GoblinSpotted)
        {
            if (distanceToPlayer < goblinFollowDistance)
            {
                if (Mathf.Abs(yDistance) < 100)
                {
                    Astar.canMove = true;
                }
            }
        }

        StartCoroutine(movingTest());

        if (grounded)
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
            grounded = true;
        }
        else
        {
            grounded = false;
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

        if(poisoned == true)
        {
            TakeDamage(poisonedDamage * Time.deltaTime);

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

            if (transform.position.x - player.transform.position.x > 0)
            {
                facingRight = true;

            }

            if (transform.position.x - player.transform.position.x < 0)
            {
                facingRight = false;
            }


            if (Vector2.Distance(transform.position, player.transform.position) > stoppingDistance && Vector2.Distance(transform.position, player.transform.position) < followDistance)
            {
                
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
               
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
        ///This triggers when the enemy is hit with the heavy attack.
        if (other.gameObject.tag == "swordHeavy")
        {
            hitOnRight = player.GetComponent<NewPlayer>().facingRight;
            
            Debug.Log("DamagePopup");
            _rigidBody.AddForce(new Vector3(hitOnRight ? 5 : -5, 0, 0), ForceMode.Impulse);
        }

        if (other.gameObject.layer == 7 || other.gameObject.layer == 12)
        {
            NewPlayer.Instance.TakeDamage(contactDamage);
            
            Debug.Log("DamagePopup");
            return;
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
    private bool PopUpOut = false;
    [Range(0f, 100f)]
    public float percentChanceToDropItem = 40f;

    public override void TakeDamage(float amount)
    {
        var currentHealth = Health;
        base.TakeDamage(amount);

        if (currentHealth - amount <= 0)
        {
            if(lootDropped == false)
            {
                contactDamage = 0;
                lootDropped = true;
                float dropYes = Random.Range(0f, 100f);

                if (dropYes >= percentChanceToDropItem) return;

                GameObject item = enemyLootTable.Drop();

                if (item != null)
                {
                    //Debug.Log("Success");
                    Instantiate(item, transform.position, Quaternion.identity);
                }
            }
            
        }
        //DamagePopup.Create(transform.position, (int)amount);

        if (!PopUpOut)
        {
            DamagePopup.Create(transform.position, amount);
            StartCoroutine(DmgPopUp());
        }

        
    }

    public IEnumerator movingTest()
    {
        var testPos = this.transform.position;
        yield return new WaitForSeconds(1f);
        if(testPos == this.transform.position)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    public IEnumerator deathCoroutine()
    {
        var waitTime = 4f;
        Astar.enabled = false;
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }

    IEnumerator DmgPopUp()
    {
        PopUpOut = true;
        yield return new WaitForSeconds(0.5f);
        PopUpOut = false;
    }

}
