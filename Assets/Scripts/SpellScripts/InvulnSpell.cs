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

    protected override void OnEnable()
    {
        base.OnEnable();
        TriggerSpell(player);
        StartCoroutine(delayDestroy());
    }

    protected override void TriggerSpell(GameObject target)
    {
        target.GetComponent<NewPlayer>().StartCoroutine(player.GetComponent<NewPlayer>().IFrames(invulnDurration));
    }

    protected override void Move()
    {
        //Activate the movement logic here
        return;
    }

    public void trackPlayer()
    {
        transform.position = new Vector3(player.transform.position.x, 
                                       player.transform.position.y + .8f,
                                         player.transform.position.z);
    }

    public IEnumerator delayDestroy()
    {
        yield return new WaitForSeconds(invulnDurration);
        Destroy(this.gameObject);
    }
}
