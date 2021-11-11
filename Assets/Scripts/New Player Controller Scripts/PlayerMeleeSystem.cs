using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        PlayerAnimationManager.Instance.PlayAnimation("LightAttack");
    }


    private void HeavyAttack()
    {
        
    }
}
