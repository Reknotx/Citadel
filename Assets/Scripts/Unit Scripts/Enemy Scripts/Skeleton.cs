/* Author: Andrew Nave
 * Date: 9/16/2021
 *
 * Brief: This script manages the skeletons basic properties and functions.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    #region Skeleton Stats

    public float skeletonShootingDistance;

    #endregion

    #region Projectile

    public GameObject projectile;

    private bool canShoot = true;

    public Transform shootLocation;
    #endregion

    /*public Image skeletonHealth;

    public Image HealthIMG;

    private float calculateHealth;*/

    public float currentHealth;

    public float runAwayDistance;

    public float runAwaySpeed;

    public bool los = false;

    public override void Start()
    {
        //Tyler Added Code
        player = GameObject.FindGameObjectWithTag("Player");
        //End
    }

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
            canShoot = false;
        }

        if (Vector2.Distance(transform.position, player.transform.position) <= skeletonShootingDistance && !isDead)
        {
            if (canShoot)
            {
                
                if (Mathf.Abs(yDistance) < 2)
                {

                    isAttacking = true;
                    //transform.LookAt(player.transform);
                    //shootLocation = player.transform;
                    Instantiate(projectile, shootLocation.position, Quaternion.identity);
                    StartCoroutine(ProjectileCooldown());
                }
            }
        }

        if (Vector2.Distance(transform.position, player.transform.position) < runAwayDistance && !isDead)
        {
            Astar.canMove = false;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -1 * runAwaySpeed * Time.deltaTime);
        }
        else
        {
            Astar.canMove = true;
        }
    }

    /*private bool CanSeePlayer(float distance)
    {
        bool val = false;

        float castDist = distance;

        if (facingRight)
        {
            castDist = -distance;
        }

        Vector2 endPos = shootLocation.position + Vector3.right * castDist;

        RaycastHit2D hit = Physics2D.Linecast(shootLocation.position, endPos, 1 << LayerMask.NameToLayer("Default"));

        if (hit.collider != null)
        {
            
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                val = true;
            }
            else
            {
                val = false;
            }
        }
        

        Debug.DrawLine(shootLocation.position, endPos, Color.blue);

        return val;
    }*/

    IEnumerator ProjectileCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }
}
