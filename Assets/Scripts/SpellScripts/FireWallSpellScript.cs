/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/5/2021
 * 
 * Brief:This scripts controls the behavior 
 * of the player's fire wall spell 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallSpellScript : MonoBehaviour
{
    #region Fire Wall Stats
        #region Fire Wall Base Stats

    public int fireWallCollideDamage;

    public int fireWallBurnDamage;

        #endregion
        #region Fire Wall Colliders/Renderers

    public Collider castCollider;
    public Renderer castRenderer;
    public Collider wallCollider;
    public Renderer wallRenderer;
    public Rigidbody castRigidbody;


    public bool changed = false;

    #endregion

    #endregion

    public void FixedUpdate()
    {
        ///<summary> switches the colliders and renderers enabled state of the cast's to the wall's</summary>
        if (changed == true)
        {
            castCollider.enabled = false;
            wallCollider.enabled = true;
            castRenderer.enabled = false;
            wallRenderer.enabled = true;
            
        }
    }



 

    #region Fire Wall Collison Control
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag =="platform")
        {
            castRigidbody.velocity = new Vector3(0f, 0f, 0f);
            changed = true;
        }
        if (other.gameObject.tag == "ground")
        {
            castRigidbody.velocity = new Vector3(0f, 0f, 0f);
            changed = true;

        }
        if (other.gameObject.tag == "Enemy")
        {
            if (changed == false)
            {
                castRigidbody.velocity = new Vector3(0f, -5, 0f);
            }
            
        }
    }
    #endregion
}
