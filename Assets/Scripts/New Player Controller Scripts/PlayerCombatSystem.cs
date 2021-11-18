/*
 * Author: Chase O'Connor
 * Date: 11/10/2021.
 * 
 * Brief: Handles the player combat input and sends out 
 * the appropriate signals as needed.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CombatSystem
{
    /// <summary>
    /// Performs actions in the system for combat and even updates UI.
    /// </summary>
    public class PlayerCombatSystem : MonoBehaviour
    {
        public static PlayerCombatSystem Instance;

        public PlayerMeleeSystem meleeSystem;
        public PlayerSpellSystem spellSystem;

        public Slider healthBar;
        public Slider manaBar;



        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(Instance);

            Instance = this;
        }

        public void OnLightAttack()
        {
            meleeSystem.SwingSword("lightAttack");
        }

        public void OnHeavyAttack()
        {
            meleeSystem.SwingSword("heavyAttack");
        }

        public void OnCastSpell()
        {
            int spellSlot = 0;

            if (Keyboard.current.digit1Key.isPressed)
                spellSlot = 0;
            else if (Keyboard.current.digit2Key.isPressed)
                spellSlot = 1;
            else if (Keyboard.current.digit3Key.isPressed)
                spellSlot = 2;

            spellSystem.CastSpell(spellSlot);
        }

        public void OnOpenSpellBook()
        {
            spellSystem.spellBook.gameObject.SetActive(!spellSystem.spellBook.gameObject.activeSelf);
        }

        public void UpdateCombatUI()
        {
            NewPlayer player = NewPlayer.Instance;

            SetHealthBar((int)player.Health);

            SetManaBar(player.Mana);


            void SetHealthBar(int health)
            {
                healthBar.value = Mathf.Clamp(health, 0, healthBar.maxValue);
            }

            void SetManaBar(int mana)
            {
                manaBar.value = Mathf.Clamp(mana, 0, manaBar.maxValue);
                spellSystem.UpdateSpellSystemUI(mana);
            }

        }
    }
}