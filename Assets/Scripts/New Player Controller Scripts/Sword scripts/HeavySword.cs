using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySword : Sword
{
    public override void OnEnable()
    {
        base.OnEnable();
        PlayerAnimationManager.Instance.ActivateTrigger(PlayerAnimationManager.TriggerAnimations.HEAVY_ATTACK);
    }
}
