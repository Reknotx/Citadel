/**
 * Author: Chase O'Connor
 * Date: 10/26/2021
 * 
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squiggmar : MonoBehaviour
{
    ///Notes for Squiggmar
    ///1. The head needs to be invulnerable until all tentacles are killed.
    ///
    ///2. Need a way to reference the tentacles that he has
    ///to tell when they are dead and damage can be applied
    ///to the head.
    ///
    ///3. Squiggmar has two states, an invulnerable state when the 
    ///tentacles are still alive, and a vulnerable state.
    ///     3a. In the invulnerable state he attacks with his tentacles
    ///     
    ///     3b. In the vulnerable state he falls to the ground and 
    ///     can be damaged by the player. He remains in this state
    ///     for a few seconds, or until his health hits 0.
    ///
    ///4. Squiggmar's head should be kept away from the terrain
    ///and be allowed to pass through everything. There shouldn't
    ///be ay form of clipping happening that makes him behave
    ///weirdly and eratic.
    ///

    public static Squiggmar Instance;

    public List<Tentacle> tentacles = new List<Tentacle>();

    private float _health;

    [Tooltip("The position squiggmar is at when he is vulnerable.")]
    public Transform vulnerablePosition;

    public float Health 
    { 
        get => _health; 
        set
        {
            _health = value;

            ///Perform death logic to end the game/proceed to
            ///the next level
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;
    }

    public void Start()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Tentacle");

        foreach (GameObject item in temp)
        {
            tentacles.Add(item.GetComponent<Tentacle>());
        }
    }

    private List<Tentacle> alreadyAttacked = new List<Tentacle>();

    public void CombatLogic()
    {
        if (tentacles.Count == 0)
        {
            ///Squiggmar is now vulnerable
            
        }
        else
        {
            SelectTentacleForSwipe();
        }
    }


    private void SelectTentacleForSwipe()
    {
        int tentacleIndex = UnityEngine.Random.Range(0, tentacles.Count);

        alreadyAttacked.Add(tentacles[tentacleIndex]);
        tentacles[tentacleIndex].Swipe();
        tentacles.RemoveAt(tentacleIndex);
    }

    IEnumerator MoveHeadToVulnerableSpot()
    {

        bool moving = true;
        float startTime = Time.time;

        Vector3 p0 = transform.position, p1 = vulnerablePosition.position, p01;

        while (moving)
        {
            float u = (Time.time - startTime);

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
