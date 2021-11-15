using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class PlayerMeleeSystem : MonoBehaviour
    {
        public GameObject lightSword;
        public GameObject heavySword;


        public void SwingSword(string meleeAttack)
        {
            switch (meleeAttack)
            {
                case "lightAttack":
                    lightSword.SetActive(true);
                    break;

                case "heavyAttack":
                    heavySword.SetActive(true);
                    break;

                default:
                    break;
            }
        }
    }
}