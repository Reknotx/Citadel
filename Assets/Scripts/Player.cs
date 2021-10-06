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

using Interactables;

public class Player : Unit
{
    /// <summary>
    /// sidhajshdkashdi
    /// </summary>

    public static Player Instance;
    #region Player Stats

            #region Player's Base Stats/Important controls

    ///<summary>This is the units health.</summary>
    public float myHealth;

    ///<summary>This is the maximum units health.</summary>
   // public float maxHealth; //

    public override float Health 
    { 
        get => base.Health; 
        set
        {
            base.Health = Mathf.Clamp(value, 0, maxHealth);
            if (base.Health <= 0)
            {
                if (undying == true)
                {
                    undying = false;
                    myHealth = Mathf.Round(maxHealth * 0.15f);
                }
                else
                {
                    ResetGame();
                }

            }
            else
            {
                invulnerable = true;
                StartCoroutine(IFrames());
            }
        }
    }

    ///<summary>This is the  units starting health.</summary>
    public float startingHealth;

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
    private float _Reach = 2f;

    ///<summary>This tracks what the ground detection raycast hits.</summary>
    RaycastHit hit;

    ///<summary>This tracks what direction the player is facing.</summary>
    //[HideInInspector]
    public bool facingRightLocal;

            #endregion
            #region Player's Attack Stats/Spell Prefabs

    ///<summary>This determines how far the player will knock back an enemy with the heavy attack.</summary>
    public float knockbackForce;

    ///<summary>This determines the range of the player's melee attack.</summary>
    public float meleeAttackRange = 1f; //

    ///<summary>This determines the damage of the player's melee attack.</summary>
    public int meleeAttackDamage; //

    ///<summary>This determines the damage the player deals to an enemy when they collide.</summary>
    public int playerCollisionDamage;

    /// <summary>this is the physical gameobject that is cast during the firewall spell</summary>
    public GameObject fireWall_prefab;
    #endregion
            #region Bool Determinates 

    /// <summary> determines if the player can move or not </summary>
        [HideInInspector]
        public bool canMove = true;

    /// <summary> determines if the player can jump once more in the air or not </summary>
    //[HideInInspector]
    public bool canDoubleJump = true;

    /// <summary> determines if the player can jump once more in the air or not </summary>
    //[HideInInspector]
    public bool hasDoubleJump = false;

    public bool invulnerable = false;

    /// <summary> determines if the player is trying to interact with things or not </summary>
   // [HideInInspector]
    public bool Interacting = false;
   // {
       // return playerInputActions.PlayerControl.
    //}

    [HideInInspector]
    public bool canInteract = false;


    /// <summary> this keeps track of if the player is in the camp shop or not  </summary>
    public bool inCampShop = false;

    /// <summary> this keeps track of if the player is in the mine  or not  </summary>
    public bool inMine = false;

    /// <summary> this keeps track of if the player is in the mine shop or not  </summary>
    public bool inMineShop = false;

    public bool grounded;

    public bool isRunning = false;
    public bool isAttacking = false;

    #endregion
            #region Bool Equipment

    public bool shuues = false;
    public bool undying = false;
    public bool spellStone = false;
    public bool backShield = false;
    #endregion
            #region Animations
    public Animator animator;
    private bool triggered = false;
    private float animationFinishTime = .5f;

            #endregion



    #endregion


