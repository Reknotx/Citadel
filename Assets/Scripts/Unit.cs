/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/1/2021
 * 
 * Brief:this script holds the similar 
 * components/data of all Units in the game 
 * 
 * Edited heavily by Chase O'Connor for clean up
 * and new player system
 */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour, IDamageable
{

    #region Base Stats

    [Header("unit base stats")]
    #region Unit's Movement Stats 

    ///<summary>This is the unit's speed.</summary>
    [Range(0, 10f)]
    [Tooltip("This is the unit's maximum speed.")]
    public float speed;

   
    #endregion
    #region Health
    protected float _health;

    public Slider HealthBar;

    ///<summary>This is the units health.</summary>
    public virtual float Health
    {
        get => _health;
        set
        {
            _health = value;

            if (_health <= 0)
            {
                ///Destroy the object here
                Destroy(gameObject);
            }

            if (HealthBar != null)
            {
                HealthBar.value = _health;
                HealthBar.GetComponentInChildren<Text>().text = value.ToString();
            }
        }
    }

    ///<summary>The player's maximum health.</summary>
    [SerializeField]
    private float maxHealth;

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }
    #endregion


    #endregion

    #region Unit's bool determinates 

    ///<summary>This determines whether the unit is on the ground or not.</summary>
    protected bool grounded = true;

    ///<summary>This determines if the unit just preformed a jump or not.</summary>
    protected bool justJumped = false;

    ///<summary>This determines what direction the unit is facing.</summary>
    [HideInInspector]
    public bool facingRight;

    /// <summary> this determines if the unit is on fire or not </summary>
    protected bool onFire;

    /// <summary> this determines if the unit is poisoned or not </summary>
    //[HideInInspector]
    [HideInInspector]
    public bool poisoned;

    /// <summary> this determines if the unit has recently taken ticking poison damage </summary>
    private bool poisonDamageTaken;

    /// <summary> this determines if the unit has recently taken ticking fire damage </summary>
    private bool fireDamageTaken;
    #endregion
    
    #region Unit's Attacks
    /// <summary> this determines how long the unit will be on fire for</summary>
    protected float onFireDuration;

    /// <summary> this determines how much damage per tick will be applied to the unit</summary>
    protected int onFireDamage;

    /// <summary> this determines how quickly on fire damage will tick against health </summary>
    private float onFireDamageRate = 1f;

    /// <summary> This determines the delay between taking on fire damage</summary>
    private float onFireDamageDelay = 2f;

    /// <summary> this determines how long the unit will be on fire for</summary>
    [HideInInspector]
    public float poisonedDuration = 5f;

    /// <summary> this determines how much damage per tick will be applied to the unit</summary>
    [HideInInspector]
    public int poisonedDamage;

    /// <summary> this determines how quickly on fire damage will tick against health </summary>
    [HideInInspector]
    protected float poisonedDamageRate = 1f;

    /// <summary> This determines the delay between taking on fire damage</summary>
    [HideInInspector]
    protected float poisonedDamageDelay = 2f;

    #endregion

    public virtual void Awake()
    {
        if (HealthBar != null)
            HealthBar.maxValue = MaxHealth;
        Health = MaxHealth;
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        //this determines if the unit can take damage from a initially cast fire spell
        onFireDamageDelay -= Time.deltaTime * onFireDamageRate;
        if (onFireDamageDelay <= 0)
        {
            fireDamageTaken = false;
            onFireDamageDelay = 2f;
        }

        poisonedDamageDelay -= Time.deltaTime * poisonedDamageRate;
        if (poisonedDamageDelay <= 0)
        {
            poisonDamageTaken = false;
            poisonedDamageDelay = 2f;
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
    public IEnumerator onFireCoroutine ()
    {
        
        onFire = true;
        yield return new WaitForSeconds(onFireDuration);
        onFire = false;
    }
    
    public IEnumerator poisonedCoroutine()
    {

        poisoned = true;
        yield return new WaitForSeconds(poisonedDuration);
        poisoned = false;
    }

    #endregion
    
    
    
    
}
