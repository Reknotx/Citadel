using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvulnSpell : Spell
{

    public GameObject player;
    public float invulnDurration;

    public void Awake()
    {

        movingSpell = false;
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerAnimationManager.Instance.ActivateTrigger("castPox");
    }

    private void OnEnable()
    {
        TriggerSpell(player);
    }

    public override void TriggerSpell(GameObject target)
    {
        target.GetComponent<NewPlayer>().IFrames(invulnDurration);
    }

    public override void Move()
    {
        ///Activate the movement logic here
        return;
    }
}
