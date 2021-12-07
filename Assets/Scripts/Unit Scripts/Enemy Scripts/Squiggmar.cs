/*
 * Author: Chase O'Connor
 * Date: 10/26/2021
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squiggmar : Enemy, IDamageable
{
    public static Squiggmar Instance;

    public List<Tentacle> tentacles = new List<Tentacle>();


    [Tooltip("The position squiggmar is at when he is vulnerable.")]
    public Transform vulnerablePosition;

    private float tentacleXOnLeftWall = 1.5f;
    private float tentacleXOnRightWall = 28.5f;

    private int _minTentacleXPos = 3;
    private int _maxTentacleXPos = 27;

    public float vulnerableTime = 7f;

    public AudioSource die;

    private bool _tentacleSwiping;
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
    private bool headVulnerable;

    private int GetActiveTentacles
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
        
    public override float Health 
    { 
        get => _health; 
        set
        {
            _health = value;
            Debug.Log("Squiggmar health is now " + _health);

            if (_health <= 0)
            {
                endGameMenu.SetActive(true);
             
               
                //Perform death logic to end the game/proceed to
                //the next level
            }
        }
    }
    
    public bool isGoingUp = false;
    public Transform headPos;

    public override void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;
        Health = MaxHealth;
    }

    public override void Start()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.5f);

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

        //hunter added
        //tracks the health real time incase of sudden enemy death 
        if (_health <= 0)
        {

            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;

            //this triggers the death animation and delays setting Active to false for a second to let the animation play
            isDead = true;
            animator.SetBool("isDead", isDead);
            if (isDead)
            {
                StartCoroutine(turnOff());
            }
        }

        //hunter added
        //tracks the animator and attached bools for activating animations
        if (animator != null)
        {
            animator.SetBool("isGoingUp", isGoingUp);
            animator.SetBool("isDead", isDead);
        }

       GetComponent<BoxCollider>().center =  headPos.position;


        if (Time.time > nextCombatLogicStart)
            CombatLogic();
    }

    public override void Update()
    {
        if (isDead)
        {
            die.Play();
        }
    }


    public void CombatLogic()
    {
        if (GetActiveTentacles == 0 && !headVulnerable)
        {
            //Squiggmar is now vulnerable
            headVulnerable = true;
            StartCoroutine(HeadVulnerableTime());
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

        //hunter added 
        //starts the going up animation 
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

        //hunter added
        //ends the going up animation 
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
