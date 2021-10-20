/* Author: Andrew Nave
 * Date: 9/16/2021
 *
 * Brief: This script manages the skeletons basic properties and functions.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Image skeletonHealth;

    public Image HealthIMG;

    private float calculateHealth;

    public float currentHealth;

    public override void Start()
    {
        base.Start();
        //Tyler Added Code
        player = GameObject.FindGameObjectWithTag("Player");
        //End

        HealthIMG.gameObject.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

     

        if (Health < maxHealth)
        {
            HealthIMG.gameObject.SetActive(true);
            calculateHealth = (float)Health / maxHealth;
            skeletonHealth.fillAmount = Mathf.MoveTowards(skeletonHealth.fillAmount, calculateHealth, Time.deltaTime);
        }
        else
        {
            HealthIMG.gameObject.SetActive(false);
        }

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
