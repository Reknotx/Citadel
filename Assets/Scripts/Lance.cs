/* Author: Andrew Nave
 * Date: 10/14/2021
 *
 * Brief: This script handles the damage done to the player for getting hit by the skeleton lancer.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour
{
    public Player playerLife;

    public float lanceDamage;

    private void Start()
    {
        playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerLife.TakeDamage(lanceDamage);
        }
    }
}
