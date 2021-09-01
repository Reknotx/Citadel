using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField]
    private int _health;

    public virtual int Health
    {
        get;
        set;
    }

    ///<summary>This is the unit's speed.</summary>
    [Range(0, 50f)]
    [Tooltip("This is the unit's speed.")]
    public float speed;


    public virtual void Update()
    {

    }


    #region Player Actions
    public void movement()
    {

    }

    /// <summary>
    /// This is the attacking function
    /// </summary>
    /// <param name="dmg">The amnt of dmg to target.</param>
    /// <param name="target">The target of the attack.</param>
    public void Attack(int dmg, Enemy target)
    {

    }
    #endregion
}
