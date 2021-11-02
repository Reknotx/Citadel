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

public class Player : Unit, IDamageable
{

    public static Player Instance;
    #region Player Stats

            #region Player's Base Stats/Important controls
    [Header ("player base stats")]

    ///<summary>This is the players tracked health.</summary>
    public float myHealth;

    ///<summary>This is the units actual health.</summary>
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

    ///<summary> the players calculated health for the health bar </summary>
    [HideInInspector]
    private float calculateHealth;

    ///<summary>  the players calculated mana for the mana bar </summary>
    [HideInInspector]
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

      ///<summary> the players current velocity</summary>
    [HideInInspector]
    public Vector3 myVelocity;


            #endregion
            #region player jumping stats and controls
    [Header("player improved jumping ")]

    ///<summary>the players gravity value while in the air </summary>
    [HideInInspector]
    float gravity = -9.8f;

    ///<summary> the players gravity value while on the ground</summary>
     [HideInInspector]
    float groundedGravity = -.05f;

    ///<summary> determines if the player is moving up in their jump</summary>
    [HideInInspector]
    public bool isJumping = false;

    ///<summary> determines if the player is holding the jump key down</summary>
    [HideInInspector]
    public bool isJumpPressed = false;

    ///<summary> determines if the player is holding the jump key down</summary>
    //[HideInInspector]
    public bool isDropPressed = false;

    ///<summary> the initial velocity applied to the player when they jump</summary>
    //[HideInInspector]
    public float initialJumpVelocity;

    ///<summary> the maximum height the player can jump</summary>
    //[HideInInspector]
    public float maxJumpHeight;

    ///<summary> the maximum time the player will be in the air during the jump</summary>
   // [HideInInspector]
    public float maxJumpTime;



    #endregion
            #region Player's Ground/Directional Detection Stats
    [Header("unit colliders/ground detection")]


    ///<summary>This is the unit's collider that detects the ground.</summary>
    public Collider _wallCollider;

    ///<summary>This is the unit's collider that detects the ground.</summary>
    [SerializeField]
    protected Collider _hitboxCollider;

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


    /// <summary>this is the physical gameobject that is cast during the firewall spell</summary>
    public GameObject fireWall_prefab;

    /// <summary>this is the physical gameobject that is cast during the icicle spell</summary>
    public GameObject icicle_prefab;

    /// <summary>this is the physical gameobject that is cast during the aerorang spell</summary>
    public GameObject Aerorang_prefab;

    ///<summary>This is the location spells will be cast on the left side of the unit.</summary>
    [SerializeField]
    protected GameObject spellLocationLeft;

    ///<summary>this is the location spells will cast on the right side of the unit.</summary>
    [SerializeField]
    protected GameObject spellLocationRight;

    ///<summary>This is the location spell will be cast from the center of the unit.</summary>
    [SerializeField]
    protected GameObject spellLocationCenter;

    ///<summary>This dis the units collider for their heavy attack.</summary>
    [SerializeField]
    protected Collider _heavyCollider;

    [Header("player attack names")]
    public string Attack1;
    public string Attack2;
    public string Attack3;

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

    public bool dropping = false;

    public bool hittingWallRight = false;
    public bool hittingWallLeft = false;

    [HideInInspector]
    public bool isRunning = false;

   [HideInInspector]
    public bool isFalling = false;

    [HideInInspector]
    public bool isAttacking = false;

    [HideInInspector]
    public bool dmgPlayerByTick = false;

    [HideInInspector]
    public bool shieldActive;

    [HideInInspector]
    public bool usingPotion = false;
    
    public bool canPass = true;

    

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

    [HideInInspector]
    public int potionMax = 2;

    [HideInInspector]
    public int healthPotionMax = 2;

    [HideInInspector]
    public int manaPotionMax = 2;

    #endregion
            #region Animations
    [Header("player animations")]
    public Animator animator;
    private bool triggered = false;
    private float animationFinishTime = .5f;

    public bool isCastingIcicle;

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
      


        base.Update();




