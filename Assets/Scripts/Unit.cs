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

public class Unit : MonoBehaviour, IDamageable
{

    #region Base Stats

    [Header("unit base stats")]
    #region Unit's Movement Stats 

    ///<summary>This is the unit's speed.</summary>
    [Range(0, 30f)]
    [Tooltip("This is the unit's speed.")]
    public float speed;

   
    #endregion
    #region Health
    [SerializeField]
    protected float _health;

    ///<summary>This is the maximum units health.</summary>
    public float maxHealth;

    ///<summary>This is the units health.</summary>
    public virtual float Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, maxHealth);

            if (_health <= 0)
            {
                ///Destroy the object here
                Destroy(gameObject);
            }
        }
    }
    #endregion


    #endregion

    #region Unit's Attached Colliders/Gameobjects
    [Header("unit colliders/ground detection")]


    ///<summary>This is the unit's collider that detects the ground.</summary>
    [SerializeField]
    [HideInInspector]
    public Collider _groundCollider;
    ///<summary>This is the unit's collider that detects the ground.</summary>
    [SerializeField]
    [HideInInspector]
    protected Collider _platformCollider;


    ///<summary>This dis the units collider for their light attack.</summary>
    [SerializeField]
    [HideInInspector]
    protected Collider _lightCollider;

    

   

            #endregion
    
    #region Unit's bool determinates 

    ///<summary>This determines whether the unit is on the ground or not.</summary>
    [HideInInspector]
    protected bool isGrounded;

    ///<summary>This determines whether the unit is on a platform or not.</summary>
    [HideInInspector]
    public bool onPlatform;

    [HideInInspector]
    ///<summary>This determines whether the unit is going through a platform or not.</summary>
    public bool throughPlatform;

    ///<summary>This determines if the unit just preformed a jump or not.</summary>
    [HideInInspector]
    protected bool justJumped = false;

    ///<summary>This determines what direction the unit is facing.</summary>
    [HideInInspector]
    public bool facingRight;

    ///<summary>This determines what direction the unit hit another unit.</summary>
    [HideInInspector]
    protected bool hitOnRight;

    /// <summary>this determines if the unit can cast a spell or not</summary>
    [HideInInspector]
    protected bool canCast;

    /// <summary> this determines if the unit is on fire or not </summary>
    [HideInInspector]
    protected bool onFire;

    /// <summary> this determines if the unit has recently taken ticking fire damage </summary>
    [HideInInspector]
    protected bool fireDamageTaken;
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

    /// <summary> this determines how long the unit will be on fire for</summary>
    [HideInInspector]
    protected float onFireDuration;

    /// <summary> this determines how much damage per tick will be applied to the unit</summary>
    [HideInInspector]
    protected int onFireDamage;

    /// <summary> this determines how quickly on fire damage will tick against health </summary>
 [HideInInspector]
    protected float onFireDamageRate = 1f;

    /// <summary> This determines the delay between taking on fire damage</summary>
    [HideInInspector]
    protected float onFireDamageDelay = 2f;

    #endregion
    public virtual void Update()
    {
      


        ///<summary>this determines if the unit can take damage from a initially cast fire spell</summary>
        onFireDamageDelay -= Time.deltaTime * onFireDamageRate;
        if (onFireDamageDelay <= 0)
        {
            fireDamageTaken = false;
            onFireDamageDelay = 2f;
        }

      
    }

    /// Author: Chase O'Connor
    /// Date: 9/28/2021
    /// <summary>
    /// This function takes in an integer value to subtract from this
    /// unit's current health.
    /// </summary>
    /// <param name="amount">The amount of damage to apply to the unit.</param>
    public virtual void TakeDamage(float amount)
    {
        Debug.Log("Dealing " + amount + " points of damage to " + name);
        Health -= amount;
    }
    
   
    #region IEnumerator Coroutines
   


    /// <summary> this allows the weapons collider to interact with things </summary>
    public IEnumerator lightAttackCoroutine()
    {
        _lightCollider.enabled = true;
        yield return new WaitForSeconds(.8f);
        _lightCollider.enabled = false;
    }



    

    public IEnumerator onFireCoroutine ()
    {
        
        onFire = true;
        yield return new WaitForSeconds(onFireDuration);
        onFire = false;
    }

    

   
     
    #endregion
}
