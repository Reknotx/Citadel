using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class PlayerMeleeSystem : MonoBehaviour
    {
        public void SwingSword(string meleeAttack)
        {
            switch (meleeAttack)
            {
                case "lightAttack":
                    LightAttack();
                    break;

                case "heavyAttack":
                    HeavyAttack();
                    break;

                default:
                    break;
            }
        }

        private void LightAttack()
        {
            ///Swing the light attack and activate the sword and initiate the animation.
            PlayerAnimationManager.Instance.PlayAnimation(PlayerAnimationManager.LIGHT_ATTACK);
        }

        private void HeavyAttack()
        {
            PlayerAnimationManager.Instance.PlayAnimation(PlayerAnimationManager.HEAVY_ATTACK);
        }
    }
}