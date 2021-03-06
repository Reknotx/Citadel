using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem
{
    public class SpellBook : MonoBehaviour
    {
        [HideInInspector]
        public List<LearnedSpell> learnedSpells;

        [HideInInspector]
        public GameObject spellInFocus;

        public GameObject learnedSpellPrefab;

        public GameObject spellAssigner;

        public Transform spellButtons;

        public List<GameObject> startingSpells;

        private void Awake()
        {
            foreach (GameObject startingSpell in startingSpells)
            {
                AddSpellToBook(startingSpell);
            }

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            spellInFocus = null;
            spellAssigner.SetActive(false);
        }

        public void AssignSpellToSlot(int slot)
        {
            if (spellInFocus == null) return;
            PlayerCombatSystem.Instance.spellSystem.AssignSpell(spellInFocus, slot);

            //PlayerCombatSystem.Instance.spellSystem.UpdateSpellSystemUI(NewPlayer.Instance.Mana);
        }

        /// <summary> Adds a new spell into the book. </summary>
        /// <param name="newSpell">The spell prefab to add.</param>
        public void AddSpellToBook(GameObject newSpell)
        {
            GameObject newLearnedSpellPrefab = Instantiate(learnedSpellPrefab, spellButtons);

            LearnedSpell temp = newLearnedSpellPrefab.GetComponent<LearnedSpell>();
            if (newSpell == null) Debug.Log("Here's the problem");
            temp.Spell = newSpell;

            learnedSpells.Add(temp);
            newLearnedSpellPrefab.GetComponent<Button>().onClick.AddListener(delegate { SelectSpell(temp.Spell); });
        }

        public void SelectSpell(GameObject focusSpell)
        {
            spellInFocus = focusSpell;
            spellAssigner.SetActive(true);
        }
    }
}
