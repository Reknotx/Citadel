/* Author: Andrew Nave
 * Date: 10/10/2021
 *
 * Brief: This script controls the AI specific to the skeleton Lancer.
 */

using System.Collections;
using UnityEngine;

public class SkeletonLancer : Enemy
{

    #region Lancer Dash Properties

    private bool isDashing;
    private bool canDash;


    public float dashDistance;

    public float dashSpeed;

    public float howFarToDash;

    public float dashForce;

    /*public Image skeletonHealth;

    public Image HealthIMG;

    private float calculateHealth;*/


    #endregion

    #region Lancer Attack Visuals

    public GameObject Lance_R;
    public GameObject Lance_L;

    #endregion

    public AudioSource attack;
    public AudioSource idle;
    public AudioSource hurt;
    public AudioSource die;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        Lance_R.SetActive(false);
        Lance_L.SetActive(false);

        isDashing = false;
        canDash = true;

        HealthIMG.gameObject.SetActive(false);

        idle.Play();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        /*if (Health < maxHealth)
        {
            HealthIMG.gameObject.SetActive(true);
            calculateHealth = (float)Health / maxHealth;
            skeletonHealth.fillAmount = Mathf.MoveTowards(skeletonHealth.fillAmount, calculateHealth, Time.deltaTime);
        }
        else
        {
            HealthIMG.gameObject.SetActive(false);
        }*/

        if(isDead)
        {
            isDashing = false;
            die.Play();
        }

        if (isDashing)
        {
            attack.Play();
            if (facingRight)
            {
                
                StartCoroutine(DashRight());
                
                StartCoroutine(DashCooldown());

            }
            else
            {
                
                StartCoroutine(DashLeft());
                
                StartCoroutine(DashCooldown());

            }
        }

        if (distanceToPlayer <= dashDistance)
        {
            if (canDash)
            {
                StartCoroutine(Dash());
            }

        }
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        hurt.Play();
    }

    IEnumerator DashRight()
    {
        isAttacking = true;
        //Lance_R.SetActive(true);
        Astar.canMove = false;
        yield return new WaitForSeconds(1f);
        Astar.canMove = true;
        _rigidBody.AddForce(-transform.right * dashForce);
        yield return new WaitForSeconds(1.5f);
        //Lance_R.SetActive(false);
    }

    IEnumerator DashLeft()
    {
        isAttacking = true;
        //Lance_L.SetActive(true);
        Astar.canMove = false;
        yield return new WaitForSeconds(1f);
        Astar.canMove = true;
        _rigidBody.AddForce(transform.right * dashForce);
        yield return new WaitForSeconds(1.5f);
        //Lance_L.SetActive(false);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        isDashing = false;
    }

    IEnumerator DashCooldown()
    {

        canDash = false;
        yield return new WaitForSeconds(2f);
        isAttacking = false;

        yield return new WaitForSeconds(3f);
       
        canDash = true;
    }
}
