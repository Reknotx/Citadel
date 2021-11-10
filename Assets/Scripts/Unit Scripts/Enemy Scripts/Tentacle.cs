/**
 * Author: Chase O'Connor
 * Date: 10/26/2021
 * 
 * Brief: Class file for Squiggmar's tentacles
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour, IDamageable
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

    public Player player;

    private bool attacking = false;
    public int damage;


    public Animator animator;

    public bool isAttacking = false;

    ///Tentacles have their own individual health bars
    ///and are attached to squiggmar.
    ///
    ///Once a tentacle is killed it needs tobe removed from combat
    ///and put into a neutral state so that it can be reactivated
    ///later on and doesn't need to be spawned in. 
    ///

    [SerializeField]
    private float _health;

    private float _maxHealth;

    public float Health 
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

                gameObject.SetActive(false);
            }

        }
    }

    private void Awake()
    {
        idlePos = transform.position;
    }

    private void Start()
    {
        player = Player.Instance;
    }

    private void FixedUpdate()
    {
        if (!trackPlayerY) return;

        if (animator != null)
        {
            animator.SetBool("isAttacking", isAttacking);

        }

        swipeStartPoint.y = player.transform.position.y;
        swipeEndPoint.y = swipeStartPoint.y;
        transform.position = new Vector3(swipeStartPoint.x, swipeStartPoint.y, 0f);

        
    }

    public void Swipe()
    {
        ///Determine if swiping from left to right, or right to left
        Squiggmar.Instance.TentacleSwiping = true;
        bool swipeFromRight = UnityEngine.Random.Range(0, 2) == 0;

        swipeStartPoint = new Vector3(swipeFromRight ? tentacleXOnRightWall : tentacleXOnLeftWall, 0, 0f);
        swipeEndPoint = new Vector3(swipeFromRight ? tentacleXOnLeftWall : tentacleXOnRightWall, 0, 0f);
            
        transform.eulerAngles = new Vector3(0, 0, swipeFromRight ? 90 : -90);

        isAttacking = true;

        StartCoroutine(SwipeMovement());

        trackPlayerY = true;
        ///Find the player's y position and set the tentacle's y position to that value
        ///After swipe is complete go back to neutral state
        

    }


    void ReturnToIdle()
    {
        isAttacking = false;
        transform.position = idlePos;
        transform.eulerAngles = Vector3.zero;
        attacking = false;
        Squiggmar.Instance.TentacleSwiping = false;

        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!attacking) return;

        if (other.gameObject == player.gameObject)
            player.TakeDamage(damage);
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

        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
        ReturnToIdle();
    }

    public void TakeDamage(float amount)
    {
        if (attacking)
            return;

        Health -= amount;
    }
}
