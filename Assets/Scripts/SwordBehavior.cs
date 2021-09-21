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
        

        #region switches renderer on and off depending on the collider being enabled 
        if (lightCollider.enabled==true)
        {
            lightRenderer.enabled = true;
        }
        else
        {
            lightRenderer.enabled = false;
        }

        if (heavyCollider.enabled == true)
        {
            heavyRenderer.enabled = true;
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
    #endregion
}
