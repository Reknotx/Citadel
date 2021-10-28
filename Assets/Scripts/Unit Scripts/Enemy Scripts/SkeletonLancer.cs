/* Author: Andrew Nave
 * Date: 10/10/2021
 *
 * Brief: This script controls the AI specific to the skeleton Lancer.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonLancer : Enemy
{

    #region Lancer Dash Properties

    private bool isDashing;
    private bool canDash;


    public float dashDistance;

    public float dashSpeed;

    public float howFarToDash;

    public float dashForce;

    public Image skeletonHealth;

    public Image HealthIMG;

    private float calculateHealth;


    #endregion

    #region Lancer Attack Visuals

    public GameObject Lance_R;
    public GameObject Lance_L;

    #endregion


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        Lance_R.SetActive(false);
        Lance_L.SetActive(false);

        isDashing = false;
        canDash = true;

        HealthIMG.gameObject.SetActive(false);

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

        if (isDashing)
        {

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

    IEnumerator DashRight()
    {
        Lance_R.SetActive(true);
        Astar.canMove = false;
        yield return new WaitForSeconds(1f);
        Astar.canMove = true;
        _rigidBody.AddForce(-transform.right * dashForce);
        yield return new WaitForSeconds(1.5f);
        Lance_R.SetActive(false);
    }

    IEnumerator DashLeft()
    {
        Lance_L.SetActive(true);
        Astar.canMove = false;
        yield return new WaitForSeconds(1f);
        Astar.canMove = true;
        _rigidBody.AddForce(transform.right * dashForce);
        yield return new WaitForSeconds(1.5f);
        Lance_L.SetActive(false);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        yield return new WaitForSeconds(1f);
        isDashing = false;
    }

    IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(5f);
        canDash = true;
    }
}
