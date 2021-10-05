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

    #region Orc Attack Visuals

    public GameObject orcAttack_L;
    public GameObject orcAttack_R;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //Tyler added code
        player = GameObject.FindGameObjectWithTag("Player");
        //end
        orcAttack_L.SetActive(false);
        orcAttack_R.SetActive(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(transform.position, player.transform.position) <= orcMeleeRange)
        {
            if (canAttack)
            {
                OrcAttack();
            }
        }
    }

    private void OrcAttack()
    {
        if (facingRight)
        {
            StartCoroutine(WaitBetweenVisual_Right());
            playerLife.Damage(orcDamage);
            StartCoroutine(WaitBetweenAttack());
            
        }
        else
        {
            StartCoroutine(WaitBetweenVisual_Left());
            playerLife.Damage(orcDamage);
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
        orcAttack_R.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        orcAttack_R.SetActive(false);
    }

    IEnumerator WaitBetweenVisual_Left()
    {
        orcAttack_L.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        orcAttack_L.SetActive(false);
    }
}