        #region Player Stat/Item controls
        Application.targetFrameRate = 60;
        facingRightLocal = facingRight;
        _rigidBody.velocity = myVelocity;
        if (dmgPlayerByTick)
        {
            dmgPlayerByTick = false;
            TakeDamage(1);
        }

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

        ///<summary>this sets the rate for how quickly players can cast spells </summary>
        spellCastDelay -= Time.deltaTime * spellCastRate;
        if (spellCastDelay <= 0)
        {
            canCast = true;
            spellCastDelay = 3f;
        }

        if (floatingShield)
        {
            flotingShieldObj.SetActive(true);
        }
        if (medicineStash == true)
        {
            manaPotionMax = 3;
            healthPotionMax = 3;
            potionMax = 4;
        }

        #endregion

        #region Player Movement Detection
        ///<summary>This moves the player constantly while the input is held.</summary>
      if(!hittingWallLeft || !hittingWallRight)
      {

        if (canMove == true)
        {
            if(isRunning && !hittingWallLeft && !hittingWallRight)
            {
                    Vector2 inputVector = playerInputActions.PlayerControl.Movement.ReadValue<Vector2>();
                    _rigidBody.MovePosition(transform.position + new Vector3(inputVector.x, 0, 0) * speed * Time.deltaTime);
                    _rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            }
               
                
           


              if (animator != null)
              {
                    animator.SetBool("isRunning", isRunning);
                    animator.SetBool("isJumping", isJumping);
                    animator.SetBool("isFalling", isFalling);
                    animator.SetBool("isGrounded", isGrounded);
                    animator.SetBool("Icicle", isCastingIcicle);
              }
                
        }
      }
        else
        {
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        }

        ///standing on a platform is the same as standing on the ground 
        if (onPlatform == true && !isJumpPressed)
        {
            isGrounded = true;
        }
        grounded = isGrounded;
        ///when you are back on the ground, it resets whether or not you can double jump or not is you have the shuues
        if(isGrounded == true)
        {
            canDoubleJump = true;
            hasDoubleJump = false;
        }

        if(hittingWallLeft && isJumpPressed)
        {
            isGrounded = false;
        }
        else if(hittingWallLeft && !isJumpPressed)
        {
            isFalling = true;
        }

        if (hittingWallRight && isJumpPressed)
        {
            isGrounded = false;
        }
        else if (hittingWallRight && !isJumpPressed)
        {
            isFalling = true;
        }

        if(isGrounded)
        {
            hittingWallLeft = false;
            hittingWallRight = false;
        }

        if (facingRight)
        {
            hittingWallLeft = false;
        }
        else
        {
            hittingWallRight = false;
        }

        if(isCastingIcicle)
        {
            StartCoroutine(IcicleCoroutine());
        }
       


        



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
        if (Physics.Raycast(transform.position, groundCheck, out hit, _Reach) && hit.transform.tag == "ground" && !isJumpPressed)
        {
            isGrounded = true;
            isJumping = false;
        }
        else
        {
            isGrounded = false;
        }

       
        ///<summary>This determines whether the unit is on the ground or not.</summary>
        var wallCheck = transform.TransformDirection(Vector3.right);
        Debug.DrawRay(transform.position, wallCheck * _Reach/2, Color.red);
        if (Physics.Raycast(transform.position, wallCheck, out hit, _Reach/2) && hit.transform.tag == "ground")
        {
            if(facingRight && !isGrounded)
            {
                hittingWallRight = true;
            }
            else
            {
               hittingWallRight = false;
            }
            
        
        
        }
        else if(isGrounded)
        {
            hittingWallRight = false;
        }

        var wallCheck2 = transform.TransformDirection(Vector3.left);
        Debug.DrawRay(transform.position, wallCheck2 * _Reach/2, Color.red);
        if (Physics.Raycast(transform.position, wallCheck2, out hit, _Reach/2) && hit.transform.tag == "ground")
        {
           if(!facingRight && !isGrounded)
           {
                hittingWallLeft = true;
           }
           else
           {
              hittingWallLeft = false;
           }
       
        
        }
        else if (isGrounded)
        {
            hittingWallLeft = false;
        }



