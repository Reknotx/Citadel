/* Author: Andrew Nave
 * Date: 9/18/2021
 *
 * Brief: This script manages the orcs basic properties and functions.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Enemy
{

    #region Orc Stats

    [SerializeField]
    private float orcMeleeRange;

    public float orcDamage;

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

        if(Vector2.Distance(transform.position, player.transform.position) <= orcMeleeRange)
        {
            if (canAttack)
            {
                playerLife.Damage(orcDamage);
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
