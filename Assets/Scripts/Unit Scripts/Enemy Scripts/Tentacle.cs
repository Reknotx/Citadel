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

public class Tentacle : MonoBehaviour
{
    public Transform idleLocation;
    private Vector3 swipeStartPoint, swipeEndPoint;
    public float swipeXPosRange;
    [Range(1, 5)]
    public float swipeCompletionTime = 1;

    ///Tentacles have their own individual health bars
    ///and are attached to squiggmar.
    ///
    ///Once a tentacle is killed it needs tobe removed from combat
    ///and put into a neutral state so that it can be reactivated
    ///later on and doesn't need to be spawned in. 
    ///

    private float _health;

    public float Health { get => _health; set => _health = value; }

    public void Swipe()
    {
        ///Determine if swiping from left to right, or right to left

        bool swipeFromRight = UnityEngine.Random.Range(0, 2) == 0;

        swipeStartPoint = new Vector3(swipeFromRight ? swipeXPosRange : -swipeXPosRange, Player.Instance.transform.position.y, 0f);
        swipeEndPoint = new Vector3(swipeStartPoint.x * -1, Player.Instance.transform.position.y, 0f);

        StartCoroutine(SwipeMovement());

        ///Find the player's y position and set the tentacle's y position to that value
        ///After swipe is complete go back to neutral state
        

    }


    IEnumerator SwipeMovement()
    {
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

    }
}
