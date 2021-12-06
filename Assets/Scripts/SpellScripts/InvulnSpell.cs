using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvulnSpell : Spell
{

    public GameObject player;
    public float invulnDurration = 5f;

    public void Awake()
    {

        movingSpell = false;
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerAnimationManager.Instance.ActivateTrigger("castPox");
    }

    public void FixedUpdate()
    {
        trackPlayer();
    }

    private void OnEnable()
    {
        TriggerSpell(player);
    }

    protected override void TriggerSpell(GameObject target)
    {
        target.GetComponent<NewPlayer>().IFrames(invulnDurration);
    }

    protected override void Move()
    {
        ///Activate the movement logic here
        return;
    }

    public void trackPlayer()
    {

        //transform.position = player.transform.position;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
    }
}
