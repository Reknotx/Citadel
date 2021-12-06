/*
 * Author: Chase O'Connor
 * Date: 10/26/2021
 * 
 * Brief: Class file for Squiggmar's tentacles
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tentacle : Enemy
{
    public Vector3 idlePos;
    private Vector3 swipeStartPoint, swipeEndPoint;
    public float swipeXPosRange;
    [Range(1, 5)]
    public float swipeCompletionTime = 1;

    private float tentacleXOnLeftWall = 1.5f;
    private float tentacleXOnRightWall = 28.5f;

    private float attackDelay = 2f;
    private bool trackPlayerY = false;

    public NewPlayer playerScript;

    private bool attacking = false;
    public int damage;




    /// huter added 
    /// holds the animator and collider's posistion
    /// 
   
    #region For Animations 
   
    public BoxCollider myCollider;

    [HideInInspector]
    public float colliderX;

    private float colliderY;

    private float colliderZ;

    private float colliderStartX;

    private float colliderStartY;

    private float colliderStartZ;

    private Vector3 colliderPos;

    #endregion

    ///Tentacles have their own individual health bars
    ///and are attached to squiggmar.
    ///
    ///Once a tentacle is killed it needs tobe removed from combat
    ///and put into a neutral state so that it can be reactivated
    ///later on and doesn't need to be spawned in. 
    ///
    //
    // [SerializeField]
    // private float _health;
    //
    // private float _maxHealth;


    public override float Health 
    { 
        get => _health; 
        set
        {
            _health = value;
            if (_health <= 0)
            {
                _health = 0;

                StopAllCoroutines();
                ReturnToIdle();
            }
        }
    }

    public override void Awake()
    {
        idlePos = transform.position;

        //hunter added
        //marks and stores the starting posistions of the collider so that it can be reset easily
        var size = myCollider.size;
        colliderStartX = size.x;
        colliderStartY = size.y;
        colliderStartZ = size.z;
        colliderPos = new Vector3(colliderStartX, colliderStartY, colliderStartZ);
        size = colliderPos;
        myCollider.size = size;
        myCollider.center = new Vector3(0, 2, 0);
    }

    public override void Start()
    {
        playerScript = NewPlayer.Instance;
    }

    public override void Update()
    {
        //hunter added
        //tracking health in update because the base health would not trigger as soon as health hit 0
        
        if (_health <= 0)
        {
            _health = 0;

            //this triggers the death animation and delays setting Active to false for a second to let the animation play
            isDead = true;
            animator.SetBool(IsDead, isDead);
            if (isDead)
            {
                StartCoroutine(turnOff());
            }
        }

        

        if (!trackPlayerY) return;

        //hunter added
        //tracks the animator and attached bools for activating animations
        if (animator != null)
        {
            animator.SetBool(IsAttacking, isAttacking);
            animator.SetBool(IsDead, isDead);
        }

        swipeStartPoint.y = playerScript.transform.position.y;
        swipeEndPoint.y = swipeStartPoint.y;
        transform.position = new Vector3(swipeStartPoint.x, swipeStartPoint.y, 0f);


        //hunter added 
        //controlls the position of the collider so that it stays up to date with the animated tenticle movements
        if(isAttacking == true)
        {
            colliderX = 1.5f;
            colliderY = 1.5f;
            colliderZ = 5.5f;

            colliderPos = new Vector3(colliderX, colliderY, colliderZ);
            myCollider.center = new Vector3(0, 0, 2);
        }
        myCollider.size = colliderPos;
    }

    public void Swipe()
    {
        //Determine if swiping from left to right, or right to left
        Squiggmar.Instance.TentacleSwiping = true;
        bool swipeFromRight = UnityEngine.Random.Range(0, 2) == 0;

        swipeStartPoint = new Vector3(swipeFromRight ? tentacleXOnRightWall : tentacleXOnLeftWall, 0, 0f);
        swipeEndPoint = new Vector3(swipeFromRight ? tentacleXOnLeftWall : tentacleXOnRightWall, 0, 0f);
          
        //hunter modified
        //changed the Y and Z rotations so that the tentecle would swipe in the correct direction
        transform.eulerAngles = new Vector3(0, swipeFromRight ? -90 : 90, swipeFromRight ? 180 : -180);

        

        isAttacking = true;

        StartCoroutine(SwipeMovement());

        trackPlayerY = true;
        //Find the player's y position and set the tentacle's y position to that value
        //After swipe is complete go back to neutral state
        

    }


    void ReturnToIdle()
    {
        isAttacking = false;
        transform.position = idlePos;
        transform.eulerAngles = Vector3.zero;
        attacking = false;
        Squiggmar.Instance.TentacleSwiping = false;
        

    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!attacking) return;

        if (other.gameObject == playerScript.gameObject)
            playerScript.TakeDamage(damage);
    }

    IEnumerator SwipeMovement()
    {
        yield return new WaitForSeconds(attackDelay);
        attacking = true;
        trackPlayerY = false;

        bool moving = true;
        float startTime = Time.time;

        Vector3 p0 = swipeStartPoint, p1 = swipeEndPoint, p01;

        while (moving)
        {
            float u = (Time.time - startTime) / swipeCompletionTime;

            if (u >= 1f)
            {
                u = 1;
                moving = false;
            }

            p01 = (1 - u) * p0 + u * p1;

            transform.position = p01;

            yield return new WaitForFixedUpdate();

        }

        AnimationEvents();
        ReturnToIdle();
    }

    public override void TakeDamage(float amount)
    {
        if (attacking)
            return;

        Health -= amount;
    }

    /// <summary>
    /// resets the animation and colliders position for the idle state
    /// </summary>
    public void AnimationEvents()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
        colliderPos = new Vector3(colliderStartX, colliderStartY, colliderStartZ);
        myCollider.size = colliderPos;
        myCollider.center = new Vector3(0, 2, 0);
    }


    /// <summary>
    /// delays the setActive to false for a second to give animations time to finish
    /// </summary>
    IEnumerator turnOff()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
