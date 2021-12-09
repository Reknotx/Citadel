/* Author: Andrew Nave
 * Date: 9/18/2021
 *
 * Brief: This script manages the orcs basic properties and functions.
 */

using System.Collections;
using UnityEngine;

public class Orc : Enemy
{

    #region Orc Stats

    [SerializeField]
    private float orcMeleeRange;

    public float orcDamage;

    private bool canAttack = true;
    #endregion

    #region Orc Attack Visuals

    public GameObject orcAttack_L;
    public GameObject orcAttack_R;

    #endregion

    public AudioSource attack;
    public AudioSource hurt;
    public AudioSource idle;
    public AudioSource die;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        
        orcAttack_L.SetActive(false);
        orcAttack_R.SetActive(false);

        idle.Play();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (debug) return;

        base.Update();

        if (Vector2.Distance(transform.position, base.player.transform.position) <= orcMeleeRange)
        {
            if (canAttack)
            {
                OrcAttack();
                attack.Play();
            }
        }

        if (isDead)
        {
            die.Play();
        }

    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        hurt.Play();
    }

    private void OrcAttack()
    {
        if (facingRight)
        {
            StartCoroutine(WaitBetweenVisual_Right());

            StartCoroutine(WaitBetweenAttack());

            if (player.GetComponent<NewPlayer>().invulnerable == false)
            {
                player.GetComponent<NewPlayer>().TakeDamage(orcDamage);
            }

        }
        else
        {
            StartCoroutine(WaitBetweenVisual_Left());

            StartCoroutine(WaitBetweenAttack());

            if (player.GetComponent<NewPlayer>().invulnerable == false)
            {
                player.GetComponent<NewPlayer>().TakeDamage(orcDamage);
            }
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
        //orcAttack_R.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //orcAttack_R.SetActive(false);
    }

    IEnumerator WaitBetweenVisual_Left()
    {
        //orcAttack_L.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //orcAttack_L.SetActive(false);
    }

    
}
