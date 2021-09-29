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
            GoblinMeleeAttack();
        }

    }

    public void GoblinMeleeAttack()
    {
        if (Time.time >= nextDamageEvent)
        {
            nextDamageEvent = Time.time + attackCoolDown;
            if (facingRight == true)
            {
                _lightCollider.transform.position = spellLocationRight.transform.position;
                _lightCollider.transform.position = _lightCollider.transform.position + (_lightCollider.gameObject.transform.localScale / 2);
                StartCoroutine(lightAttackCoroutine());

            }
            else
            {
                _lightCollider.transform.position = spellLocationLeft.transform.position;
                _lightCollider.transform.position = _lightCollider.transform.position - (_lightCollider.gameObject.transform.localScale / 2);
                StartCoroutine(lightAttackCoroutine());
            }
        }
    }
}
