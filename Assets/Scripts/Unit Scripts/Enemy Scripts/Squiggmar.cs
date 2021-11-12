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

public class Squiggmar : MonoBehaviour, IDamageable
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


    [Tooltip("The position squiggmar is at when he is vulnerable.")]
    public Transform vulnerablePosition;

    private float tentacleXOnLeftWall = 1.5f;
    private float tentacleXOnRightWall = 28.5f;

    private int _minTentacleXPos = 3;
    private int _maxTentacleXPos = 27;

    public float vulnerableTime = 7f;

    private bool _tentacleSwiping = false;
    public bool TentacleSwiping 
    { 
        get => _tentacleSwiping;
        set
        {
            _tentacleSwiping = value;
            if (!value)
                nextCombatLogicStart = Time.time + 2f;
        }
    }
    private List<Tentacle> alreadyAttacked = new List<Tentacle>();

    float combatDelayTime = 5f;
    float nextCombatLogicStart;

    public GameObject endGameMenu;

    [SerializeField]
    private bool headVulnerable = false;

    public int GetActiveTentacles
    {
        get
        {
            int count = 0;

            foreach (Tentacle tentacle in tentacles)
            {
                if (tentacle.gameObject.activeSelf) count++;
            }

            return count;
        }
    }
    private float _maxHealth;

    [SerializeField]
    private float _health;
    public float Health 
    { 
        get => _health; 
        set
        {
            _health = value;
            Debug.Log("Squiggmar health is now " + _health);
            endGameMenu.SetActive(true);
            Destroy(gameObject);
            ///Perform death logic to end the game/proceed to
            ///the next level
        }
    }




    public bool isGoingUp = false;
    public bool isDead = false;

    public Animator animator;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;

    }

    public void Start()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);

        _maxHealth = Health;

        foreach (Transform child in transform)
        {
            if (child.GetComponent<Tentacle>() != null)
                tentacles.Add(child.GetComponent<Tentacle>());
        }

        nextCombatLogicStart = Time.time + combatDelayTime;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void FixedUpdate()
    {

        if (_health <= 0)
        {
            _health = 0;

            ///this triggers the death animation and delays setting Active to false for a second to let the animation play
            isDead = true;
            animator.SetBool("isDead", isDead);
            if (isDead)
            {
                StartCoroutine(turnOff());
            }
        }

        ///hunter added
        ///tracks the animator and attached bools for activating animations
        if (animator != null)
        {
            animator.SetBool("isGoingUp", isGoingUp);
            animator.SetBool("isDead", isDead);
        }


        if (Time.time > nextCombatLogicStart)
            CombatLogic();
    }


    public void CombatLogic()
    {
        if (GetActiveTentacles == 0)
        {
            if (!headVulnerable)
            {
                ///Squiggmar is now vulnerable
                headVulnerable = true;
                StartCoroutine(HeadVulnerableTime());
            }
        }
        else if (!TentacleSwiping)
        {
            SelectTentacleForSwipe();
        }
    }


    private void SelectTentacleForSwipe()
    {
        if (GetActiveTentacles == 0) return;
        
        while (true)
        {
            int tentacleIndex = UnityEngine.Random.Range(0, tentacles.Count);

            if (!tentacles[tentacleIndex].gameObject.activeSelf)
            {
                alreadyAttacked.Add(tentacles[tentacleIndex]);
                continue;
            }

            tentacles[tentacleIndex].Swipe();
            break;
        }

    }

    IEnumerator HeadVulnerableTime()
    {
        bool moving = true;
        float startTime = Time.time;

        isGoingUp = true;
        animator.SetBool("isGoingUp", isGoingUp);

        transform.GetChild(0).gameObject.SetActive(true);
        Vector3 invulnerablePos = transform.GetChild(0).position, vulnerablePos = vulnerablePosition.position, p01;

        while (moving)
        {
            float u = (Time.time - startTime) * 2;

            if (u >= 1f)
            {
                u = 1;
                moving = false;
            }

            p01 = (1 - u) * invulnerablePos + u * vulnerablePos;

            transform.GetChild(0).position = p01;

            yield return new WaitForFixedUpdate();

        }

        isGoingUp = false;
        animator.SetBool("isGoingUp", isGoingUp);

        Debug.Log("Head made vulnerable.");
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(vulnerableTime);

        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
        transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;

        moving = true;
        startTime = Time.time;

        while (moving)
        {
            float u = (Time.time - startTime) * 2;

            if (u >= 1f)
            {
                u = 1;
                moving = false;
            }

            p01 = (1 - u) * vulnerablePos + u * invulnerablePos;

            transform.GetChild(0).position = p01;

            yield return new WaitForFixedUpdate();

        }

        headVulnerable = false;
        nextCombatLogicStart = Time.time + 2;
        transform.GetChild(0).gameObject.SetActive(false);
        Debug.Log("Head made invulnerable.");


    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
    }

    /// <summary>
    /// delays the setActive to false for a second to give animations time to finish
    /// </summary>
    IEnumerator turnOff()
    {
        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
    }
}
