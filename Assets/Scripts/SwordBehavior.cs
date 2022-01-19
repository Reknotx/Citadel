/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/2/2021
 * 
 * Brief:this script controls how the players 
 * melee weapon will react to the in game world 
 */

using System.Collections;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    #region melee weapon colliders/renderers
    public Collider lightCollider;
    public Collider heavyCollider;


    public MeshRenderer lightRenderer;
    public MeshRenderer heavyRenderer;

    public float fadeInSpeed = 0.0000001f; 
    public float fadeOutSpeed = 0.0000001f;

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
            
            lightRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
            
            
        }
        else
        {
            lightRenderer.enabled = false;
        }

        if (heavyCollider.enabled == true)
        {
            heavyRenderer.enabled = true;
            StartCoroutine(FadeInObjectHeavy());
        }
        else
        {
            StartCoroutine(FadeOutObjectHeavy());
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
        else return;

        if (other.GetComponent<IDamageable>() != null)
            other.GetComponent<IDamageable>().TakeDamage(Player.Instance.meleeAttackDamage);
        else if (other.transform.parent.GetComponent<IDamageable>() != null)
            other.transform.parent.GetComponent<IDamageable>().TakeDamage(Player.Instance.meleeAttackDamage);

    }

    public IEnumerator FadeOutObjectHeavy()
    {
        
        while (heavyRenderer.material.color.a > 0)
        {
            Color objectColor = heavyRenderer.material.color;
            float fadeAmount = objectColor.a - ((fadeOutSpeed * Time.deltaTime) / 5);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            heavyRenderer.material.color = objectColor;
            yield return null;
        }
        //heavyRenderer.enabled = false;
    }

    public IEnumerator FadeInObjectHeavy()
    {
        //heavyRenderer.enabled = true;
        while (heavyRenderer.material.color.a < 1)
        {
            Color objectColor = heavyRenderer.material.color;
            float fadeAmount = objectColor.a + ((fadeInSpeed * Time.deltaTime) / 100);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            heavyRenderer.material.color = objectColor;
            yield return null;
        }
        
    }
    #endregion
}
