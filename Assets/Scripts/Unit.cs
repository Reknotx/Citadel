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

    ///<summary>This is the unit's private health.</summary>
    [SerializeField]
    private int _health;

    ///<summary>This is the unit's public health.</summary>
    public virtual int Health
    {
        get;
        set;
    }

    ///<summary>This is the unit's speed.</summary>
    [Range(0, 5f)]
    [Tooltip("This is the unit's speed.")]
    public float speed;

    ///<summary>This is the unit's private rigidbody.</summary>
    [SerializeField]
    protected Rigidbody _rigidBody;

    ///<summary>This is the unit's private collider.</summary>
    [SerializeField]
    protected Collider _groundCollider;


    ///<summary>This determines the unit's jump height.</summary>
    [Range(0, 8f)]
    [Tooltip("This determines the unit's jump height.")]
    public float jumpFroce;

    ///<summary>This determines whether the unit is on the ground or not.</summary>
    protected bool isGrounded;

    ///<summary>This determines whether the unit is on a platform or not.</summary>
    protected bool onPlatform;

    [SerializeField]
    ///<summary>This determines whether the unit is going through a platform or not.</summary>
    protected bool throughPlatform;

    ///<summary>This determines if the unit just preformed a jump or not.</summary>
    protected bool justJumped = false;

    ///<summary>This determines what direction the unit is facing.</summary>
    protected bool facingRight;
    ///<summary>This is the cool down between melee attacks for the unit .</summary>
    protected float attackCoolDown =  1f;
    ///<summary>This trakcs when the unit can deal damage again.</summary>
    protected float nextDamageEvent;

    ///<summary>This dis the units collider for their light attack.</summary>
    [SerializeField]
    protected Collider _lightCollider;

    ///<summary>This dis the units collider for their heavy attack.</summary>
    [SerializeField]
    protected Collider _heavyCollider;

    ///<summary>These are the location weapons will appear in when the unit attacks.</summary>
    [SerializeField]
    protected GameObject weaponLocationLeft;
    ///<summary>These are the location weapons will appear in when the unit attacks.</summary>
    [SerializeField]
    protected GameObject weaponLocationRight;


    #endregion

    public virtual void Update()
    {
        if (throughPlatform == true && justJumped == true)
        {
            StartCoroutine(dropDown());
            _rigidBody.AddForce(Vector3.up * .03f, ForceMode.Impulse);
        }
    }


    #region Player Actions
    /// <summary>This moves the player from side to side on the x axis  /// </summary>
    /// <param name="context">this is the information returned when the input is registered</param>
    public void movement(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        _rigidBody.AddForce(new Vector3(inputVector.x, 0, 0) * speed, ForceMode.Force);
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
        if(context.performed)
        {
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
            
        }
    }

    ///<summary>This triggers the unit to drop down if they are on a platform.</summary>
    public void Drop(InputAction.CallbackContext context)
    {
        if ( onPlatform == true)
        {
            StartCoroutine(dropDown()); 
        }
    }

    /// <summary> This is the attacking function /// </summary>
    /// <param name="dmg">The amnt of dmg to target.</param>
    /// <param name="target">The target of the attack.</param>
    public void lightAttack(InputAction.CallbackContext context)
    {

        if (Time.time >= nextDamageEvent)
        {
            nextDamageEvent = Time.time + attackCoolDown;
            if (facingRight == true)
            {
                _lightCollider.transform.position = weaponLocationRight.transform.position;
                StartCoroutine(lightAttackCoroutine());

            }
            else
            {
                _lightCollider.transform.position = weaponLocationLeft.transform.position;
                StartCoroutine(lightAttackCoroutine());
            }
        }
        
    }

    /// <summary> This is the attacking function /// </summary>
    /// <param name="dmg">The amnt of dmg to target.</param>
    /// <param name="target">The target of the attack.</param>
    public void heavyAttack(InputAction.CallbackContext context)
    {
        if (Time.time >= nextDamageEvent)
        {
            nextDamageEvent = Time.time + attackCoolDown;
            if (facingRight == true)
            {
                _heavyCollider.transform.position = weaponLocationRight.transform.position;
                StartCoroutine(heavyAttackCoroutine());

            }
            else
            {
                _heavyCollider.transform.position = weaponLocationLeft.transform.position;
                StartCoroutine(heavyAttackCoroutine());
            }
        }
        

    }
    #endregion


    /// <summary> this allows units to drop through platforms </summary>
    public IEnumerator dropDown()
    {
        _groundCollider.enabled = false;
       yield return new WaitForSeconds(.5f);
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
}
