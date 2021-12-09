using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sword : MonoBehaviour
{
    public static Sword ActiveSword;

    protected List<GameObject> enemiesAttacked = new List<GameObject>();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnEnable()
    {
        ActiveSword = this;
        enemiesAttacked.Clear();
        StartCoroutine(delayTurnOffCoroutine());
    }

    public abstract void AttackEnemy(Enemy target, int dmg);

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && !enemiesAttacked.Contains(other.gameObject))
        {
            AttackEnemy(other.GetComponent<Enemy>(), NewPlayer.Instance.combatSystem.meleeSystem.playerMeleeDamage);
            DamagePopup.Create(transform.position, NewPlayer.Instance.combatSystem.meleeSystem.playerMeleeDamage);
            enemiesAttacked.Add(other.gameObject);
        }
    }

    IEnumerator delayTurnOffCoroutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

}