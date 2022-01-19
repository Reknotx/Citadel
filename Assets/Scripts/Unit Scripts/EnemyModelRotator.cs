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
        if(enemy.GetComponent<Enemy>().isDead == false)
        {
            if (facingRightLocal == true)
            {
                gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
    }
}
