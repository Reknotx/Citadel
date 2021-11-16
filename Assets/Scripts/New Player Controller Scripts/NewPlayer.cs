/*
 * Author: Chase O'Connor
 * Date: 11/10/2021.
 * 
 * Brief: A new and improved player controller for the game.
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using CombatSystem;


public class NewPlayer : Unit, IDamageable
{
    #region Fields



    #endregion
    public static NewPlayer Instance;

    [HideInInspector]
    public Rigidbody playerRB;

    [HideInInspector]
    public Vector2 moveDir;

    public float jumpHeight = 5f;

    public GameObject physicalBody;
    
    bool checkForPlatform = true;
    
    private int groundLayer = 1 << 10;
    private int platformLayer = 1 << 11;
    private int solidGroundLayer = 1 << 31;
    
    private int playerLayer = 7;
    private int ignorePlayerLayer = 12;

    public PlayerCombatSystem combatSystem;
    public PlayerInventory inventory;

    public Slider ManaBar;
    public Slider HealthBar;

    [Tooltip("The height of the model for collision detection purposes.")]
    public float modelHeight = 1.5f;

    /// <summary> The player's health. </summary>
    public override float Health
    {
        get => base.Health;

        set
        {
            base.Health = Mathf.Clamp(value, 0, _maxHealth);
            if (HealthBar != null) HealthBar.value = value;
            if (Health == 0)
            {
                ///Activate game ver logic
                ///after game over logic it is better to turn off the player 
                ///object rather than destroy it
                ///Movement of the player and attacking moves
                ///of the player could actually be possibly done on a 
                ///different script but probably not at the same time
                ///too confusing.
            }
        }
    }

    private int _mana;
    private int _maxMana;

    /// <summary> The player's mana. </summary>
    public int Mana
    {
        get => _mana;

        set
        {
            _mana = value;
            if (ManaBar != null) ManaBar.value = value;

            combatSystem.spellSystem.UpdateSpellSystemUI(_mana);
        }
    }

    public int MaxMana
    {
        get => _maxMana;
        set
        {
            _maxMana = value;
        }
    }

    public override void Awake()
    {
        if (Instance != null & Instance != this)
            Destroy(Instance);

        Instance = this;
        playerRB = GetComponent<Rigidbody>();
        inventory = new PlayerInventory();

        base.Awake();
    }

    public void Start()
    {
        combatSystem = PlayerCombatSystem.Instance;
    }

    public override void Update()
    {
        Move();

        GroundedCheck();

    }

    /// <summary> Moves the player with forces and puts a limit on our maximum velocity. </summary>
    public void Move()
    {
        ///Player needs to perform a check to see if it's about to collide
        ///with a wall if it moves in the proposed direction.
        ///If it does we want to cancel the request for forces to be added
        ///and to stop the player as well, this might be as simple as 
        ///setting the moveDir equal to zero so that the below if statement
        ///fires off and does the job for us
        Vector3 playersCurrVel = playerRB.velocity;
        if (moveDir != Vector2.zero && CheckForWalls())
        {
            Debug.Log("Collided with wall");
            playerRB.velocity = new Vector3(0, playersCurrVel.y);
        }
        else if (moveDir == Vector2.zero)
        {
            ///Get the player's velocity.
            ///use the velocity to calculate how much force in the opposite 
            ///direction we need to put on the player to stop it faster
            Vector3 stoppingForce = new Vector3(playersCurrVel.x * -1, 0f);
            playerRB.AddForce(stoppingForce);
        }
        else playerRB.AddForce(moveDir * 20);

        ///when move dir is 0 in the x i need to apply a force in the opposite direction
        ///to stop the player from moving so that drag can remain at 0

        playerRB.velocity = new Vector3(Mathf.Clamp(playerRB.velocity.x, -speed, speed), playerRB.velocity.y);

        ///Internal helper function to check for walls or terrain in front of the player.
        bool CheckForWalls()
        {
            int layerMask;
            if (physicalBody.layer == ignorePlayerLayer)
            {
                layerMask = groundLayer | solidGroundLayer;
            }
            else layerMask = groundLayer | solidGroundLayer | platformLayer;


            ///Here I'll want to do a physics cast instead of a ray cast
            ///so that I get all of the information easily and without trial 
            ///and error 
            return Physics.BoxCast(new Vector3(transform.position.x, transform.position.y + (modelHeight / 2), transform.position.z),
                                   new Vector3(0.1f, modelHeight / 2, 0.5f),
                                   moveDir,
                                   Quaternion.identity,
                                   0.3f,
                                   layerMask);
        }
    }

    private void GroundedCheck()
    {
        if (playerRB.velocity.y > 0f)
        {
            grounded = false;
        }
        else if ((checkForPlatform && CheckForPlatform()) || CheckIfGrounded())
        {
            physicalBody.layer = playerLayer;
            grounded = true;
            Debug.Log("Landed");
            PlayerAnimationManager.Instance.ActivateTrigger(PlayerAnimationManager.LANDING);
        }
        else if (grounded == false && playerRB.velocity.y < 0)
        {
            PlayerAnimationManager.Instance.ActivateTrigger(PlayerAnimationManager.FALLING);
        }

        bool CheckIfGrounded()
        {
            return Physics.CheckBox(transform.position,
                                    new Vector3(0.1f, 0.2f, 0.5f),
                                    Quaternion.identity,
                                    groundLayer | solidGroundLayer);
        }

        bool CheckForPlatform()
        {
            return Physics.CheckBox(transform.position,
                                    new Vector3(0.1f, 0.1f, 0.5f),
                                    Quaternion.identity,
                                    platformLayer);
        }
    }

    public void OnMove(InputValue value)
    {
        ///Use forces to move the player in the desired direction instead
        ///use the technique that Brackey's utilized in the Ball wars video
        ///Limit the velocity the player can have in the x direction only 
        moveDir = value.Get<Vector2>();

        facingRight = Keyboard.current.dKey.isPressed;

        if (Keyboard.current.dKey.isPressed)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        //if (moveDir == Vector2.zero) 
    }

    public void OnJump()
    {
        if (!grounded) return;
        Debug.Log("Grounded");

        grounded = false;
        physicalBody.layer = ignorePlayerLayer;
        playerRB.velocity = new Vector3(0, Mathf.Sqrt(-2.0f * Physics.gravity.y * jumpHeight));
        PlayerAnimationManager.Instance.ActivateTrigger(PlayerAnimationManager.JUMP);
    }

    public void OnDropDown()
    {
        checkForPlatform = false;
        physicalBody.layer = ignorePlayerLayer;
        StartCoroutine(GroundedCheckDelay());
    }

    IEnumerator GroundedCheckDelay()
    {
        yield return new WaitForSeconds(1f);
        checkForPlatform = true;
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }

    void Attack()
    {
        ///How can we process the attack to ake it more dynamic?
        ///Ideas I need ideas
        ///Having a secondary class that processes a string
        ///sent in via the input system that then returns a 
        ///stored object which is used to instantiate spells
        ///
        ///However, we can make it simpler by setting up harder references to
        ///sword attacks versus spells
        ///A spell system object that's an empty gameobject a child of the player
        ///game object
        ///
        ///A player Combat system script is going to be the best course of action here
        ///The player combat system script will take in string inputs and then
        ///use that string information to make the appropriate calls to other 
        ///combat class scripts for melee attacks and spell attacks 
        ///the melee attack script can trigger the animations and turn on the spell
        ///while the spell attack script can spawn in spells and fire them 
        ///away from the player
        ///
        ///The possible way to have it work with spells and the sword is to change
        ///the combat system entirely from what Tyler wants and allow for the player 
        ///to cast spells and swing the sword at the same time.
        ///The melee combat system script will register if we need to swing
        ///the light attack or the heavy attack
        ///
        ///The spell combat sysstem script will hold all of the spells and allow
        ///the player to select two or three spells they can fire off by pressing
        ///the number keys and then spawns the spells fired off into the direction
        ///the player is facing
        ///the player can right click and select the number slot the spell will be in
        ///The spell book will have it's own special UI so that the player can
        ///examine the spells and what damage they will do and what special
        ///effects they have
        ///
        ///This will help make things more dynamic for us and allow for
        ///as many spells as want for the player to have.x
    }
}
