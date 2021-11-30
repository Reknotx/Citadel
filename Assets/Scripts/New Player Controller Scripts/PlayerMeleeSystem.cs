using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class PlayerMeleeSystem : MonoBehaviour
    {
        public Sword lightSword;
        public Sword heavySword;

        public int playerMeleeDamage;

        ///<summary>This determines how far the player will knock back an enemy with the heavy attack.</summary>
        public float knockbackForce;

        public void SwingSword(string meleeAttack)
        {
            if (NewPlayer.Instance.isPaused) return;

            switch (meleeAttack)
            {
                case "lightAttack":
                    if (!lightSword.gameObject.activeSelf)
                        lightSword.gameObject.SetActive(true);
                    break;

                case "heavyAttack":
                    if (!heavySword.gameObject.activeSelf)
                        heavySword.gameObject.SetActive(true);
                    break;

                default:
                    break;
            }
        }
    }
}