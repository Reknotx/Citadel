using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [System.Serializable]
    public class SpellStats
    {
        public float cooldown = 1f;
        public int manaCost = 10;
        public int damage = 3;
    }

    public SpellStats stats;

    [HideInInspector]
    public bool movingRight;

    public abstract void TriggerSpell(GameObject target);

    public abstract void Move();

    public void Update()
    {
        Move();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 31)
            Destroy(gameObject);
        
        if (other.gameObject.layer == 8)
            TriggerSpell(other.gameObject);
    }


}
