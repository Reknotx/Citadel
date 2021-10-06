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

    

    private void Start()
    {
        
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(transform.position, player.transform.position) <= skeletonShootingDistance)
        {
            if (canShoot)
            {
                //transform.LookAt(player.transform);
                Instantiate(projectile, shootLocation.position, Quaternion.identity);
                StartCoroutine(ProjectileCooldown());
            }
        }
    }

    IEnumerator ProjectileCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }
}
