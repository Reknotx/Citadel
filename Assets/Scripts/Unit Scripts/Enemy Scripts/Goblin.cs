/* Author: Andrew Nave
 * Date: 9/20/2021
 *
 * Brief: This script manages the goblins basic properties and functions.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Goblin : Enemy
{
    #region Goblin Stats

    [SerializeField]
    private float goblinMeleeRange;

    public float goblinDamage;

    private bool canAttack = true;

    public float goblinDashForce = 10f;


    #endregion

    /*public Image goblinHealth;

    public Image HealthIMG;

    private float calculateHealth;*/

    #region Life Handler for Player

    public NewPlayer playerLife;

    #endregion


    #region Goblin Attack Visuals

    public GameObject goblinAttack_L;
    public GameObject goblinAttack_R;

    #endregion

    private bool canLunge;
    // Start is called before the first frame update
    public  override void Start()
    {
        base.Start();
        //Tyler Added code
        player = GameObject.FindGameObjectWithTag("Player");
        //end
        goblinAttack_L.SetActive(false);
        goblinAttack_R.SetActive(false);
        playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>();
        HealthIMG.gameObject.SetActive(false);
        canLunge = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        /* if(Health < maxHealth)
         {
             HealthIMG.gameObject.SetActive(true);
             calculateHealth = (float)Health / maxHealth;
             goblinHealth.fillAmount = Mathf.MoveTowards(goblinHealth.fillAmount, calculateHealth, Time.deltaTime);
         }
         else
         {
             HealthIMG.gameObject.SetActive(false);
         }*/

        /*if (Vector2.Distance(transform.position, player.transform.position) <= 5)
        {
            Astar.canMove = false;
        }
        else
        {
            Astar.canMove = true;
        }*/

        if(distanceToPlayer < followDistance)
        {
            GoblinSpotted = true;
        }

        if (distanceToPlayer <= goblinMeleeRange)
        {
            if (canAttack)
            {
                if(Mathf.Abs(yDistance) <= 2)
                {
                    StartCoroutine(Lunge());
                }
                
            }
        }
        if (canLunge)
        {
            if (facingRight)
            {

                StartCoroutine(LungeRight());

                StartCoroutine(WaitBetweenAttack());

            }
            else
            {

                StartCoroutine(LungeLeft());

                StartCoroutine(WaitBetweenAttack());

            }
        }
        

        

        /* yDistance = transform.position.y - player.transform.position.y;


     if (yDistance <= Mathf.Abs(jumpHeight))
     {
         /*if (isGrounded)
         {*/
        /* if (canJump)
         {
             //jump toward player

             //_rigidBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpVelocity));
             StartCoroutine(IsJumping());

         }

     //}

    /* if (onPlatform == true)
     {
         _rigidBody.velocity = new Vector2(0, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpVelocity));
         StartCoroutine(Jumped());

     }

 }
 else if (yDistance < jumpHeight)
 {
     //drop through floor toward player
 }*/

    }


    private void GoblinAttack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", isAttacking);
        if (facingRight)
        {
            StartCoroutine(WaitBetweenVisual_Right());
            // _rigidBody.AddForce(transform.right * goblinDashForce);
            _rigidBody.velocity = new Vector2(goblinDashForce, _rigidBody.velocity.y);
            playerLife.TakeDamage(goblinDamage);
            StartCoroutine(WaitBetweenAttack());


            //GoblinMeleeAttack();
        }
        else
        {
            StartCoroutine(WaitBetweenVisual_Left());
            // _rigidBody.AddForce(-transform.right * goblinDashForce);
            _rigidBody.velocity = new Vector2(-goblinDashForce, _rigidBody.velocity.y);
            playerLife.TakeDamage(goblinDamage);
            StartCoroutine(WaitBetweenAttack());
        }
    }

    IEnumerator Lunge()
    {
        canLunge = true;
        yield return new WaitForSeconds(1f);
        canLunge = false;
    }

    IEnumerator LungeLeft()
    {
        StartCoroutine(WaitBetweenVisual_Left());
        _rigidBody.AddForce(transform.right * goblinDashForce);
        Debug.Log("Hey Andrew it added the force");
        if(distanceToPlayer <= 1)
        {
            playerLife.TakeDamage(goblinDamage);
            Debug.Log("Hey Andrew it took damage");
        }
        yield return new WaitForSeconds(.2f);
    }
    IEnumerator LungeRight()
    {
        StartCoroutine(WaitBetweenVisual_Right());
        _rigidBody.AddForce(-transform.right * goblinDashForce);
        if (distanceToPlayer <= 1)
        {
            playerLife.TakeDamage(goblinDamage);
        }
        yield return new WaitForSeconds(.2f);
    }

    IEnumerator WaitBetweenAttack()
    {
        canAttack = false;
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
        yield return new WaitForSeconds(2f);
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
                _lightCollider.transform.position = goblinAttack_R.transform.position;
                _lightCollider.transform.position = _lightCollider.transform.position + (_lightCollider.gameObject.transform.localScale / 2);
                StartCoroutine(lightAttackCoroutine());

            }
            else
            {
                _lightCollider.transform.position = goblinAttack_L.transform.position;
                _lightCollider.transform.position = _lightCollider.transform.position - (_lightCollider.gameObject.transform.localScale / 2);
                StartCoroutine(lightAttackCoroutine());
            }
        }
    }
}
