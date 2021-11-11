using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatSystem : MonoBehaviour
{

    public PlayerMeleeSystem meleeSystem;
    public PlayerSpellSystem spellSystem;

    /// <summary>
    /// Accepts a string that indicates which melee attack to use.
    /// <paramref name="meleeAttack"/> is passed off to PlayerMeleeSystem.
    /// </summary>
    /// <param name="meleeAttack"></param>
    public void MeleeAttack(string meleeAttack)
    {
        meleeSystem.SwingSword(meleeAttack);
    }


    public void SpellAttack(string spellAttack)
    {
        spellSystem.CastSpell(spellAttack);
    }


}
