public class LightSword : Sword
{
    public override void OnEnable()
    {
        base.OnEnable();
        PlayerAnimationManager.Instance.ActivateTrigger(PlayerAnimationManager.TriggerAnimations.LIGHT_ATTACK);
    }

    protected override void AttackEnemy(Enemy target, int dmg)
    {
        target.GetComponent<IDamageable>().TakeDamage(dmg);
        if (NewPlayer.Instance.inventory.serratedStone && !target.bleeding)
        {
            target.StartBleed(dmg);
        }
    }
}
