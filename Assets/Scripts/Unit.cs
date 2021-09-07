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
    protected Collider _collider;


    ///<summary>This determines the unit's jump height.</summary>
    [Range(0, 8f)]
    [Tooltip("This determines the unit's jump height.")]
    public float jumpFroce;

    ///<summary>This determines whether the unit is on the ground or not.</summary>
    protected bool isGrounded;

    ///<summary>This determines whether the unit is on a platform or not.</summary>
    protected bool onPlatform;

    
    #endregion

    public virtual void Update()
    {
       
    }


    #region Player Actions
    /// <summary>This moves the player from side to side on the x axis  /// </summary>
    /// <param name="context">this is the information returned when the input is registered</param>
    public void movement(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        _rigidBody.AddForce(new Vector3(inputVector.x, 0, 0) * speed, ForceMode.Force);
    }

    ///<summary>This triggers the unit to jump up.</summary>
    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(isGrounded==true)
            {
                _rigidBody.AddForce(Vector3.up * jumpFroce, ForceMode.Impulse);
            }
            if (onPlatform==true)
            {
                _rigidBody.AddForce(Vector3.up * jumpFroce, ForceMode.Impulse);
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
    public void Attack(int dmg, Enemy target)
    {

    }
    #endregion


    /// <summary> this allows units to drop through platforms </summary>
    /// <returns>turns off the units collider, waits for a time then turns it back on</returns>
    public IEnumerator dropDown()
    {
        _collider.enabled = false;
       yield return new WaitForSeconds(.7f);
        _collider.enabled = true;
    }

}
