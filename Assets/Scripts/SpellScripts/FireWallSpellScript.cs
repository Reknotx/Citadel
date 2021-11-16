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

public class FireWallSpellScript : Spell
{

    

    #region Fire Wall Stats
    #region Fire Wall Base Stats

    public int fireWallCollideDamage;

    public int fireWallBurnDamage;

    public float spellDuration;

        #endregion
        #region Fire Wall Colliders/Renderers

    public Collider castCollider;
    public Renderer castRenderer;
    public Collider wallCollider;
    public Renderer wallRenderer;
    public Rigidbody castRigidbody;


    public bool changed = false;

    #endregion

    public GameObject ballParticles;
    public GameObject wallParticles;

    #endregion

    public void FixedUpdate()
    {
        ///<summary> switches the colliders and renderers enabled state of the cast's to the wall's</summary>
        if (changed == true)
        {
            ballParticles.SetActive(false);
            wallParticles.SetActive(true);
            castCollider.enabled = false;
            wallCollider.enabled = true;
            castRenderer.enabled = false;
            wallRenderer.enabled = true;
            StartCoroutine(SpellDuration());
        }
    }

    public override void Move()
    {
    }





    #region Fire Wall Collison Control
    public override void OnTriggerEnter(Collider other)
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

    public override void TriggerSpell(GameObject target)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    /// <summary> this destroys the spell object after the duration expires </summary>
    IEnumerator SpellDuration()
    {
        yield return new WaitForSeconds(spellDuration);
        Destroy(this.gameObject);
    }
}
