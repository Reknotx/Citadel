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

    ///<summary>This determines whether the unit is on a platform or not.</summary>
    [HideInInspector]
    public bool onPlatform;

    ///<summary>This determines whether the unit is going through a platform or not.</summary>
    [HideInInspector]
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


    #region Poisoned and fire and bleed shit

    /// <summary> this determines if the unit has recently taken ticking poison damage </summary>
    private bool poisonDamageTaken;

    /// <summary> this determines if the unit has recently taken ticking fire damage </summary>
    private bool fireDamageTaken;
    
    /// <summary> this determines how quickly on fire damage will tick against health </summary>
    private float onFireDamageRate = 1f;

    /// <summary> This determines the delay between taking on fire damage</summary>
    private float onFireDamageDelay = 2f;

    /// <summary> this determines how quickly on fire damage will tick against health </summary>
    private float poisonedDamageRate = 1f;

    /// <summary> This determines the delay between taking on fire damage</summary>
    private float poisonedDamageDelay = 2f;
    
    /// <summary> this determines how long the unit will be on fire for</summary>
    private float onFireDuration;

    /// <summary> this determines how much damage per tick will be applied to the unit</summary>
    private int onFireDamage;

    /// <summary> this determines if the unit is on fire or not </summary>
    private bool onFire;

    /// <summary> this determines if the unit is poisoned or not </summary>
    //[HideInInspector]
    [HideInInspector]
    public bool poisoned;

    /// <summary> this determines how long the unit will be on fire for</summary>
    [HideInInspector]
    public float poisonedDuration = 5f;

    /// <summary> this determines how much damage per tick will be applied to the unit</summary>
    [HideInInspector]
    public int poisonedDamage;


    private int bleedDuration = 5;
    private float bleedDamage;
    private float nextTick;
    private float tickDelay = 1f;
    public bool bleeding;
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
            BoxCollider[] colliders = GetComponents<BoxCollider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
                this.GetComponent<BoxCollider>().enabled = false;
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            StartCoroutine(deathCoroutine());
        }

        if (killThis) TakeDamage(1000);

        if (debug) return;

        base.Update();

        #region onfire and poison shit and bleed
        //this determines if the unit can take damage from a initially cast fire spell
        if (onFire)
        {
            onFireDamageDelay -= Time.deltaTime * onFireDamageRate;
            if (onFireDamageDelay <= 0)
            {
                fireDamageTaken = false;
                onFireDamageDelay = 2f;
            }
        }

        if (poisoned)
        {
            poisonedDamageDelay -= Time.deltaTime * poisonedDamageRate;
            if (poisonedDamageDelay <= 0)
            {
                poisonDamageTaken = false;
                poisonedDamageDelay = 2f;
            }
        }

        if (bleeding)
        {
            nextTick -= Time.deltaTime;
            if (nextTick <= 0)
            {
                TakeDamage(bleedDamage);
                nextTick = Time.time + tickDelay;
            }
        }
        
        #endregion
        
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
            Astar.canMove = distanceToPlayer < followDistance && Mathf.Abs(yDistance) < 9;
        }
        else
        {
            if (distanceToPlayer < goblinFollowDistance && Mathf.Abs(yDistance) < 100)
            {
                Astar.canMove = true;
            }
        }

        StartCoroutine(movingTest());

        if (grounded && canJump)
        {
            //jump toward player

            //_rigidBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpVelocity));
            StartCoroutine(IsJumping());

        }

        speed = yDistance < noJumpHeight ? stopSpeed : normalSpeed;

        #region Player Detection
        //This sets the player as the target in the scene.
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");

        #endregion


        #region Ground/Platform detection
        
        var groundCheck = transform.TransformDirection(Vector3.down);
        
        //This determines whether the unit is on a platform or not.
        Debug.DrawRay(transform.position, groundCheck * _Reach, Color.red);
        onPlatform = Physics.Raycast(transform.position, groundCheck, out hit, _Reach) &&
                     hit.transform.CompareTag("platform");

        //This determines whether the unit is on the ground or not.
        Debug.DrawRay(transform.position, groundCheck * _Reach, Color.red);
        grounded = Physics.Raycast(transform.position, groundCheck, out hit, _Reach) &&
                   hit.transform.CompareTag("ground");


        var roofCheck = transform.TransformDirection(Vector3.up);
        Debug.DrawRay(transform.position, roofCheck * _Reach, Color.red);
        throughPlatform = Physics.Raycast(transform.position, roofCheck, out hit, _Reach) &&
                          hit.transform.CompareTag("platform");

        //this checks if the unit is trying to pass up through a platform and will assist.
        if (throughPlatform && justJumped)
        {
            //StartCoroutine(dropDown());
            _rigidBody.AddForce(Vector3.up * .03f, ForceMode.Impulse);
        }
        #endregion

        //this damages the enemy over time if they are on fire
        if (onFire)
        {
            
            TakeDamage(onFireDamage * Time.deltaTime);
        }

        if(poisoned)
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
        if (!seenByCamera) return;
        if (transform.position.x - player.transform.position.x > 0)
        {
            facingRight = true;
        }
        if (transform.position.x - player.transform.position.x < 0)
        {
            facingRight = false;
        }


        if (Vector2.Distance(transform.position, player.transform.position) > stoppingDistance &&
            Vector2.Distance(transform.position, player.transform.position) < followDistance)
            transform.position =
                Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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
        //This triggers when the enemy is hit with the heavy attack.
        if (other.gameObject.CompareTag("swordHeavy"))
        {
            hitOnRight = player.GetComponent<NewPlayer>().facingRight;
            
            Debug.Log("DamagePopup");
            _rigidBody.AddForce(new Vector3(hitOnRight ? 5 : -5, 0, 0), ForceMode.Impulse);
        }

        if (other.gameObject is {layer: 7} || other.gameObject is {layer: 12})
        {
            NewPlayer.Instance.TakeDamage(contactDamage);
            
            Debug.Log("DamagePopup");
            return;
        }

        if (other.gameObject.CompareTag("FireWallWall") && onFire == false)
        {
            onFireDuration = 5f;
            onFireDamage = 3;
            StartCoroutine(onFireCoroutine());
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

    IEnumerator onFireCoroutine ()
    {
        
        onFire = true;
        yield return new WaitForSeconds(onFireDuration);
        onFire = false;
    }
    
    public IEnumerator poisonedCoroutine()
    {

        poisoned = true;
        yield return new WaitForSeconds(poisonedDuration);
        poisoned = false;
    }

    public void StartBleed(float dmg)
    {
        bleedDamage = dmg / bleedDuration;
        StartCoroutine(Bleed());
    }
    
    IEnumerator Bleed()
    {
        bleeding = true;
        yield return new WaitForSeconds(bleedDuration);
        bleeding = false;
    }
    
}
