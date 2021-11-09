using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoxSpell : Spell
{

    public int startSize = 1;
    public int minSize = 1;
    public int maxSize = 6;

    public float speed = 2.0f;

    private Vector3 targetScale;
    private Vector3 baseScale;
    private int currScale;

    public bool atMax = false;

    public float spellDuration;


    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
        transform.localScale = baseScale * startSize;
        currScale = startSize;
        targetScale = baseScale * startSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale,  Time.deltaTime / speed);
        ChangeSize(true);

        if (currScale == maxSize)
        {
            atMax = true;
        }

        if (atMax == true)
        {
            StartCoroutine(despawnCoroutine());
        }
    }


    public override void TriggerSpell(GameObject target)
    {
        target.GetComponent<IDamageable>().TakeDamage(stats.damage);
    }

    public override void Move()
    {
        ///Activate the movement logic here
        return;
    }

    public void ChangeSize(bool bigger)
    {

        if (bigger)
            currScale++;
        else
            currScale--;

        currScale = Mathf.Clamp(currScale, minSize, maxSize);

        targetScale = baseScale * currScale;

        
    }

    public IEnumerator despawnCoroutine()
    {

        yield return new WaitForSeconds(spellDuration);

        Destroy(this.gameObject);
        



    }
}