    private void Awake()
    {

      if(Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        
        #region Player Movement Important Connectors
         ///<summary>The following is used to track player inputs and controls.</summary>
         playerInputActions = new PlayerInputActions();
         playerInputActions.PlayerControl.Enable();
         playerInputActions.PlayerControl.Jump.performed += Jump;
         //playerInputActions.PlayerControl.Movement.performed += movement;
         playerInputActions.PlayerControl.Drop.performed += Drop;
         

        


        #endregion


    }


    public override void Update()
    {
        Application.targetFrameRate = 60;
        facingRightLocal = facingRight;
        base.Update();

        #region Player Stat controls
        

        if (myMana >= maxMana)
        {
            myMana = maxMana;
        }

        if (Interacting == true)
        {
            StartCoroutine(InteractCoroutine());
        }

        #endregion

        #region Player Movement Detection
        ///<summary>This moves the player constantly while the input is held.</summary>
        
        //Tyler Added Code
        if (isAttacking)
        {
            canMove = false;
        }
        //End Tyler Code
        if (canMove == true)
        {
            
            Vector2 inputVector = playerInputActions.PlayerControl.Movement.ReadValue<Vector2>();
            _rigidBody.MovePosition(transform.position + new Vector3(inputVector.x, 0, 0) * speed * Time.deltaTime);
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            
            if (animator != null)
                animator.SetBool("isRunning", isRunning);
            
        }
        else
        {
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
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
            //_rigidBody.AddForce(Vector3.up * .03f, ForceMode.Impulse);
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

    public override void TakeDamage(int amount)
    {
        if (invulnerable)
            return;

        base.TakeDamage(amount);
    }


    #region Player Movement Actions
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

    ///<summary>This triggers the unit to jump up.</summary>
    public void Jump(InputAction.CallbackContext context)
    {
        if (this != null)
        {
            if (isGrounded == true )
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

    ///<summary>This triggers the unit to drop down if they are on a platform.</summary>
    public void Drop(InputAction.CallbackContext context)
    {
        if (onPlatform == true)
        {
            StartCoroutine(dropDown());
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
    #endregion
    #region Unit Melee Attacks
    /// <summary> This is the attacking function  </summary>
    public void lightAttack(InputAction.CallbackContext context)
    {
        //Tyler made an edit to 1.0f y from 0.3fy
        _lightCollider.gameObject.transform.localScale = new Vector3(1.0f,meleeAttackRange, 1.0f);

        if (Time.time >= nextDamageEvent)
        {
            nextDamageEvent = Time.time + (attackCoolDown/2);
            triggered = true;
            if (facingRight == true)
            {
                //10/4/21 Tyler Added this to fix the problems with sword position and rotation
                /* _lightCollider.transform.position = spellLocationRight.transform.position;
                _lightCollider.transform.position = _lightCollider.transform.position;
                
                StartCoroutine(lightAttackCoroutine());
                //Tyler commented this out to fix the problems with sword pos and rtotation
                 */
                _lightCollider.transform.position = spellLocationRight.transform.position;
                _lightCollider.transform.position = _lightCollider.transform.position + (_lightCollider.gameObject.transform.localScale/2);
                _lightCollider.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                _lightCollider.transform.localPosition = new Vector3(_lightCollider.transform.localPosition.x, 0f, _lightCollider.transform.localPosition.z);
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
                _lightCollider.transform.position = spellLocationLeft.transform.position;
                _lightCollider.transform.position = _lightCollider.transform.position - (_lightCollider.gameObject.transform.localScale / 2);
                _lightCollider.transform.eulerAngles = new Vector3(180.0f, 0.0f, 90.0f);
                _lightCollider.transform.localPosition = new Vector3(_lightCollider.transform.localPosition.x, 0f, _lightCollider.transform.localPosition.z);
                StartCoroutine(lightAttackCoroutine());
                
            }

           

        }
             if (triggered)
             {
                animator.SetTrigger("lightAttack");

                triggered = false;
             }
    }

    /// <summary> This is the attacking function  </summary>

    public void heavyAttack(InputAction.CallbackContext context)
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
                _heavyCollider.transform.position = spellLocationRight.transform.position;
                _heavyCollider.transform.position = _heavyCollider.transform.position + (_heavyCollider.gameObject.transform.localScale / 2);
                _heavyCollider.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                _heavyCollider.transform.localPosition = new Vector3(_heavyCollider.transform.localPosition.x, 0f, _heavyCollider.transform.localPosition.z);
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
                _heavyCollider.transform.position = spellLocationLeft.transform.position;
                _heavyCollider.transform.position = _heavyCollider.transform.position - (_heavyCollider.gameObject.transform.localScale / 2);
                _heavyCollider.transform.eulerAngles = new Vector3(180.0f, 0.0f, 90.0f);
                _heavyCollider.transform.localPosition = new Vector3(_heavyCollider.transform.localPosition.x, 0f, _heavyCollider.transform.localPosition.z);
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


    #region Collision Detection
    public void OnTriggerStay(Collider other)
    {


        #region Camp Collisions

        if (other.GetComponent<Interactable>() != null)
        {
            other.GetComponent<Interactable>().Interact();
        }   


        //if(other.gameObject.tag == "MineEntrance")
        //{
        //    GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
        //    buttonController.GetComponent<SceneButtonControllerScript>().enterMineBTN.SetActive(true);
        //    if (Interacting == true)
        //    {
        //        Interacting = false;
        //        other.GetComponent<MineEntranceInteractScript>().Interact();

        //    }
        //}


        //if (other.gameObject.tag == "CastleEntrance")
        //{
        //    GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
        //    buttonController.GetComponent<SceneButtonControllerScript>().enterCastleBTN.SetActive(true);

        //    if (Interacting == true)
        //    {
        //        Interacting = false;
        //        other.GetComponent<CastleEntranceInteractScript>().Interact();

        //    }
        //}

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
            yield return new WaitForSeconds(0.125f);

            ///wait 0.125 seconds
            ///turn opacity back to 100%

            yield return new WaitForFixedUpdate();
            if (Time.time - startTime >= 1f)
                break;
        }



        invulnerable = false;
    }

}