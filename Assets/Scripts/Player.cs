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

public class Player : Unit
{
    


    #region Player Stats

    #region Player's Base Stats/Important controls

    ///<summary>This is the units health.</summary>
    public int myHealth;

    ///<summary>This is the maximum units health.</summary>
    public int maxHealth;

    ///<summary>This is the units mana for magic casting.</summary>
    public int myMana;

    ///<summary>This is the units maximum mana for magic casting.</summary>
    public int maxMana;

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
    public bool facingRightLocal;

            #endregion
            #region Player's Attack Stats/Spell Prefabs

    ///<summary>This determines how far the player will knock back an enemy with the heavy attack.</summary>
    public float knockbackForce;

    ///<summary>This determines the damage of the player's light attack.</summary>
    public int lightAttackDamage;

    ///<summary>This determines the damage of the player's heavy attack.</summary>
    public int heavyAttackDamage;

    ///<summary>This determines the damage the player deals to an enemy when they collide.</summary>
    public int playerCollisionDamage;

    /// <summary>this is the physical gameobject that is cast during the firewall spell</summary>
    public GameObject fireWall_prefab;
    #endregion
            #region Bool Determinates 

    /// <summary> determines if the player can move or not </summary>
        [HideInInspector]
        public bool canMove = true;

    /// <summary> determines if the player is trying to interact with things or not </summary>
   // [HideInInspector]
    public bool Interacting = false;

    #endregion
    #endregion


    private void Awake()
    {
       

        #region Player Movement Important Connectors
        ///<summary>The following is used to track player inputs and controls.</summary>
        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerControl.Enable();
        playerInputActions.PlayerControl.Jump.performed += Jump;
        playerInputActions.PlayerControl.Movement.performed += movement;
        playerInputActions.PlayerControl.Drop.performed += Drop;

        


        #endregion
    }


    public override void Update()
    {
        facingRightLocal = facingRight;
        base.Update();

        #region Player Movement Detection
        ///<summary>This moves the player constantly while the input is held.</summary>
        if (canMove == true)
        {
            Vector2 inputVector = playerInputActions.PlayerControl.Movement.ReadValue<Vector2>();
            _rigidBody.AddForce(new Vector3(inputVector.x, 0, 0) * speed, ForceMode.Acceleration);
        }
        
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
            _rigidBody.AddForce(Vector3.up * .03f, ForceMode.Impulse);
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
            }
        }
    }

    ///<summary>This triggers the unit to jump up.</summary>
    public void Jump(InputAction.CallbackContext context)
    {
        if (this != null)
        {
            if (isGrounded == true)
            {

                _rigidBody.AddForce(Vector3.up * jumpFroce, ForceMode.Impulse);
                StartCoroutine(Jumped());

            }
            if (onPlatform == true)
            {
                _rigidBody.AddForce(Vector3.up * jumpFroce, ForceMode.Impulse);
                StartCoroutine(Jumped());

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
        Interacting = true;
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


    #region Collision Detection
    public void OnTriggerStay(Collider other)
    {
        #region Camp Collisions
        if(other.gameObject.tag == "MineEntrance")
        {
            GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
            buttonController.GetComponent<SceneButtonControllerScript>().enterMineBTN.SetActive(true);
            if (Interacting == true)
            {
               
                GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
                sceneManager.GetComponent<SceneManagerScript>().goToMine();
                Interacting = false;
            }
        }


        if (other.gameObject.tag == "CastleEntrance")
        {
            GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
            buttonController.GetComponent<SceneButtonControllerScript>().enterCastleBTN.SetActive(true);

            if (Interacting == true)
            {

                GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
                sceneManager.GetComponent<SceneManagerScript>().goToCastle();
                Interacting = false;
            }
        }

        if (other.gameObject.tag == "CampShopEntrance")
        {
            GameObject buttonController = GameObject.FindGameObjectWithTag("ButtonController");
            buttonController.GetComponent<SceneButtonControllerScript>().enterCampShopBTN.SetActive(true);

            if (Interacting == true)
            {

                GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
                sceneManager.GetComponent<SceneManagerScript>().goToCampShop();
                Interacting = false;
            }
        }

        #endregion

        #region Enemy Collisions
        if (other.gameObject.tag == "Enemy")
        {
            myHealth--;
           
        }
        #endregion

        #region ground/platform collisions

        if (other.gameObject.tag =="ground")
        {
            _groundCollider.enabled = true;
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "platform")
        {
            _groundCollider.enabled = true;
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

    

    #endregion

}
