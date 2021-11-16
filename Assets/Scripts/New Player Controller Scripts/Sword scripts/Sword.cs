using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sword : MonoBehaviour
{
    public static Sword ActiveSword;

    protected List<GameObject> enemiesAttacked = new List<GameObject>();

    public virtual void OnEnable()
    {
        ActiveSword = this;
        enemiesAttacked.Clear();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && enemiesAttacked.Contains(other.gameObject))
        {
            other.gameObject.GetComponent<IDamageable>().TakeDamage(NewPlayer.Instance.combatSystem.meleeSystem.playerMeleeDamage);
            enemiesAttacked.Add(other.gameObject);
        }
    }
}