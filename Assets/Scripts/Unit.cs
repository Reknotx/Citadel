/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/1/2021
 * 
 * Brief:this script holds the similar 
 * components/data of all Units in the game 
 */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Unit : MonoBehaviour
{

    #region Base Stats

         #region Unit's Movement Stats 

    ///<summary>This is the unit's speed.</summary>
    [Range(0, 5f)]
    [Tooltip("This is the unit's speed.")]
    public float speed;

    ///<summary>This determines the unit's jump height.</summary>
    [Range(0, 8f)]
    [Tooltip("This determines the unit's jump height.")]
    public float jumpFroce;

        #endregion
         #region Unit's Attached Colliders/Gameobjects

    ///<summary>This is the unit's private rigidbody.</summary>
    [SerializeField]
    protected Rigidbody _rigidBody;

    ///<summary>This is the unit's collider that detects the ground.</summary>
    [SerializeField]
    protected Collider _groundCollider;

    ///<summary>This is the unit's collider that detects the ground.</summary>
    [SerializeField]
    protected Collider _hitboxCollider;

    ///<summary>This dis the units collider for their light attack.</summary>
    [SerializeField]
    protected Collider _lightCollider;

    ///<summary>This dis the units collider for their heavy attack.</summary>
    [SerializeField]
    protected Collider _heavyCollider;

    ///<summary>This is the location spells will be cast on the left side of the unit.</summary>
    [SerializeField]
    protected GameObject spellLocationLeft;

    ///<summary>this is the location spells will cast on the right side of the unit.</summary>
    [SerializeField]
    protected GameObject spellLocationRight;

    ///<summary>This is the location spell will be cast from the center of the unit.</summary>
    [SerializeField]
    protected GameObject spellLocationCenter;

            #endregion
         #region Unit's bool determinates 

    ///<summary>This determines whether the unit is on the ground or not.</summary>
    [HideInInspector]
    protected bool isGrounded;

    ///<summary>This determines whether the unit is on a platform or not.</summary>
    [HideInInspector]
    protected bool onPlatform;

    [HideInInspector]
    ///<summary>This determines whether the unit is going through a platform or not.</summary>
    protected bool throughPlatform;

    ///<summary>This determines if the unit just preformed a jump or not.</summary>
    [HideInInspector]
    protected bool justJumped = false;

    ///<summary>This determines what direction the unit is facing.</summary>
    [HideInInspector]
    public bool facingRight;

    ///<summary>This determines what direction the unit hit another unit.</summary>
    [HideInInspector]
    protected bool hitOnRight;

            #endregion
         #region Unit's Attacks

    ///<summary>This is the cool down between melee attacks for the unit .</summary>
    [HideInInspector]
    protected float attackCoolDown =  1f;

    ///<summary>This trakcs when the unit can deal damage again.</summary>
    [HideInInspector]
    protected float nextDamageEvent;

    /// <summary> This is the time units must wait between casting spells </summary>
    [HideInInspector]
    protected float spellCastDelay;

    /// <summary> This determines how fast a Unit can cast spells </summary>
    [HideInInspector]
    protected float spellCastRate = 1f;

    /// <summary></summary>
    protected bool canCast;
    #endregion


    #endregion

    public virtual void Update()
    {
        ///<summary>this checks if the unit is trying to pass up through a platform and will assist.</summary>
        if (throughPlatform == true && justJumped == true)
        {
            StartCoroutine(dropDown());
            _rigidBody.AddForce(Vector3.up * .03f, ForceMode.Impulse);
        }

        ///<summary>this sets the rate for how quickly players can cast spells </summary>
        spellCastDelay -= Time.deltaTime * spellCastRate;
        if (spellCastDelay <= 0)
        {
            canCast = true;
            spellCastDelay = .7f;
        }

    }

    
    #region Unit Actions

        #region Player Movement Actions
    /// <summary>This moves the player from side to side on the x axis  /// </summary>
    /// <param name="context">this is the information returned when the input is registered</param>
    public void movement(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        //_rigidBody.AddForce(new Vector3(inputVector.x, 0, 0) * speed, ForceMode.Force);
        _rigidBody.MovePosition(transform.position + new Vector3(inputVector.x, transform.position.y, 0) * speed * Time.deltaTime);
        if(inputVector.x > 0)
        {
            facingRight = true;
        }
        if (inputVector.x < 0)
        {
            facingRight = false;
        }
    }

    ///<summary>This triggers the unit to jump up.</summary>
    public void Jump(InputAction.CallbackContext context)
    {
       // if(context.performed)
      //  {
            if(isGrounded==true)
            {
               
                _rigidBody.AddForce(Vector3.up * jumpFroce, ForceMode.Impulse);
                StartCoroutine(Jumped());

            }
            if (onPlatform==true)
            {
                _rigidBody.AddForce(Vector3.up * jumpFroce, ForceMode.Impulse);
                StartCoroutine(Jumped());

            }
            
        //}
    }

    ///<summary>This triggers the unit to drop down if they are on a platform.</summary>
    public void Drop(InputAction.CallbackContext context)
    {
        if ( onPlatform == true)
        {
            StartCoroutine(dropDown()); 
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
               fireWallSpell.GetComponent<Rigidbody>().velocity = fireWallSpell.transform.right * 12 +fireWallSpell.transform.up * -2;
                if(fireWallSpell.GetComponent<FireWallSpellScript>().changed == true)
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
    /// <summary> This is the attacking function /// </summary>
    public void lightAttack(InputAction.CallbackContext context)
    {

        if (Time.time >= nextDamageEvent)
        {
            nextDamageEvent = Time.time + attackCoolDown;
            if (facingRight == true)
            {
                _lightCollider.transform.position = spellLocationRight.transform.position;
                StartCoroutine(lightAttackCoroutine());

            }
            else
            {
                _lightCollider.transform.position = spellLocationLeft.transform.position;
                StartCoroutine(lightAttackCoroutine());
            }
        }
        
    }

    /// <summary> This is the attacking function /// </summary>

    public void heavyAttack(InputAction.CallbackContext context)
    {
        if (Time.time >= nextDamageEvent)
        {
            nextDamageEvent = Time.time + attackCoolDown;
            if (facingRight == true)
            {
                _heavyCollider.transform.position = spellLocationRight.transform.position;
                StartCoroutine(heavyAttackCoroutine());

            }
            else
            {
                _heavyCollider.transform.position = spellLocationLeft.transform.position;
                StartCoroutine(heavyAttackCoroutine());
            }
        }
        

    }

        #endregion
        #region Enemy Actions

    ///<summary>this makes the unit move between points A and B.</summary>
    public void patrolAB()
    {
        return;
    }


    #endregion

    #endregion

    #region IEnumerator Coroutines
    /// <summary> this allows units to drop through platforms </summary>
    public IEnumerator dropDown()
    {
        _groundCollider.enabled = false;
       yield return new WaitForSeconds(.7f);
        _groundCollider.enabled = true;
    }


    /// <summary> this allows the weapons collider to interact with things </summary>
    public IEnumerator lightAttackCoroutine()
    {
        _lightCollider.enabled = true;
        yield return new WaitForSeconds(.7f);
        _lightCollider.enabled = false;
    }

    /// <summary> this allows the weapons collider to interact with things </summary>
    public IEnumerator heavyAttackCoroutine()
    {
        _heavyCollider.enabled = true;
        yield return new WaitForSeconds(.7f);
        _heavyCollider.enabled = false;
    }

    public IEnumerator Jumped()
    {
        justJumped = true;
        yield return new WaitForSeconds(.5f);
        justJumped = false;
    }

    public IEnumerator Stun()
    {
        yield return new WaitForSeconds(2f);
    }

    public IEnumerator InvicibilityFrames()
    {
        _hitboxCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        _hitboxCollider.enabled = true;
    }
    #endregion
}
