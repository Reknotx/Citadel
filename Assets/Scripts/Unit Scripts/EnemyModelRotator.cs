using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModelRotator : MonoBehaviour
{
    public GameObject enemy;
    public bool facingRightLocal = true;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


        facingRightLocal = enemy.GetComponent<Enemy>().facingRight;

        checkDirection();
    }


    public void checkDirection()
    {
        
            if (facingRightLocal == true)
            {
                this.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                this.gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        
        

    }
}
