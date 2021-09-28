/*
 * Author: Chase O'Connor
 * Date: 9/23/2021
 * 
 * Brief: Handles the logic for the flying eyeball enemy like attacking, 
 * bobbing, movement, etc.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeball : Enemy
{
    Vector3 moveDir;

    public Transform playerTrans;

    private bool _spottedPlayer = true, _bobbing = true;

    private IEnumerator BobCR;

    private bool _attacking = false;

    Vector3 posBeforeAttack;

    private bool canAttack;
    private float attackRate = 3f;
    private float attackCooldown;

    private bool Attacking
    {
        get => _attacking;

        set
        {
            if (attackCoolDown < attackRate) return;

            _attacking = value;
            if (value)
            {
                ///Start the attack function.
                posBeforeAttack = transform.position;
                attackCoolDown = 0;
            }
            //else
        }
    }

    [HideInInspector]
    public bool SpottedPlayer
    {
        get => _spottedPlayer;
        set => _spottedPlayer = value;
    }

    private void Start()
    {
        playerTrans = Player.Instance.transform;
        Bob();
    }

    protected override void Move()
    {
        if (Attacking)
            return;
        else
            attackCoolDown += Time.deltaTime;

        if (!SpottedPlayer) return;
        else if (Vector3.Distance(transform.position, playerTrans.position) <= 5f
            && Physics.Raycast(transform.position, (playerTrans.position - transform.position).normalized, out RaycastHit info, 100f))
        {
            if (info.collider.gameObject.layer == 7)
            {
                //Debug.Log("Direct path to the player");
                _bobbing = false;
                Attacking = true;
                return;
            }
            else
            {
                //Debug.Log(info.collider.gameObject.name + " Layer: " + info.collider.gameObject.layer);
            }
        }

        ///Fire rays around the eyeball searching for walls.

        moveDir = playerTrans.position - transform.position;
        #region Starting and stopping the bob
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

        _rigidBody.MovePosition(transform.position + (moveDir.normalized * speed * Time.deltaTime));
    }


    private void WallDetection()
    {
        float maxX = 1f, minX = -1f, maxY = 1f, minY = -1f;

        bool northHit = Physics.Raycast(transform.position, new Vector2(0, 1).normalized, out RaycastHit northHitInfo, 1, 1 << 31);
        bool northEastHit = Physics.Raycast(transform.position, new Vector2(1, 1).normalized, out RaycastHit northEastHitInfo, 1, 1 << 31);
        bool northWestHit = Physics.Raycast(transform.position, new Vector2(-1, 1).normalized, out RaycastHit northWestHitInfo, 1, 1 << 31);
        bool southHit = Physics.Raycast(transform.position, new Vector2(0, -1).normalized, out RaycastHit southHitInfo, 1, 1 << 31);
        bool eastHit = Physics.Raycast(transform.position, new Vector2(1, 0).normalized, out RaycastHit eastHitInfo, 1, 1<<31);
        bool westHit = Physics.Raycast(transform.position, new Vector2(-1, 0).normalized, out RaycastHit westHitInfo, 1, 1<<31);
        bool southWestHit = Physics.Raycast(transform.position, new Vector2(-1, -1).normalized, out RaycastHit southWestHitInfo, 1, 1<<31);
        bool southEastHit = Physics.Raycast(transform.position, new Vector2(1, -1).normalized, out RaycastHit southEastHitInfo, 1, 1<<31);

        if (southEastHit || southWestHit || southHit)
        {

        }


        ///I could use these rays to place a limit on the range of movement that the enemy can have
        ///For example. If the north one returns positive, then y can't be a certain value. To be more clear
        ///the movement vector can't be any value between north east and north west.
        ///If north returns true then x can't be between sqrt(2)/2 and -sqrt(2)/2
        ///     In terms of what I'm thinking of x can only be positive 1 or negative 1 
        ///     Of course this means that the movement direction vector will need to be adjusted as the vector
        ///     will be aimed towards the player at all times.


    }

    public void DmgPlayer()
    {
        ///Damage the player and set attacking to false
        Player.Instance.TakeDamage(1);
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

}
