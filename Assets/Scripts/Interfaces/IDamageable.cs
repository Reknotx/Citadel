/*
 * Author: Chase O'Connor
 * Date: 11/2/2021
 */

using System.Collections;

/// <summary> Interface that makes a unit as a damageable and forces functionality needed for combat. </summary>
public interface IDamageable
{
    public void TakeDamage(float amount);

    public IEnumerator Bleed();
}
