/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/5/2021
 * 
 * Brief:This scripts controls the behavior 
 * of traps in game 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    /// <summary>
    /// determines how much damage the trap deals every tick
    /// </summary>
    public float trapDamage;

    /// <summary>
    /// this determines if the trap can damage the player
    /// </summary>
    private bool canDamage = true;

    /// <summary>
    /// this determines how long of a wait between each tick of damage when the player is standing on the trap
    /// </summary>
    public float trapDamageInterval = 1f;
   

    public void Interact()
    {
       
        if(canDamage == true)
        {
            StartCoroutine(trapTrigger());
        }
       
    }

    /// <summary>
    /// damages the player every interval 
    /// </summary>
    public IEnumerator trapTrigger()
    {
        canDamage = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().myHealth -= trapDamage;
        yield return new WaitForSeconds(trapDamageInterval);
        canDamage = true;
        
    }

}
