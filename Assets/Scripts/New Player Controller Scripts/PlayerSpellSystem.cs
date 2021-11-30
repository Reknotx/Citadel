using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CombatSystem
{
    ///Need to handle the UI for the spell system
    ///When the player doesn't have enough mana for a spell
    ///the spell icon needs to be grayed out so it is obvious 
    ///to the player that they don't have enough resources. 
    ///The spell system will need to be updated
    ///whenever mana is gained or spent.

    public class PlayerSpellSystem : MonoBehaviour
    {
        public List<SpellSlot> spellSlots = new List<SpellSlot>();

        public SpellBook spellBook;

        public void CastSpell(int slotIndex)
        {
            if (NewPlayer.Instance.isPaused) return;

            SpellSlot attemptedCast = spellSlots[slotIndex];

            if (!attemptedCast.CanCast) return;

            Debug.Log("Casting " + attemptedCast.Spell.name);

            //Apply force to spell or perform unique movement math
            GameObject spawnedSpell = Instantiate(attemptedCast.Spell, NewPlayer.Instance.Center, Quaternion.identity);

            if (spawnedSpell.GetComponent<Spell>().movingSpell)
            {
                int multiplier = NewPlayer.Instance.facingRight ? 1 : -1;

                spawnedSpell.GetComponent<Rigidbody>().velocity = new Vector3((NewPlayer.Instance.speed + 2) * multiplier,
                                                                              0f,
                                                                              0f);
            }

            Debug.Log(spawnedSpell.GetComponent<Rigidbody>().velocity.x);

        }

        public void AssignSpell(GameObject spell, int slotIndex)
        {
            foreach (SpellSlot assignedSpell in spellSlots)
            {
                if (assignedSpell.Spell == spell)
                {
                    assignedSpell.Clear();
                }
            }

            spellSlots[slotIndex - 1].AssignSpell(spell);

            UpdateSpellSystemUI(NewPlayer.Instance.Mana);

        }


        public void UpdateSpellSystemUI(int playerMana)
        {
            foreach (SpellSlot spellSlot in spellSlots)
            {
                spellSlot.CompareCurrManaToManaCost(playerMana);
            }
        }
    }

    [System.Serializable]
    public class SpellSlot
    {
        [HideInInspector]
        public GameObject Spell;

        [HideInInspector]
        public int manaCost;

        public Image spellImage;

        public Text manaCostText;

        private bool _canCast;

        private bool sufficientMana;
        
        public bool CanCast => remainingCooldown == 0f && sufficientMana;


        [HideInInspector]
        public float cooldownTime;

        private float remainingCooldown;
        
        [HideInInspector]
        private bool isEmpty;

        public void AssignSpell(GameObject spell)
        {
            this.Spell = spell;
            manaCost = spell.GetComponent<Spell>().stats.manaCost;
            manaCostText.text = manaCost.ToString();
            manaCostText.enabled = true;
            spellImage.sprite = spell.GetComponent<Spell>().spellUIImage;
            spellImage.enabled = true;

            cooldownTime = spell.GetComponent<Spell>().stats.manaCost;
            
            isEmpty = false;
        }

        public void DecreaseCooldown(float deltaTime)
        {
            remainingCooldown -= deltaTime;
            if (remainingCooldown <= 0f)
            {
                remainingCooldown = 0;
            }
            
        }
        
        
        

        public void CompareCurrManaToManaCost(int playerMana)
        {
            Color temp = spellImage.color;
            temp.a = playerMana < manaCost ? 0.5f : 1f;
            
            spellImage.color = temp;
            
            sufficientMana = temp.a == 1f;
        }

        public void Clear()
        {
            Spell = null;
            manaCost = 0;
            spellImage.enabled = false;
            manaCostText.enabled = false;

            isEmpty = true;
        }
    }
}