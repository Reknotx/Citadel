/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/2/2021
 * 
 * Brief:this script controls how the players 
 * melee weapon will react to the in game world 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    #region melee weapon colliders/renderers
    public Collider lightCollider;
    public Collider heavyCollider;


    public MeshRenderer lightRenderer;
    public MeshRenderer heavyRenderer;

    #endregion


    private void FixedUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        #region switches renderer on and off depending on the collider being enabled 
        if (lightCollider.enabled==true)
        {
            // lightCollider.gameObject.transform.localScale += new Vector3(player.GetComponent<Player>().meleeAttackRange, 0, 0);
            //lightCollider.gameObject.transform.localScale.x = player.GetComponent<Player>().meleeAttackRange;
            lightRenderer.enabled = true;
            /*for(double i=0; i < 10;)
            {
                lightRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, (float)i);
                i += 0.1;
            }*/
            lightRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
            
            
        }
        else
        {
            lightRenderer.enabled = false;
        }

        if (heavyCollider.enabled == true)
        {
            heavyRenderer.enabled = true;
            /*for (double i = 0; i <= 10;)
            {
                heavyRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, (float)i);
                i += 0.1;
            }*/


        }
        else
        {
            heavyRenderer.enabled = false;
        }
        #endregion
    }

    #region collision detection

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            lightCollider.enabled = false;
            heavyCollider.enabled = false;
        }
    }

    IEnumerator waitBetweenOpacity()
    {
        yield return new WaitForSeconds(0.5f);
    }
    #endregion
}
