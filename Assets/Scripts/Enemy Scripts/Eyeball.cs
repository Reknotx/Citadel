using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeball : Enemy
{
    Vector3 moveDir;

    public Transform playerTrans;

    public bool spottedPlayer;

    private void Awake()
    {
        StartCoroutine(WaitTillPlayerSpawns());
    }

    private void Start()
    {
        Bob();
    }

    protected override void Move()
    {
        //if (!spottedPlayer) return;
        /////Fire rays around the eyeball searching for walls.

        //moveDir = Player.Instance.transform.position - transform.position;

        //_rigidBody.MovePosition(transform.position + moveDir);
    }

    private void Bob()
    {
        StartCoroutine(Lerp());
    }

    private void WallDetection()
    {
        Physics.Raycast(transform.position, new Vector2(1, 0).normalized, out RaycastHit eastHit, 1, 1<<31);
        Physics.Raycast(transform.position, new Vector2(1, 1).normalized, out RaycastHit northEastHit, 1, 1<<31);
        Physics.Raycast(transform.position, new Vector2(0, 1).normalized, out RaycastHit northHit, 1, 1<<31);
        Physics.Raycast(transform.position, new Vector2(-1, 1).normalized, out RaycastHit northWestHit, 1, 1 << 31);
        Physics.Raycast(transform.position, new Vector2(-1, 0).normalized, out RaycastHit westHit, 1, 1<<31);
        Physics.Raycast(transform.position, new Vector2(-1, -1).normalized, out RaycastHit southWestHit, 1, 1<<31);
        Physics.Raycast(transform.position, new Vector2(0, -1).normalized, out RaycastHit southHit, 1, 1<<31);
        Physics.Raycast(transform.position, new Vector2(1, -1).normalized, out RaycastHit southEastHit, 1, 1<<31);

        ///I could use these rays to place a limit on the range of movement that the enemy can have
        ///For example. If the north one returns positive, then y can't be a certain value. To be more clear
        ///the movement vector can't be any value between north east and north west.
        ///If north returns true then x can't be between sqrt(2)/2 and -sqrt(2)/2
        ///     In terms of what I'm thinking of x can only be positive 1 or negative 1 
        ///     Of course this means that the movement direction vector will need to be adjusted as the vector
        ///     will be aimed towards the player at all times.


    }

    protected void Attack()
    {

    }

    bool movingVertical = false;

    IEnumerator Lerp()
    {

        bool lerping = true;
        float timeStart = Time.time;

        float p0 = 0f, p1 = 360f, p01 = 0;

        while (lerping)
        {
            if (movingVertical)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }

            float u = (Time.time - timeStart) / 1;
            if (u >= 1)
            {
                u = 1;
                lerping = false;
            }

            p01 = (1 - u) * p0 + u * p1;

            ///set the y-offset for bobbing here
            transform.GetChild(0).localPosition = new Vector3(0f, Mathf.Sin(Mathf.Deg2Rad * p01));

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator WaitTillPlayerSpawns()
    {
        yield return new WaitUntil(() => Player.Instance != null);

        playerTrans = Player.Instance.transform;
    }
}
