/*
 * Author: Chase O'Connor
 * Date: 11/10/2021.
 * 
 * Brief: A new and improved player controller for the game.
 * 
 * 
 */

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Interactables;
using Menu;
using CombatSystem;
using JetBrains.Annotations;
using UnityEngine.Serialization;


public class NewPlayer : Unit, IDamageable
{
    public Renderer bodyRenderer;
    
    #region Fields



    #endregion
    /// <summary> The static instance of the New Player in the scene. </summary>
    public static NewPlayer Instance;

    [HideInInspector]
    public Rigidbody playerRB;

    [HideInInspector]
    public Vector2 moveDir;
    
    [Tooltip("The amount of units the player will jump.")]
    public float jumpHeight = 5f;

    public GameObject physicalBody;
    
    bool checkForSolidSurface = true;
    
    public LayerMask groundLayerMask;
    public LayerMask platformLayerMask;
    
    private int playerLayer = 7;
    private int ignorePlayerLayer = 12;

    public PlayerCombatSystem combatSystem;


    public PlayerInventory inventory;

    [SerializeField]
    private UI_Inventory uiInventory;

    private InventoryDisplay inventoryDisplay;

    private Interactable currentInteractableItem;

    [NotNull] public Slider ManaBar;
    private bool falling;

    [Tooltip("The height of the model for collision detection purposes.")]
    public float modelHeight = 1.5f;

    [HideInInspector]
    public bool isPaused;

