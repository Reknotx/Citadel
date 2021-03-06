using System.Collections;
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

    [HideInInspector]
    public int damage;

    [HideInInspector]
    public int manaCost;


    public Transform player;


    public void Awake()
    {
        
        movingSpell = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerAnimationManager.Instance.ActivateTrigger("castPox");
    }

    // Start is called before the first frame update
    void Start()
    {
        
        baseScale = transform.localScale;
        transform.localScale = baseScale * startSize;
        currScale = startSize;
        targetScale = baseScale * startSize;

        damage = stats.damage;
        manaCost = stats.manaCost;

        
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
        
        trackPlayer();
    }


    protected override void TriggerSpell(GameObject target)
    {
        target.GetComponent<IDamageable>().TakeDamage(stats.damage);
        DamagePopup.Create(transform.position, stats.damage);
    }

    protected override void Move()
    {
        //Activate the movement logic here
        return;
    }

    public void trackPlayer()
    {
        
       transform.position = player.position;
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

        Destroy(gameObject);
        



    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 31)
        {
            return;
        }

        if (other.gameObject.layer == 8)
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            other.gameObject.GetComponent<Enemy>().poisonedDamage = 3;
            other.gameObject.GetComponent<Enemy>().poisonedDuration = 5;
            other.gameObject.GetComponent<Enemy>().poisoned = true;
        }
    }

}
