/* Author: Andrew Nave
 * Date: 9/16/2021
 *
 * Brief: This script manages the projectile fired by the skeleton enemy.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    private Vector3 target;

    [SerializeField]
    private float projectileDamage;

    void Start()
    {
        Vector3 playerPos = NewPlayer.Instance.transform.position;

        target = new Vector3(playerPos.x, playerPos.y, playerPos.z);

    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
    void dealDamage()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            NewPlayer.Instance.TakeDamage(projectileDamage);
            
            DestroyProjectile();
        }

        if (other.tag == "ground")
        {
            DestroyProjectile();
        }

        if(other.tag == "Trap")
        {
            DestroyProjectile();
        }

        if(other.tag == "platform")
        {
            DestroyProjectile();
        }

        if(other.tag == "Pot")
        {
            DestroyProjectile();
        }

        
    }
/*
    public void OnCollisionEnter(Collision collision)
    {
        DestroyProjectile();
    }
    */
}
   
