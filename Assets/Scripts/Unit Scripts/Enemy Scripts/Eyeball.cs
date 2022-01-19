/*
 * Author: Chase O'Connor
 * Date: 9/23/2021
 * 
 * Brief: Handles the logic for the flying eyeball enemy like attacking, 
 * bobbing, movement, etc.
 * 
 */

using System.Collections;
using UnityEngine;
using Pathfinding;

public class Eyeball : Enemy
{
    Vector3 moveDir;


    private bool _spottedPlayer = true, _bobbing = true;

    private IEnumerator BobCR;

    private bool _attacking = false;

    private bool canAttack;
    public float attackRate = 5f;
    private float attackCooldown = 0;
    public float lurchDist = 5f;

    public AudioSource attack;
    public AudioSource die;
    public AudioSource idle;
    public AudioSource hurt;

    #region Pathfinding
    public Transform playerTrans;

    public float seekerSpeed = 200f;
    //public float nextWaypointDistance = 3f;

    Path path;

    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    
    public GameObject enemyModel;
    #endregion

    private bool startAttackCooldown = false;
    private bool Attacking
    {
        get => _attacking;

        set
        {
            _attacking = value;

            if (value)
            {
                ///Start the attack function.
                Debug.Log("Starting attack lurch coroutine");
                StartCoroutine(AttackLurch());
            }
        }
    }

    [HideInInspector]
    public bool SpottedPlayer
    {
        get => _spottedPlayer;
        set => _spottedPlayer = value;
    }

    public override void Start()
    {
        playerTrans = NewPlayer.Instance.transform;
        Bob();
        Attacking = false;
        startAttackCooldown = true;
        seeker = GetComponent<Seeker>();

        idle.Play();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    public override void Update()
    {
        base.Update();
        if (transform.position.x - player.transform.position.x > 0)
        {
            facingRight = true;

        }

        if (transform.position.x - player.transform.position.x < 0)
        {
            facingRight = false;
        }

        if (isDead)
        {
            die.Play();
        }

    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(_rigidBody.position, playerTrans.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected override void Move()
    {
        if (!SpottedPlayer) return;

        if (Attacking)
            return;
        else if (startAttackCooldown)
        {
            //Debug.Log("Adding to attack cooldown");
            attackCooldown += Time.deltaTime;
            if (attackCooldown >= attackRate)
                canAttack = true;
            else
            {
                canAttack = false;
                isAttacking = false;
            }
               

        }

        #region pathfinding
        PathFinding();
        #endregion


        if (Vector3.Distance(transform.position, playerTrans.position) <= lurchDist
            && Physics.Raycast(transform.position, (playerTrans.position - transform.position).normalized, out RaycastHit info, 100f))
        {
            if (info.collider.gameObject.layer == 7 && canAttack)
            {
                Debug.Log("Direct path to the player");
                _bobbing = false;
                Attacking = true;
                isAttacking = true;
                return;
            }
            else
            {
                //Debug.Log(info.collider.gameObject.name + " Layer: " + info.collider.gameObject.layer);
                startAttackCooldown = true;
            }
        }

        ///Fire rays around the eyeball searching for walls.

        #region Starting and stopping the bob
        moveDir = playerTrans.position - transform.position;
        float angle = playerTrans.position.y < transform.position.y 
                                             ? 360 - Vector3.Angle(moveDir, transform.right) 
                                             : Vector3.Angle(moveDir, transform.right);
        
        bool movingVert;
        
        if ((angle > 45 && angle <= 90)
            || (angle > 90 && angle < 135)
            || (angle > 225 && angle <= 270)
            || (angle > 270 && angle < 315))
            movingVert = true;
        else
            movingVert = false;
        
        if (movingVert)
            ///Cancle the bob
            _bobbing = false;
        else if (_bobbing == false && !movingVert)
            Bob();
        #endregion   

        void PathFinding()
        {
            if (path == null) return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
                reachedEndOfPath = false;

            Vector2 dir = (Vector2)(path.vectorPath[currentWaypoint] - _rigidBody.position).normalized;
            Vector2 force = dir * seekerSpeed * Time.deltaTime;

            _rigidBody.AddForce(force);

            float dist = Vector2.Distance(_rigidBody.position, path.vectorPath[currentWaypoint]);

            if (dist < nextWaypointDistance)
                currentWaypoint++;

            if (force.x >= 0.01f)
                enemyModel.transform.localScale = new Vector3(-1f, 1f, 1f);
            else if (force.x <= -0.01f)
                enemyModel.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void DmgPlayer()
    {
        ///Damage the player and set attacking to false
        Player.Instance.TakeDamage(1);
        attack.Play();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        hurt.Play();
    }

    #region Bobbing
    /// <summary> Starts the bobbing coroutine. </summary>
    private void Bob()
    {
        _bobbing = true;
        BobCR = BobbingLerp();
        StartCoroutine(BobCR);
    }

    [Range(0f, 0.8f)]
    public float BobDistance = 0.8f;

    IEnumerator BobbingLerp()
    {

        bool lerping = true;
        float timeStart = Time.time;

        float p0 = 0f, p1 = 360f, p01 = 0;

        while (lerping)
        {
            float u = (Time.time - timeStart) / 1;
            if (u >= 1)
            {
                u = 1;
                lerping = false;
            }

            p01 = (1 - u) * p0 + u * p1;

            ///set the y-offset for bobbing here
            transform.GetChild(0).localPosition = new Vector3(0f, BobDistance * Mathf.Sin(Mathf.Deg2Rad * p01));

            yield return new WaitForFixedUpdate();
        }
        if (_bobbing) StartCoroutine(BobbingLerp());
    }
    #endregion


    IEnumerator AttackLurch()
    {
        bool moving = true;
        float lurchSpeed = 2f;
        float timeStart = Time.time;

        Vector3 startPos = transform.position;
        Vector3 p1 = playerTrans.position;
        Vector3 p01;

        while(moving)
        {
            float u = ((Time.time - timeStart) * lurchSpeed) / 1;
            if (u >= 1)
            {
                u = 1;
                moving = false;
            }

            p01 = (1 - u) * startPos + u * p1;

            _rigidBody.MovePosition(p01);
            //Debug.Log("moving to player");

            yield return new WaitForFixedUpdate();
        }

        moving = true;
        Vector3 p2 = transform.position;
        timeStart = Time.time;

        while (moving)
        {
            float u = (Time.time - timeStart) / 1;
            if (u >= 1)
            {
                u = 1;
                moving = false;
            }

            p01 = (1 - u) * p2 + u * startPos;

            _rigidBody.MovePosition(p01);
            //Debug.Log("Moving back to start pos.");

            yield return new WaitForFixedUpdate();
        }

        Attacking = false;
        attackCooldown = 0;
    }
}
