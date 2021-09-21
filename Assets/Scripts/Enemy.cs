/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/1/2021
 * 
 * Brief:this script holds the stats and 
 * controls for the enemies of the game
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    #region Enemy Stats

            #region Enemy's Base Stats/Important Controls
    ///<summary>This is the units health.</summary>
    public double myHealth;

    ///<summary>This is the players Input system.</summary>
    private PlayerInputActions playerInputActions;

    
    



            #endregion
            #region Enemy's Ground/Directional Detection Stats

    ///<summary>This is the range of detection to the ground.</summary>
    private float _Reach = 1f;

    ///<summary>This tracks what direction the enemy is facing.</summary>
    [HideInInspector]
    public bool facingRightLocal;

            #endregion
            #region Enemy's Player Detection Stats

    ///<summary>This is the range of detection to the player.</summary>
    [Range(0, 20)]
    private float _DetectionRange;
    ///<summary>This tracks what the ground detection raycast hits.</summary>
    RaycastHit hit;

    ///<summary>This targets the player for the Enemy.</summary>
    public GameObject player;

    ///<summary>This is the distance the enemy must be within in order to move towards the player</summary>
    public float followDistance;

    ///<summary>This is the distance from the player the enemy wills top at</summary>
    public float stoppingDistance;

    #endregion

    #endregion

    #region Gold Handler

    public GoldHandler gold;
    public int goldPerKill = 100000;

    #endregion


    public override void Update()
    {

        base.Update();

        #region Enemy Movement
        if (Vector2.Distance(transform.position, player.transform.position) > stoppingDistance && Vector2.Distance(transform.position, player.transform.position) < followDistance) 
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

        #endregion


        #region Player Detection
        ///<summary>This sets the player as the target in the scene.</summary>
        player = GameObject.FindGameObjectWithTag("Player");

        #endregion


        #region Ground/Platform detection
        ///<summary>This determines whether the unit is on a platform or not.</summary>
        var groundCheck = transform.TransformDirection(Vector3.down);
        Debug.DrawRay(transform.position, groundCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, groundCheck, out hit, _Reach) && hit.transform.tag == "platform")
        {
            onPlatform = true;

        }
        else
        {
            onPlatform = false;

        }

        ///<summary>This determines whether the unit is on the ground or not.</summary>
        Debug.DrawRay(transform.position, groundCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, groundCheck, out hit, _Reach) && hit.transform.tag == "ground")
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }


        var roofCheck = transform.TransformDirection(Vector3.up);
        Debug.DrawRay(transform.position, roofCheck * _Reach, Color.red);
        if (Physics.Raycast(transform.position, roofCheck, out hit, _Reach) && hit.transform.tag == "platform")
        {
            throughPlatform = true;

        }
        else
        {
            throughPlatform = false;

        }
        #endregion

        ///<summary> this damages the enemy over time if they are on fire</summary>
        if (onFire == true)
        {
            myHealth -= onFireDamage * Time.deltaTime;
            
        }

        #region On Death

        if (myHealth <= 0)
        {
            Destroy(this.gameObject);
            gold.AddSoftGold(goldPerKill);
        }

        #endregion
    }


    #region Enemy Actions

    ///<summary>this makes the unit move between points A and B.</summary>
    public void patrolAB()
    {
        return;
    }


    #endregion

    #region Collision Detection
    ///<summary>These track the collisions between the enemy and in-game objects .</summary>
    public void OnTriggerEnter(Collider other)
    {
        ///<summary>This triggers when the enemy is hit with the light attack.</summary>
        if (other.gameObject.tag=="swordLight")
        {
            myHealth = myHealth - player.GetComponent<Player>().lightAttackDamage;
            hitOnRight = player.GetComponent<Player>().facingRightLocal ;

            /*
            if (hitOnRight == true)
            {
                _rigidBody.AddForce(new Vector3(1, 0, 0) * 1f, ForceMode.Impulse);

            }
            else
            {
                _rigidBody.AddForce(new Vector3(-1, 0, 0) * 1f, ForceMode.Impulse);
            }
            */
        }

        ///<summary>This triggers when the enemy is hit with the heavy attack.</summary>
        if (other.gameObject.tag == "swordHeavy")
        {
            
            myHealth = myHealth - player.GetComponent<Player>().heavyAttackDamage;
            hitOnRight = player.GetComponent<Player>().facingRightLocal;
            

            if(hitOnRight == true)
            {
                _rigidBody.AddForce(new Vector3(player.GetComponent<Player>().knockbackForce, 0, 0) * 1f, ForceMode.Impulse);
             
            }
            else
            {
                _rigidBody.AddForce(new Vector3(-player.GetComponent<Player>().knockbackForce, 0, 0) * 1f, ForceMode.Impulse);
               
            }
        }
        if (other.gameObject.tag == "Player")
        {
            myHealth = myHealth - player.GetComponent<Player>().playerCollisionDamage; 
        }

        if (other.gameObject.tag == "FireWallCast")
        {
            if(fireDamageTaken == false)
            {
                myHealth -= other.GetComponent<FireWallSpellScript>().fireWallCollideDamage;
                fireDamageTaken = true;
            }
        }

        if (other.gameObject.tag == "FireWallWall")
        {
            if(onFire == false)
            {
                onFireDuration = 5f;
                onFireDamage = 1;
                StartCoroutine(onFireCoroutine());
            }
        }
    }

    #endregion

}
