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

        public void SwingSword(string meleeAttack)
        {
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