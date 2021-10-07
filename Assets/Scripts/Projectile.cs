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

    private Transform player;
    private Vector3 target;

    public LifeManaHandler playerHealth;

    [SerializeField]
    private float projectileDamage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector3(player.position.x, player.position.y, player.position.z);

        playerHealth = GameObject.FindGameObjectWithTag("ManaHealthController").GetComponent<LifeManaHandler>();

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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DestroyProjectile();
            playerHealth.Damage(projectileDamage);
            
        }

        if (other.tag == "ground")
        {
            DestroyProjectile();
        }
    }
}
   
