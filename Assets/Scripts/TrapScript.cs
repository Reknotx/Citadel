using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    public float trapDamage;

    private bool canDamage = true;

    

    public void Awake()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
       
        if(canDamage == true)
        {
            StartCoroutine(trapTrigger());
        }
       
    }


    public IEnumerator trapTrigger()
    {
        canDamage = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().myHealth -= trapDamage;
        yield return new WaitForSeconds(1f);
        canDamage = true;
        
    }

}