    /// <summary> The player's health. </summary>
    public override float Health
    {
        get => base.Health;

        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            if (HealthBar != null) HealthBar.value = value;
            if (Health == 0)
            {
                //Activate game over logic
                //after game over logic it is better to turn off the player 
                //object rather than destroy it
                physicalBody.transform.GetChild(0).gameObject.SetActive(false);
                enabled = false;
                GameOver.Instance.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
    
    private int _mana;
    [SerializeField]
    private int maxMana;

    /// <summary> The player's mana. </summary>
    public int Mana
    {
        get => _mana;

        set
        {
            _mana = value;
            if (ManaBar != null)
            {
                ManaBar.value = value;
                ManaBar.GetComponentInChildren<Text>().text = value.ToString(); 
            }

            combatSystem.spellSystem.UpdateSpellSystemUI(_mana);
        }
    }

    public int MaxMana
    {
        get => maxMana;
        set => maxMana = value;
    }

    /// <summary> Returns the position of the center of the player prefab. Affected by model height. </summary>
    public Vector3 Center
    {
        get
        {
            Vector3 temp = transform.position;
            return new Vector3(temp.x, temp.y + (modelHeight / 2));
        }
    }

    public override void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;
        playerRB = GetComponent<Rigidbody>();
        inventory = new PlayerInventory();

        ManaBar.maxValue = MaxMana;
        Mana = MaxMana;

        uiInventory.SetInventory(inventoryDisplay);

        base.Awake();
    }

    public override void Start()
    {
        combatSystem = PlayerCombatSystem.Instance;
    }

    public override void Update()
    {
        if (isPaused) return;

        Move();

        GroundedCheck();

    }



    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && other.GetComponent<Interactable>() != null)
        {
            currentInteractableItem = other.GetComponent<Interactable>();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6 && other.GetComponent<Interactable>() != null)
        {
            currentInteractableItem = null;
        }
    }

    /// <summary> Moves the player with forces and puts a limit on our maximum velocity. </summary>
    public void Move()
    {
        //Player needs to perform a check to see if it's about to collide
        //with a wall if it moves in the proposed direction.
        //If it does we want to cancel the request for forces to be added
        //and to stop the player as well, this might be as simple as 
        //setting the moveDir equal to zero so that the below if statement
        //fires off and does the job for us
        Vector3 playersCurrVel = playerRB.velocity;
        if (moveDir != Vector2.zero && CheckForWalls())
        {
            Debug.Log("Collided with wall");
            playerRB.velocity = new Vector3(0, playersCurrVel.y);
        }
        else if (moveDir == Vector2.zero)
        {
            //Get the player's velocity.
            //use the velocity to calculate how much force in the opposite 
            //direction we need to put on the player to stop it faster
            Vector3 stoppingForce = new Vector3(playersCurrVel.x * -1, 0f);
            playerRB.AddForce(stoppingForce);
        }
        else playerRB.AddForce(moveDir * 20);

        //when move dir is 0 in the x i need to apply a force in the opposite direction
        //to stop the player from moving so that drag can remain at 0

        playerRB.velocity = new Vector3(Mathf.Clamp(playerRB.velocity.x, -speed, speed), playerRB.velocity.y);

        //Internal helper function to check for walls or terrain in front of the player.
        bool CheckForWalls()
        {
            LayerMask layerMask;
            if (physicalBody.layer == ignorePlayerLayer)
            {
                layerMask = groundLayerMask;
            }
            else layerMask = groundLayerMask | platformLayerMask;


            //Here I'll want to do a physics cast instead of a ray cast
            //so that I get all of the information easily and without trial 
            //and error 
            return Physics.BoxCast(new Vector3(transform.position.x, transform.position.y + (modelHeight / 2), transform.position.z),
                                   new Vector3(0.1f, modelHeight / 2 - 0.05f, 0.5f),
                                   moveDir,
                                   Quaternion.identity,
                                   0.3f,
                                   layerMask); ;
        }
    }

    private void GroundedCheck()
    {
        if (grounded) return;

        if (checkForSolidSurface && (CheckForPlatform() || CheckIfGrounded()))
        {
            physicalBody.layer = playerLayer;
            grounded = true;
            falling = false;
            Debug.Log("Landed");
            PlayerAnimationManager.Instance.ActivateTrigger(PlayerAnimationManager.TriggerAnimations.LANDING);
        }
        else if (playerRB.velocity.y < 0)
        {
            falling = true;
        }
        PlayerAnimationManager.Instance.SetBool(PlayerAnimationManager.BoolAnimations.FALLING, falling);

        bool CheckIfGrounded()
        {
            return Physics.CheckBox(transform.position,
                                    new Vector3(0.1f, 0.05f, 0.5f),
                                    Quaternion.identity,
                                    groundLayerMask);
        }

        bool CheckForPlatform()
        {
            return Physics.CheckBox(transform.position,
                                    new Vector3(0.1f, 0.05f, 0.5f),
                                    Quaternion.identity,
                                    platformLayerMask);
        }
    }

    public void OnMove(InputValue value)
    {
        if (isPaused) return;
        //Use forces to move the player in the desired direction instead
        //use the technique that Brackey's utilized in the Ball wars video
        //Limit the velocity the player can have in the x direction only 
        moveDir = value.Get<Vector2>();
        Debug.Log("OnMove");
        if (Keyboard.current.dKey.isPressed)
        {
            physicalBody.transform.eulerAngles = new Vector3(0f, 90f, 0f);
            facingRight = true;
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            physicalBody.transform.eulerAngles = new Vector3(0f, 270f, 0f);
            facingRight = false;
        }

        PlayerAnimationManager.Instance.SetBool(PlayerAnimationManager.BoolAnimations.RUNNING, moveDir != Vector2.zero);
        PlayerAnimationManager.Instance.SetBool(PlayerAnimationManager.BoolAnimations.IDLE, moveDir == Vector2.zero);
    }

    public void OnJump()
    {
        if (!grounded || isPaused) return;
        LeftGround();
        Debug.Log("Jump");
        playerRB.velocity = new Vector3(0, Mathf.Sqrt(-2.0f * Physics.gravity.y * jumpHeight));
        PlayerAnimationManager.Instance.ActivateTrigger(PlayerAnimationManager.TriggerAnimations.JUMP);
    }

    public void OnDropDown()
    {
        if (!grounded || isPaused) return;
        PlayerAnimationManager.Instance.SetBool(PlayerAnimationManager.BoolAnimations.FALLING, true);
        LeftGround();
        falling = true;
    }

    public void OnInteract()
    {
        if (currentInteractableItem != null && !isPaused) currentInteractableItem.Interact();
    }

    public void OnPause()
    {
        PauseMenu.Instance.gameObject.SetActive(!PauseMenu.Instance.gameObject.activeSelf);
        isPaused = PauseMenu.Instance.gameObject.activeSelf;
        combatSystem.spellSystem.spellBook.gameObject.SetActive(false);
    }

    public void OnUsePotion()
    {
        if (Keyboard.current.fKey.isPressed)
            inventory.UseHealthPotion();
        else
            inventory.UseManaPotion();
    }

    private void LeftGround()
    {
        physicalBody.layer = ignorePlayerLayer;
        grounded = false;
        checkForSolidSurface = false;
        StartCoroutine(GroundedCheckDelay());

    }

    IEnumerator GroundedCheckDelay()
    {
        yield return new WaitForSeconds(0.6f);
        checkForSolidSurface = true;
    }

    public override void TakeDamage(float amount)
    {
        if (invulnerable) return;

        base.TakeDamage(amount);
        invulnerable = true;
        StartCoroutine(IFrames());

    }

    void Attack()
    {
        //How can we process the attack to make it more dynamic?
        //Ideas I need ideas
        //Having a secondary class that processes a string
        //sent in via the input system that then returns a 
        //stored object which is used to instantiate spells
        //
        //However, we can make it simpler by setting up harder references to
        //sword attacks versus spells
        //A spell system object that's an empty gameobject a child of the player
        //game object
        //
        //A player Combat system script is going to be the best course of action here
        //The player combat system script will take in string inputs and then
        //use that string information to make the appropriate calls to other 
        //combat class scripts for melee attacks and spell attacks 
        //the melee attack script can trigger the animations and turn on the spell
        //while the spell attack script can spawn in spells and fire them 
        //away from the player
        //
        //The possible way to have it work with spells and the sword is to change
        //the combat system entirely from what Tyler wants and allow for the player 
        //to cast spells and swing the sword at the same time.
        //The melee combat system script will register if we need to swing
        //the light attack or the heavy attack
        //
        //The spell combat sysstem script will hold all of the spells and allow
        //the player to select two or three spells they can fire off by pressing
        //the number keys and then spawns the spells fired off into the direction
        //the player is facing
        //the player can right click and select the number slot the spell will be in
        //The spell book will have it's own special UI so that the player can
        //examine the spells and what damage they will do and what special
        //effects they have
        //
        //This will help make things more dynamic for us and allow for
        //as many spells as want for the player to have.x
    }

    private bool invulnerable;

    [Tooltip("Indicates how long the invulnerability frames last for.")]
    public float iFrameDuration = 3f;
    public IEnumerator IFrames()
    {
        float startTime = Time.time;
        float blinkTime = 0.25f;
        
        while (true)
        {
            //Turn on 50% opacity
            Color origColor = bodyRenderer.material.color;
            origColor.a = 0.25f;
            
            bodyRenderer.material.color = origColor;
            yield return new WaitForSeconds(blinkTime);
            
            origColor.a = 1f;
            bodyRenderer.material.color = origColor;
            yield return new WaitForSeconds(blinkTime);

            
            
            //wait 0.125 seconds
            //turn opacity back to 100%

            yield return new WaitForFixedUpdate();
            if (Time.time - startTime >= iFrameDuration)
                break;
        }



        invulnerable = false;

    }
}
