/* Author: Andrew Nave
 * Date: 10/10/2021
 *
 * Brief: This script controls the AI specific to the skeleton Lancer.
 */

using System.Collections;
using System.Collections.Generic;
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
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

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
        yield return new WaitForSeconds(1f);
        Lance_R.SetActive(false);
    }

    IEnumerator DashLeft()
    {
        Lance_L.SetActive(true);
        Astar.canMove = false;
        yield return new WaitForSeconds(1f);
        Astar.canMove = true;
        _rigidBody.AddForce(transform.right * dashForce);
        yield return new WaitForSeconds(1f);
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
