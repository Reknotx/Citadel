using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    
    public Collider lightCollider;
    public Collider heavyCollider;


    public MeshRenderer lightRenderer;
    public MeshRenderer heavyRenderer;

    private void FixedUpdate()
    {
        if(lightCollider.enabled==true)
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
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            lightCollider.enabled = false;
            heavyCollider.enabled = false;
        }
    }
}
