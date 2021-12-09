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
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IDamageable
{

    #region Base Stats

    [Header("unit base stats")]
    #region Unit's Movement Stats 

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
                
                //Destroy the object here
                Destroy(gameObject);
            }

            if (HealthBar != null)
                HealthBar.value = _health;
        }
    }

    ///<summary>The player's maximum health.</summary>
    [SerializeField]
    private float maxHealth;

    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            if (HealthBar != null) HealthBar.maxValue = value;
        }
    }
    #endregion


    #endregion

    #region Unit's bool determinates 

    ///<summary>This determines whether the unit is on the ground or not.</summary>
    protected bool grounded = true;

    ///<summary>This determines if the unit just preformed a jump or not.</summary>
    [HideInInspector]
    protected bool justJumped = false;

    ///<summary>This determines what direction the unit is facing.</summary>
    [SerializeField]
    public bool facingRight;

    #endregion
    
    #region Unit's Attacks

    ///<summary>This is the cool down between melee attacks for the unit .</summary>
    [HideInInspector]
    protected float attackCoolDown =  1f;

    ///<summary>This trakcs when the unit can deal damage again.</summary>
    [HideInInspector]
    protected float nextDamageEvent;


    ///// <summary> This determines how fast a Unit can cast spells </summary>
    //[HideInInspector]
    //protected float spellCastRate = 1f;

   

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

    public virtual IEnumerator Bleed()
    {
        throw new System.NotImplementedException();
    }
}
