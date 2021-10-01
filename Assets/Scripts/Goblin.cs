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

    private float yDistance;

    public float jumpVelocity;

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

        yDistance = transform.position.y - player.transform.position.y;

        if (yDistance > jumpHeight)
        {
            if (isGrounded)
            {
                //jump toward player
                // _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpFroce);
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpVelocity);
                StartCoroutine(Jumped());
            }

            if (onPlatform == true)
            {
                //_rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpFroce);
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpVelocity);
                StartCoroutine(Jumped());

            }

        }
        else if (yDistance < jumpHeight)
        {
            //drop through floor toward player
        }

    }


    private void GoblinAttack()
    {
        if (facingRight)
        {
            StartCoroutine(WaitBetweenVisual_Right());
            playerLife.Damage(goblinDamage);
            StartCoroutine(WaitBetweenAttack());


            GoblinMeleeAttack();
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