        ///<summary>This determines whether the unit is trying to jump up through a platform or not.</summary>
        var roofCheck = transform.TransformDirection(Vector3.up);
        Debug.DrawRay(transform.position, roofCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, roofCheck, out hit, _Reach) && hit.transform.tag == "platform")
        {
            throughPlatform = true;
            //hit.transform.gameObject.GetComponent<PlatformColliderControllerScript>().isPassing = true;

        }
        else
        {
            throughPlatform = false;
            

        }


        ///<summary>this checks if the unit is trying to pass up through a platform and will assist.</summary>
        if (throughPlatform == true && justJumped == true && !isGrounded)
        {
            //StartCoroutine(dropDown());
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 12);
            
        }


       


        #endregion

        
    }

    #region health and mana bar functionality

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

    #endregion


    #region health/mana reduction + reset methods

    /// <summary>
    /// resets the players soft gold and returns them to the camp
    /// </summary>
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

    /// <summary>
    /// reduces player health and makes them invulnerable for a short time 
    /// </summary>
    /// <param name="amount"> the amount of damage delt </param>
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


    /// <param name="mana">Amount of mana lost by the player for casting a spell</param>
    public void ReduceMana(float mana)
    {
        myMana -= mana;
    }

    #endregion

    #region Player Movement Actions
   
    public void movement2(InputAction.CallbackContext context)
    {
      if (!hittingWallRight || !hittingWallLeft)
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
    }

    
    public void Jump2(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
         float jumpTimeFrame = Time.deltaTime + maxJumpTime;
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
            isJumping = false;
            myVelocity.y = groundedGravity;
        }
        else if(isFalling)
        {
            isJumping = false;
            float previousYVelocity = myVelocity.y;
            float newYVelocity = myVelocity.y + (gravity * fallMultiplier*Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * .5f, -10.0f);
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
        isDropPressed = context.ReadValueAsButton();
        
        
        

    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(Interacting == false)
        {
            Interacting = true;
            
        }
    }



    public void Escape(InputAction.CallbackContext context)
    {

        return;


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

    public void icicle()
    {
        if (canCast == true && myMana >= 10)
        {
            isCastingIcicle = true;
            if (spellStone == true)
            {

                ReduceMana(7);
            }
            else
            {

                ReduceMana(10);
            }

            ///<summary> this spawns the fire wall spell prefab and moves it at a 60 degree angle away from the player depending on their direction</summary>   
            if (facingRight == true)
            {

                var icicleSpell = (GameObject)Instantiate(this.gameObject.GetComponent<Player>().icicle_prefab, spellLocationRight.transform.position, spellLocationRight.transform.rotation);
                icicleSpell.GetComponent<Rigidbody>().velocity = icicleSpell.transform.right * 12;
                  
                canCast = false;
            }
            else
            {

                var icicleSpell = (GameObject)Instantiate(this.gameObject.GetComponent<Player>().icicle_prefab, spellLocationLeft.transform.position, spellLocationLeft.transform.rotation);
                icicleSpell.GetComponent<Rigidbody>().velocity = icicleSpell.transform.right * -12;
                   
                canCast = false;
            }
        }
        

    }



    public void aerorang()
    {
        if (canCast == true && myMana >= 10)
        {
            
            if (spellStone == true)
            {

                ReduceMana(7);
            }
            else
            {

                ReduceMana(10);
            }

            ///<summary> this spawns the fire wall spell prefab and moves it at a 60 degree angle away from the player depending on their direction</summary>   
            if (facingRight == true)
            {

                var Aerorang = (GameObject)Instantiate(this.gameObject.GetComponent<Player>().Aerorang_prefab, spellLocationRight.transform.position, spellLocationRight.transform.rotation);
                //Aerorang.GetComponent<Rigidbody>().velocity = Aerorang.transform.right * 12;
                

                canCast = false;
            }
            else
            {

                var Aerorang = (GameObject)Instantiate(this.gameObject.GetComponent<Player>().Aerorang_prefab, spellLocationLeft.transform.position, spellLocationLeft.transform.rotation);
               // Aerorang.GetComponent<Rigidbody>().velocity = Aerorang.transform.right * -12;

                canCast = false;
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
            nextDamageEvent = Time.time + attackCoolDown/2;
            triggered = true;
            if (facingRight == true)
            {
                
                _lightCollider.transform.position = _lightCollider.transform.position + (_lightCollider.gameObject.transform.localScale/2);
                
                _lightCollider.transform.localPosition = new Vector3(0f, 0f, 0f);
                StartCoroutine(lightAttackCoroutine());
              
            }
            else
            {
               
                _lightCollider.transform.position = _lightCollider.transform.position - (_lightCollider.gameObject.transform.localScale / 2);
                
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
               
                _heavyCollider.transform.position = _heavyCollider.transform.position + (_heavyCollider.gameObject.transform.localScale / 2);
                
                _heavyCollider.transform.localPosition = new Vector3(0f, 0f, 0f); ;
                StartCoroutine(heavyAttackCoroutine());
               

            }
            else
            {
                
                _heavyCollider.transform.position = _heavyCollider.transform.position - (_heavyCollider.gameObject.transform.localScale / 2);
                
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
    #region Player using potions

    public void useHealthPotion()
    {
        if(Health < maxHealth)
        {
            if (healthPotions > 0)
            {
                if (!usingPotion)
                {
                    usingPotion = true;
                    healthPotions--;
                    Health = Health + (maxHealth * 0.4f);
                    StartCoroutine(usePotionCoroutine());
                }

            }
        }
        
    }

    public void useManaPotion()
    {
        if(myMana < maxMana)
        {
            if (manaPotions > 0)
            {
                if (!usingPotion)
                {
                    usingPotion = true;
                    manaPotions--;
                    myMana = myMana + (maxMana * 0.4f);
                    StartCoroutine(usePotionCoroutine());
                }

            }
        }
        
    }

    public void restockPotions()
    {
       if(medicineStash)
        {
            healthPotions = 1;
            manaPotions = 1;
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
        else if (Attack1 == "Heavy Attack")
        {
            heavyAttack();
        }
        else if (Attack1 == "Fire Wall")
        {
            fireWall();
        }
        else if (Attack1 == "Icicle")
        {
            icicle();
        }
        else if(Attack1 == "Aerorang")
        {
            aerorang();
        }
        else
        {
            lightAttack();
        }
    }
    public void actionCheck2()
    {
        if (Attack2 == "Light Attack")
        {
            lightAttack();
        }
        else if (Attack2 == "Heavy Attack")
        {
            heavyAttack();
        }
        else if (Attack2 == "Fire Wall")
        {
            fireWall();
        }
        else if (Attack2 == "Icicle")
        {
            icicle();
        }
        else if (Attack2 == "Aerorang")
        {
            aerorang();
        }
        else
        {
            lightAttack();
        }
    }
    public void actionCheck3()
    {
        if (Attack3 == "Light Attack")
        {
            lightAttack();
        }
        else if (Attack3 == "Heavy Attack")
        {
            heavyAttack();
        }
        else if (Attack3 == "Fire Wall")
        {
            fireWall();
        }
        else if (Attack3 == "Icicle")
        {
            icicle();
        }
        else if (Attack3 == "Aerorang")
        {
            aerorang();
        }
        else
        {
            lightAttack();
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



  


    #region player IEnumerators
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

    public IEnumerator usePotionCoroutine()
    {
       
        yield return new WaitForSeconds(.3f);
        if (usingPotion)
            usingPotion = false;
      


    }

    /// <summary> this allows the weapons collider to interact with things </summary>
    public IEnumerator heavyAttackCoroutine()
    {
        _heavyCollider.enabled = true;
        yield return new WaitForSeconds(.9f);
        _heavyCollider.enabled = false;
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
    

    public IEnumerator InvicibilityFrames()
    {
        _hitboxCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        _hitboxCollider.enabled = true;
    }


    public IEnumerator PassThroughCoroutine()
    {
        //canPass = false;
        yield return new WaitForSeconds(.5f);
        
        canPass = true;
    }

    public IEnumerator IcicleCoroutine()
    {
        
        yield return new WaitForSeconds(.3f);

        isCastingIcicle = false ;
    }
    #endregion
}