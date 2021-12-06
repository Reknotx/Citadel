using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Sprite spellUIImage;

    public bool movingSpell = true;

    protected abstract void TriggerSpell(GameObject target);

    protected abstract void Move();

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

    protected virtual void OnEnable()
    {
        NewPlayer.Instance.Mana -= stats.manaCost 
                                   - (NewPlayer.Instance.inventory.spellStone
                                       ? Mathf.RoundToInt(stats.manaCost * 0.25f)
                                       : 0);
    }

}
