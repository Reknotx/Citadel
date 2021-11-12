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
            public bool canCast = true;

            public void AssignSpell(GameObject spell)
            {
                this.spell = spell;
                manaCost = spell.GetComponent<Spell>().stats.manaCost;
                spellImage = spell.GetComponent<Spell>().spellUIImage;
            }

            public void CompareCurrManaToManaCost(int playerMana)
            {
                Color temp = spellImage.color;
                temp.a = playerMana < manaCost ? 0.5f : 1f;
                canCast = temp.a == 1f;
                spellImage.color = temp;
            }
        }

        [HideInInspector]
        public List<GameObject> acquiredSpells;

        public List<SpellSlot> spellSlots = new List<SpellSlot>(3);

        public void CastSpell(int slotIndex)
        {
            if (!spellSlots[slotIndex].canCast) return;

            GameObject spellToCast = spellSlots[slotIndex].spell;

            ///Apply force to spell or perform unique movement math
            GameObject spawnedSpell = Instantiate(spellToCast);

        }

        public void SwapSpell(GameObject spell, int slotIndex)
        {
            spellSlots[slotIndex].spell = spell;
        }

        /// <summary>
        /// Adds a new spell into the book.
        /// </summary>
        /// <param name="newSpell">The spell prefab to add.</param>
        public void AddSpellToBook(GameObject newSpell)
        {
            //if (newSpell.GetComponent<Spell>() == null)
            //    Debug.LogError(newSpell.name + " does not have a spell script attached.");

            acquiredSpells.Add(newSpell);
        }


        public void UpdateSpellSystemUI(int playerMana)
        {
            foreach (SpellSlot spellSlot in spellSlots)
            {
                spellSlot.CompareCurrManaToManaCost(playerMana);
            }
        }
    }
}