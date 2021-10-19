    /*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/1/2021
 * 
 * Brief:this script holds the specific 
 * components/data of the player character 
 * WASD = moving up,left,down and right respectively 
 * left mouse button = Attack 1 (light attack)
 * right mouse button = Attack 2 (heavy attack)
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Interactables;

public class Player : Unit
{

    public static Player Instance;
    #region Player Stats

            #region Player's Base Stats/Important controls
    [Header ("player base stats")]

    ///<summary>This is the units health.</summary>
    public float myHealth;

    public override float Health 
    { 
        get => base.Health; 
        set
        {
            _health = Mathf.Clamp(value, 0, maxHealth);
            if (_health <= 0)
            {
                if (undying == true)
                {
                    undying = false;
                    myHealth = Mathf.Round(maxHealth * 0.15f);
                }
                else
                {
                    Debug.Log("Reset");
                    ResetGame();
                }

            }
        }
    }

    ///<summary>This is the  units starting health.</summary>
    public float startingHealth;

    private float calculateHealth;

    private float calculateMana;

    ///<summary>This is the units mana for magic casting.</summary>
    public float myMana;

    ///<summary>This is the units maximum mana for magic casting.</summary>
    public float maxMana; //

    ///<summary>This is the units starting .</summary>
    public float startingMana;

    ///<summary>This is the players Input system.</summary>
    private PlayerInputActions playerInputActions;

    ///<summary>This is the unit's private rigidbody.</summary>
    [SerializeField]
    public Rigidbody _rigidBody;

    #endregion
            #region Player's Ground/Directional Detection Stats

    ///<summary>This is the range of detection to the ground.</summary>
    private float _Reach = 1f;

    ///<summary>This tracks what the ground detection raycast hits.</summary>
   private RaycastHit hit;

    ///<summary>This tracks what direction the player is facing.</summary>
    //[HideInInspector]
    public bool facingRightLocal;

    #endregion
            #region Player's Attack Stats/Spell Prefabs

    [Header("player attack stats")]
    ///<summary>This determines how far the player will knock back an enemy with the heavy attack.</summary>
    public float knockbackForce;

    ///<summary>This determines the range of the player's melee attack.</summary>
    public float meleeAttackRange = 1f; //

    ///<summary>This determines the damage of the player's melee attack.</summary>
    public int meleeAttackDamage; //

    ///<summary>This determines the damage the player deals to an enemy when they collide.</summary>
    public int playerCollisionDamage;

    public GameObject ManaHealthController;

    /// <summary>this is the physical gameobject that is cast during the firewall spell</summary>
    public GameObject fireWall_prefab;
    #endregion
            #region Bool Determinates 
    [Header("player bool determinates")]
    /// <summary> determines if the player can move or not </summary>
    [HideInInspector]
        public bool canMove = true;

    /// <summary> determines if the player can jump once more in the air or not </summary>
    [HideInInspector]
    public bool canDoubleJump = true;

    /// <summary> determines if the player can jump once more in the air or not </summary>
    [HideInInspector]
    public bool hasDoubleJump = false;

    [HideInInspector]
    public bool invulnerable = false;

    /// <summary> determines if the player is trying to interact with things or not </summary>
    [HideInInspector]
    public bool Interacting = false;
   // {
       // return playerInputActions.PlayerControl.
    //}

    [HideInInspector]
    public bool canInteract = false;

    [HideInInspector]
    /// <summary> this keeps track of if the player is in the camp shop or not  </summary>
    public bool inCampShop = false;

    [HideInInspector]
    /// <summary> this keeps track of if the player is in the mine  or not  </summary>
    public bool inMine = false;

    [HideInInspector]
    /// <summary> this keeps track of if the player is in the mine shop or not  </summary>
    public bool inMineShop = false;

    [HideInInspector]
    public bool grounded;

    [HideInInspector]
    public bool isRunning = false;

    [HideInInspector]
    public bool isFalling = false;

    [HideInInspector]
    public bool isAttacking = false;

    public bool dmgPlayerByTick = false;

    public bool shieldActive;

    #endregion
            #region Bool/int Equipment

    [Header("player equipment")]
    public bool shuues = false;
    public bool undying = false;
    public bool spellStone = false;
    public bool floatingShield = false;
    public bool medicineStash = false;
    [HideInInspector]
    public GameObject flotingShieldObj;

    public int healthPotions = 0;
    public int manaPotions = 0;
    public int potionMax = 2;
    public int healthPotionMax = 2;
    public int manaPotionMax = 2;

    #endregion
    #region Animations
    [Header("player animations")]
    public Animator animator;
    private bool triggered = false;
    private float animationFinishTime = .5f;

    #endregion
            #region health and mana bars
    [Header("player health and mana bars")]
    /// <summary>
    /// Image holding the UI for player's health bar
    /// </summary>
    public Image healthBar;

    /// <summary>
    /// Image holding the UI for the player's mana bar
    /// </summary>
    public Image manaBar;

    /// <summary>
    /// Text displayed in player's health bar
    /// </summary>
    public Text healthText;

    /// <summary>
    /// Text displayed in player's mana bar
    /// </summary>
    public Text manaText;


    #endregion
    [Header("player attack names")]
    public string Attack1;
    public string Attack2;
    public string Attack3;

    [Header("player improved jumping ")]
    float gravity = -9.8f;
    float groundedGravity = -.05f;

    public bool isJumping = false;
    public bool isJumpPressed = false;
    public float initialJumpVelocity;
    public float maxJumpHeight;
    public float maxJumpTime;

    public Vector3 movementVelocity;
    public Vector3 myVelocity;



   

    #endregion


    private void Awake()
    {

        if(Instance != null && Instance != this)
            Destroy(Instance.gameObject);

        Instance = this;

        Health = maxHealth;
        myMana = maxMana;
        
        #region Player Movement Important Connectors
        ///<summary>The following is used to track player inputs and controls.</summary>
        playerInputActions = new PlayerInputActions();
         playerInputActions.PlayerControl.Enable();
         playerInputActions.PlayerControl.Jump.started += Jump2;
        playerInputActions.PlayerControl.Jump.canceled += Jump2;
        playerInputActions.PlayerControl.Movement.performed += movement2;
        playerInputActions.PlayerControl.Drop.started += Drop;
        playerInputActions.PlayerControl.Drop.canceled += Drop;
        setJumpVariables();
        



        #endregion

    }

   
    public override void Update()
    {
        Application.targetFrameRate = 60;
        facingRightLocal = facingRight;
        _rigidBody.velocity = myVelocity;
        if (dmgPlayerByTick)
        {
            dmgPlayerByTick = false;
            TakeDamage(1);
        }


        base.Update();

        if(floatingShield)
        {
            flotingShieldObj.SetActive(true);
        }
        if(medicineStash == true)
        {
            manaPotionMax = 3;
            healthPotionMax = 3;
            potionMax = 4;
        }
        

        #region Player Stat controls
       myHealth = Health;
        if(myHealth <= 0)
        {
            ResetGame();
        }

        if (myMana >= maxMana)
        {
            myMana = maxMana;
        }

        if (Interacting == true)
        {
            StartCoroutine(InteractCoroutine());
        }

        handleGravity();
        handleJump();
        

        #endregion

        #region Player Movement Detection
        ///<summary>This moves the player constantly while the input is held.</summary>
        if (canMove == true)
        {
            
            Vector2 inputVector = playerInputActions.PlayerControl.Movement.ReadValue<Vector2>();
            _rigidBody.MovePosition(transform.position + new Vector3(inputVector.x, 0, 0) * speed * Time.deltaTime);
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            
            if (animator != null)
            {
                animator.SetBool("isRunning", isRunning);
                animator.SetBool("isJumping", isJumping);
                animator.SetBool("isFalling", isFalling);
            }
                
            
        }
        else
        {
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        }


        if (onPlatform == true)
        {
            isGrounded = true;
        }
        if(isGrounded == true)
        {
            canDoubleJump = true;
            hasDoubleJump = false;
        }
        grounded = isGrounded;

        



        #endregion

        #region Ground/Platform detection
        ///<summary>This determines whether the unit is on a platform or not.</summary>
        var groundCheck = transform.TransformDirection(Vector3.down);
        Debug.DrawRay(transform.position, groundCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, groundCheck, out hit, _Reach) && hit.transform.tag == "platform")
        {
            onPlatform = true;
            isJumping = false;

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
            isJumping = false;
        }
        else
        {
            isGrounded = false;
        }

        



        ///<summary>This determines whether the unit is trying to jump up through a platform or not.</summary>
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
            StartCoroutine(dropDown());
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 6);
            
        }


       


        #endregion

        ///<summary>this sets the rate for how quickly players can cast spells </summary>
        spellCastDelay -= Time.deltaTime * spellCastRate;
        if (spellCastDelay <= 0)
        {
            canCast = true;
            spellCastDelay = 3f;
        }
    }

    private void FixedUpdate()
    {
        if (healthBar != null)
        {
            calculateHealth = Health / maxHealth;
            healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, calculateHealth, Time.deltaTime);
            healthText.text = "" + (int)myHealth;
        }

        if (manaBar != null)
        {
            calculateMana = myMana / maxMana;
            manaBar.fillAmount = Mathf.MoveTowards(manaBar.fillAmount, calculateMana, Time.deltaTime);
            manaText.text = "" + myMana;
        }
    }


    public void ResetGame()
    {
        maxHealth = startingHealth;
        myHealth = startingHealth;
        maxMana = startingMana;
        myMana = startingMana;
        //GetComponentInChildren<GoldHandler>().myHardGold = GetComponentInChildren<GoldHandler>().startingHardGold;
        
        var goldHandler = GameObject.FindGameObjectWithTag("PlayerGoldHandler");
        goldHandler.GetComponent<GoldHandler>()._mySoftGold = goldHandler.GetComponent<GoldHandler>().startingSoftGold;
        var goldTracker = GameObject.FindGameObjectWithTag("GoldTracker");
        goldTracker.GetComponent<PlayerGoldTrackerScript>().playerDead = true;
        GameObject SceneManager = GameObject.FindGameObjectWithTag("SceneManager");
        SceneManager.GetComponent<SceneManagerScript>().goToCamp();

    }

    public override void TakeDamage(float amount)
    {
        if (invulnerable)
            return;
        else
        {
            invulnerable = true;
            StartCoroutine(IFrames());
        }

        base.TakeDamage(amount);
    }

   

   


    #region Player Movement Actions
    /*
    /// <summary> This moves the player from side to side on the x axis  /// </summary>
    public void movement(InputAction.CallbackContext context)
    {
        if (canMove == true)
        {
            if (this != null)
            {
               
                    Vector2 inputVector = context.ReadValue<Vector2>();
                
                    _rigidBody.MovePosition(transform.position + new Vector3(inputVector.x, transform.position.y, 0) * speed * Time.deltaTime);
                    if (inputVector.x > 0)
                    {
                        facingRight = true;

                    }
                    if (inputVector.x < 0)
                    {
                        facingRight = false;
                    }

                    if(inputVector.x == 0)
                    {
                        isRunning = false;
                    }
                    else
                    {
                    
                        isRunning = true;
                    
                    }
                
                
            }
        }
    }
    */
    public void movement2(InputAction.CallbackContext context)
    {
        if (canMove == true)
        {
            if (this != null)
            {
                Vector2 inputVector = context.ReadValue<Vector2>();
                myVelocity.x = inputVector.x;
                myVelocity.z = inputVector.y;

                if (inputVector.x > 0)
                {
                    facingRight = true;

                }
                if (inputVector.x < 0)
                {
                    facingRight = false;
                }

                if (inputVector.x == 0)
                {
                    isRunning = false;
                }
                else
                {

                    isRunning = true;

                }

            }
        }
    }

    /*
    ///<summary>This triggers the unit to jump up.</summary>
    public void Jump(InputAction.CallbackContext context)
    {
        if (this != null)
        {
            
            if (context.performed && isGrounded == true )
            {
                
                _rigidBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpFroce));
                
                canDoubleJump = true;
               StartCoroutine(Jumped());
               
                
                
            }
            else if(shuues == true )
            {
                if(canDoubleJump == true)
                {
                    _rigidBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpFroce));
                    canDoubleJump = false;
                    StartCoroutine(Jumped());
                }
                
            }


            if (onPlatform == true)
            {

                _rigidBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpFroce));
                canDoubleJump = true;
                StartCoroutine(Jumped());

            }
            else if (  shuues == true )
            {
                if(canDoubleJump == true)
                {
                    _rigidBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpFroce));
                    canDoubleJump = false;
                    StartCoroutine(Jumped());
                }
                
            }

        }
    }
    */
    public void Jump2(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        float jumpTime = Time.deltaTime;
    }
    void setJumpVariables()
    {
        bool atApex = false;
        
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleGravity()
    {
        if (onPlatform)
        {
            isGrounded = true;
        }
        isFalling = myVelocity.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;
        if(isGrounded)
        {
            isJumping = false;
            myVelocity.y = groundedGravity;
        }
        else if(onPlatform)
        {
            myVelocity.y = groundedGravity;
        }
        else if(isFalling)
        {
            isJumping = false;
            float previousYVelocity = myVelocity.y;
            float newYVelocity = myVelocity.y + (gravity * fallMultiplier*Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * .5f, -20.0f);
            myVelocity.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = myVelocity.y;
            float newYVelocity = myVelocity.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            myVelocity.y = nextYVelocity ;
        }
    }
    void handleJump()
    {
        if(!isJumping && isGrounded  && isJumpPressed)
        {
            isJumping = true;
            myVelocity.y = initialJumpVelocity * .5f;
            canDoubleJump = true;
            StartCoroutine(Jumped());
        }
        else if(isJumping && isJumpPressed && shuues && canDoubleJump)
        {
            myVelocity.y = initialJumpVelocity * .5f;
            canDoubleJump = false;
            StartCoroutine(Jumped());
        }
        else if(!isJumpPressed && isGrounded && isJumping)
        {
            isJumping = false;
        }

        if (!isJumping && onPlatform && isJumpPressed)
        {
            isJumping = true;
            myVelocity.y = initialJumpVelocity * .5f;
            canDoubleJump = true;
            StartCoroutine(Jumped());
        }
        else if (isJumping && isJumpPressed && shuues && canDoubleJump)
        {
            myVelocity.y = initialJumpVelocity * .5f;
            canDoubleJump = false;
            StartCoroutine(Jumped());
        }
        else if (!isJumpPressed && onPlatform && isJumping)
        {
            isJumping = false;
        }


    }

    ///<summary>This triggers the unit to drop down if they are on a platform.</summary>
    public void Drop(InputAction.CallbackContext context)
    {
        if (onPlatform == true)
        {
            onPlatform = false;
            isGrounded = false;
            StartCoroutine(dropDown());
            myVelocity = new Vector2(_rigidBody.velocity.x, -16);
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(Interacting == false)
        {
            Interacting = true;
            
        }
    }
    


   

    #endregion
    #region Player Spells

    /// <summary> cast forth a fireball at  60 degree angle that will make a vertical wall of fire that damages passing enemies over time </summary>
    public void fireWall()
    {
        if (canCast == true)
        {
           
            if (myMana >= fireWall_prefab.GetComponent<FireWallSpellScript>().manaCost)
            {
                if (spellStone == true)
                {

                    ReduceMana((fireWall_prefab.GetComponent<FireWallSpellScript>().manaCost * 0.75f));
                }
                else
                {

                    ReduceMana(fireWall_prefab.GetComponent<FireWallSpellScript>().manaCost);
                }

                ///<summary> this spawns the fire wall spell prefab and moves it at a 60 degree angle away from the player depending on their direction</summary>   
                if (facingRight == true)
                {

                    var fireWallSpell = (GameObject)Instantiate(this.gameObject.GetComponent<Player>().fireWall_prefab, spellLocationRight.transform.position, spellLocationRight.transform.rotation);
                    fireWallSpell.GetComponent<Rigidbody>().velocity = fireWallSpell.transform.right * 12 + fireWallSpell.transform.up * -2;
                    if (fireWallSpell.GetComponent<FireWallSpellScript>().changed == true)
                    {
                        fireWallSpell.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    }
                    canCast = false;
                }
                else
                {

                    var fireWallSpell = (GameObject)Instantiate(this.gameObject.GetComponent<Player>().fireWall_prefab, spellLocationLeft.transform.position, spellLocationLeft.transform.rotation);
                    fireWallSpell.GetComponent<Rigidbody>().velocity = fireWallSpell.transform.right * -12 + fireWallSpell.transform.up * -2;

                    if (fireWallSpell.GetComponent<FireWallSpellScript>().changed == true)
                    {
                        fireWallSpell.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    }
                    canCast = false;
                }
            }
        }


    }
    #endregion
    #region Unit Melee Attacks
    /// <summary> This is the attacking function  </summary>
    public void lightAttack()
    {
        //Tyler made an edit to 1.0f y from 0.3fy
        _lightCollider.gameObject.transform.localScale = new Vector3(1f, meleeAttackRange, 1f) ;

        if (Time.time >= nextDamageEvent)
        {
            nextDamageEvent = Time.time + attackCoolDown;
            triggered = true;
            if (facingRight == true)
            {
                //10/4/21 Tyler Added this to fix the problems with sword position and rotation
                /* _lightCollider.transform.position = spellLocationRight.transform.position;
                _lightCollider.transform.position = _lightCollider.transform.position;
                
                StartCoroutine(lightAttackCoroutine());
                //Tyler commented this out to fix the problems with sword pos and rtotation
                 */

                //_lightCollider.transform.position = spellLocationRight.transform.position;
                _lightCollider.transform.position = _lightCollider.transform.position + (_lightCollider.gameObject.transform.localScale/2);
                //_lightCollider.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                _lightCollider.transform.localPosition = new Vector3(0f, 0f, 0f);
                StartCoroutine(lightAttackCoroutine());
              
            }
            else
            {
                //10/4/21 Tyler Added this to fix the problems with sword position and rotation
               /*  _lightCollider.transform.position = spellLocationLeft.transform.position;
                _lightCollider.transform.position = _lightCollider.transform.position;
                
                StartCoroutine(lightAttackCoroutine());
                //Tyler commented this out to fix the problems with sword pos and rtotation
               */

                //_lightCollider.transform.position = spellLocationLeft.transform.position;
                _lightCollider.transform.position = _lightCollider.transform.position - (_lightCollider.gameObject.transform.localScale / 2);
                //_lightCollider.transform.eulerAngles = new Vector3(180.0f, 0.0f, 90.0f);
                _lightCollider.transform.localPosition = new Vector3(0f, 0f, 0f);
                StartCoroutine(lightAttackCoroutine());
                
            }

           

        }
        
        if (triggered && animator != null)
        {
            animator.SetTrigger("lightAttack");

            triggered = false;
        }
    }

    /// <summary> This is the attacking function  </summary>

    public void heavyAttack()
    {
        //Tyler made an edit to 1.0f y from 0.3fy
        _heavyCollider.gameObject.transform.localScale = new Vector3(2.0f, meleeAttackRange, 1.0f);

        
        if (Time.time >= nextDamageEvent)
        {
            nextDamageEvent = Time.time + attackCoolDown;

            triggered = true;

            if (facingRight == true)
            {
                //10/4/21 Tyler Added this to fix the problems with sword position and rotation
              /*  _heavyCollider.transform.position = spellLocationRight.transform.position;
                _heavyCollider.transform.position = _heavyCollider.transform.position;
                _heavyCollider.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                StartCoroutine(heavyAttackCoroutine());
                //Tyler commented this out to fix the problems with sword pos and rtotation
               */ 
               // _heavyCollider.transform.position = spellLocationRight.transform.position;
                _heavyCollider.transform.position = _heavyCollider.transform.position + (_heavyCollider.gameObject.transform.localScale / 2);
                //_heavyCollider.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                _heavyCollider.transform.localPosition = new Vector3(0f, 0f, 0f); ;
                StartCoroutine(heavyAttackCoroutine());
               

            }
            else
            {
                //10/4/21 Tyler Added this to fix the problems with sword position and rotation
               /* _heavyCollider.transform.position = spellLocationLeft.transform.position;
                _heavyCollider.transform.position = _heavyCollider.transform.position;
                _heavyCollider.transform.eulerAngles = new Vector3(180.0f, 0.0f, 90.0f);
                StartCoroutine(heavyAttackCoroutine());
                //Tyler commented this out to fix the problems with sword pos and rtotation
                */
                //_heavyCollider.transform.position = spellLocationLeft.transform.position;
                _heavyCollider.transform.position = _heavyCollider.transform.position - (_heavyCollider.gameObject.transform.localScale / 2);
                //_heavyCollider.transform.eulerAngles = new Vector3(180.0f, 0.0f, 90.0f);
                _heavyCollider.transform.localPosition = new Vector3(0f, 0f, 0f);
                StartCoroutine(heavyAttackCoroutine());
               

            }


           
        }
        if(triggered)
        {
            animator.SetTrigger("heavyAttack");
          
            triggered = false;
        }


    }

    #endregion

    #region actionCheckers
    public void actionCheck1()
    {
        if(Attack1 == "Light Attack")
        {
            lightAttack();
        }
        if (Attack1 == "Heavy Attack")
        {
            heavyAttack();
        }
        if (Attack1 == "Fire Wall")
        {
            fireWall();
        }
    }
    public void actionCheck2()
    {
        if (Attack2 == "Light Attack")
        {
            lightAttack();
        }
        if (Attack2 == "Heavy Attack")
        {
            heavyAttack();
        }
        if (Attack2 == "Fire Wall")
        {
            fireWall();
        }
    }
    public void actionCheck3()
    {
        if (Attack3 == "Light Attack")
        {
            lightAttack();
        }
        if (Attack3 == "Heavy Attack")
        {
            heavyAttack();
        }
        if (Attack3 == "Fire Wall")
        {
            fireWall();
        }
    }

    #endregion

   
    
    #region Collision Detection
    public void OnTriggerStay(Collider other)
    {


        #region Camp Collisions

        if (other.GetComponent<Interactable>() != null)
        {
            Interactable localInteractRef = other.GetComponent<Interactable>();
           
            if(localInteractRef is CastleEntranceInteractScript)
            {
                   GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
                   buttonController.GetComponent<SceneButtonControllerScript>().enterCastleBTN.SetActive(true);
            }

            if (localInteractRef is MineEntranceInteractScript)
            {
                GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
                buttonController.GetComponent<SceneButtonControllerScript>().enterMineBTN.SetActive(true);
            }

            if (localInteractRef is CampShopEntranceInteractScript)
            {
                GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
                buttonController.GetComponent<SceneButtonControllerScript>().enterCampShopBTN.SetActive(true);
            }
            ///turn on button manager

            if (Interacting)
                other.GetComponent<Interactable>().Interact();
        }


       


       

        //if (other.gameObject.tag == "CampShopEntrance")
        //{
        //    GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
        //    buttonController.GetComponent<SceneButtonControllerScript>().enterCampShopBTN.SetActive(true);

        //    if (Interacting == true)
        //    {
        //        Interacting = false;
        //        canMove = false;
        //        other.GetComponent<CampShopEntranceInteractScript>().Interact();

        //    }
        //}

        #endregion

        #region Enemy Collisions
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Interact();
           
        }

        if (other.gameObject.tag == "Trap")
        {
            other.GetComponent<TrapScript>().Interact();

        }
        #endregion

        #region ground/platform/camp collisions

        if (other.gameObject.tag =="ground")
        {
            _groundCollider.enabled = true;
        }
        
    }

   public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "platform")
        {
            _platformCollider.enabled = true;
        }
 
        if (other.gameObject.tag == "MineEntrance")
        {
            GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
            buttonController.GetComponent<SceneButtonControllerScript>().enterMineBTN.SetActive(false);
        }

        if (other.gameObject.tag == "CastleEntrance")
        {
            GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
            buttonController.GetComponent<SceneButtonControllerScript>().enterCastleBTN.SetActive(false);
        }

        if (other.gameObject.tag == "CampShopEntrance")
        {
            GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
            buttonController.GetComponent<SceneButtonControllerScript>().enterCampShopBTN.SetActive(false);
        }
    }
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("testing");
        }
    }


    #endregion



    /// <param name="mana">Amount of mana lost by the player for casting a spell</param>
    public void ReduceMana(float mana)
    {
        myMana -= mana;
    }


    public IEnumerator InteractCoroutine()
    {
       
            yield return new WaitForSeconds(.01f);
            if (Interacting == true)
            {
                Interacting = false;
            }
      
        
       
        
    }

    public IEnumerator Jumped()
    {
        justJumped = true;
        yield return new WaitForSeconds(.5f);
        justJumped = false;
        if(hasDoubleJump == false)
        {
            canDoubleJump = true;
            hasDoubleJump = true;
        }
        

    }

  

    public IEnumerator IFrames()
    {
        float startTime = Time.time;
        float waitTime = 0.125f;

        MeshRenderer render = GetComponent<MeshRenderer>();

        while (true)
        {
            ///Turn on 50% opacityy
            Color origMat = render.material.color;
            origMat.a = 0.5f;
            render.material.color = origMat;
            yield return new WaitForSeconds(waitTime);
            origMat.a = 1f;
            render.material.color = origMat;
            yield return new WaitForSeconds(waitTime);

            ///wait 0.125 seconds
            ///turn opacity back to 100%

            yield return new WaitForFixedUpdate();
            if (Time.time - startTime >= 1f)
                break;
        }



        invulnerable = false;
    }

    /// <summary> this allows units to drop through platforms </summary>
    public IEnumerator dropDown()
    {
        
        _platformCollider.enabled = false;
        _groundCollider.enabled = false;
        onPlatform = false;
        isGrounded = false;
        isFalling = true;
        
      
        yield return new WaitForSeconds(2f);
        _groundCollider.enabled = true;
        _platformCollider.enabled = true;
    }

}