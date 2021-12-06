using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleSpell : Spell
{
    private GameObject _target;


    private void Awake()
    {
        PlayerAnimationManager.Instance.ActivateTrigger("castIcicle");
    }


    public override void Move()
    {
        ///Activate the movement logic here
    }

    

  
    
    public override void TriggerSpell(GameObject target)
    {
        target.GetComponent<IDamageable>().TakeDamage(stats.damage);
        DamagePopup.Create(transform.position, stats.damage);
        Destroy(this.gameObject);
    }
}
