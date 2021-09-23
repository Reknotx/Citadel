/* Author: Andrew Nave
 * Date: 9/20/2021
 *
 * Brief: This script manages the goblins basic properties and functions.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    #region Goblin Stats

    [SerializeField]
    private float goblinMeleeRange;

    public float goblinDamage;

    private bool canAttack = true;

    #endregion

    #region Life Handler for Player

    public LifeManaHandler playerLife;

    #endregion


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(transform.position, player.transform.position) <= goblinMeleeRange)
        {
            if (canAttack)
            {
                playerLife.Damage(goblinDamage);
                StartCoroutine(WaitBetweenAttack());
            }
        }

    }

    IEnumerator WaitBetweenAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
}
