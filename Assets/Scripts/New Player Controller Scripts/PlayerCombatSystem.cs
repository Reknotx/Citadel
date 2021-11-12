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

namespace CombatSystem
{
    public class PlayerCombatSystem : MonoBehaviour
    {
        public static PlayerCombatSystem Instance;

        public PlayerMeleeSystem meleeSystem;
        public PlayerSpellSystem spellSystem;


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
                spellSlot = 1;
            else if (Keyboard.current.digit2Key.isPressed)
                spellSlot = 2;
            else if (Keyboard.current.digit3Key.isPressed)
                spellSlot = 3;

            spellSystem.CastSpell(spellSlot);
        }
    }
}