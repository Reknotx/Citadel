using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public List<SpellSlot> spellSlots = new List<SpellSlot>(3);

        public SpellBook spellBook;

        public void CastSpell(int slotIndex)
        {
            if (!spellSlots[slotIndex].canCast) return;

            GameObject spellToCast = spellSlots[slotIndex].spell;

            ///Apply force to spell or perform unique movement math
            GameObject spawnedSpell = Instantiate(spellToCast, NewPlayer.Instance.transform.position, Quaternion.identity);

            int multiplier = NewPlayer.Instance.playerRB.velocity.x < 0 ? -1 : 1;
            spawnedSpell.GetComponent<Rigidbody>().velocity = new Vector3(NewPlayer.Instance.speed + (3 * multiplier) * (NewPlayer.Instance.facingRight ? 1 : -1),
                                                                          0f,
                                                                          0f);
        }

        public void AssignSpell(GameObject spell, int slotIndex)
        {
            foreach (SpellSlot assignedSpell in spellSlots)
            {
                if (assignedSpell.spell == spell)
                {
                    assignedSpell.Clear();
                }
            }

            spellSlots[slotIndex].spell = spell;
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
        public GameObject spell;

        [HideInInspector]
        public int manaCost;

        public Image spellImage;

        public Text manaCostText;

        [HideInInspector]
        public bool canCast;

        [HideInInspector]
        private bool isEmpty;

        public void AssignSpell(GameObject spell)
        {
            this.spell = spell;
            manaCost = spell.GetComponent<Spell>().stats.manaCost;
            manaCostText.text = manaCost.ToString();
            manaCostText.enabled = true;
            spellImage = spell.GetComponent<Spell>().spellUIImage;
            spellImage.enabled = true;

            isEmpty = false;
        }

        public void CompareCurrManaToManaCost(int playerMana)
        {
            Color temp = spellImage.color;
            temp.a = playerMana < manaCost ? 0.5f : 1f;

            canCast = temp.a == 1f;

            spellImage.color = temp;
        }

        public void Clear()
        {
            spell = null;
            manaCost = 0;
            spellImage.enabled = false;
            manaCostText.enabled = false;
            canCast = true;

            isEmpty = true;
        }
    }
}