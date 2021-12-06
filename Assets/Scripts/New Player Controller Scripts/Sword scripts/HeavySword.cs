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

    public override void AttackEnemy(Enemy target, int dmg)
    {
        target.GetComponent<IDamageable>().TakeDamage(NewPlayer.Instance.combatSystem.meleeSystem.playerMeleeDamage);
        if (NewPlayer.Instance.inventory.serratedStone && !target.bleeding)
        {
            target.StartBleed(dmg + Mathf.RoundToInt(dmg * 0.5f));
        }
        
    }
}
