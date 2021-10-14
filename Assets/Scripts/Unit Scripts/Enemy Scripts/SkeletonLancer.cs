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


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

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
        Astar.canMove = false;
        yield return new WaitForSeconds(1f);
        Astar.canMove = true;
        _rigidBody.AddForce(-transform.right * dashForce);
    }

    IEnumerator DashLeft()
    {
        Astar.canMove = false;
        yield return new WaitForSeconds(1f);
        Astar.canMove = true;
        _rigidBody.AddForce(transform.right * dashForce);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        yield return new WaitForSeconds(.5f);
        isDashing = false;
    }

    IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(5f);
        canDash = true;
    }
}
