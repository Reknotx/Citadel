using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSword : Sword
{
    public override void OnEnable()
    {
        base.OnEnable();
        PlayerAnimationManager.Instance.ActivateTrigger(PlayerAnimationManager.LIGHT_ATTACK);
    }
}
