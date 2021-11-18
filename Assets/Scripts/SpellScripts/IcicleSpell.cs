using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleSpell : Spell
{
    public override void Move()
    {
        ///Activate the movement logic here
    }

    public override void TriggerSpell(GameObject target)
    {
        target.GetComponent<IDamageable>().TakeDamage(stats.damage);
        Destroy(this.gameObject);
    }
}
