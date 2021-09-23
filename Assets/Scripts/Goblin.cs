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

    #region Goblin Attack Visuals

    public GameObject goblinAttack_L;
    public GameObject goblinAttack_R;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        goblinAttack_L.SetActive(false);
        goblinAttack_R.SetActive(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(transform.position, player.transform.position) <= goblinMeleeRange)
        {
            if (canAttack)
            {
                GoblinAttack();
            }
        }

    }

    private void GoblinAttack()
    {
        if (facingRight)
        {
            StartCoroutine(WaitBetweenVisual_Right());
            playerLife.Damage(goblinDamage);
            StartCoroutine(WaitBetweenAttack());

        }
        else
        {
            StartCoroutine(WaitBetweenVisual_Left());
            playerLife.Damage(goblinDamage);
            StartCoroutine(WaitBetweenAttack());
        }
    }

    IEnumerator WaitBetweenAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }

    IEnumerator WaitBetweenVisual_Right()
    {
        goblinAttack_R.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        goblinAttack_R.SetActive(false);
    }

    IEnumerator WaitBetweenVisual_Left()
    {
        goblinAttack_L.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        goblinAttack_L.SetActive(false);
    }
}
