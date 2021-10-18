/* Author: Andrew Nave
 * Date: 9/18/2021
 *
 * Brief: This script manages the orcs basic properties and functions.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Orc : Enemy
{

    #region Orc Stats

    [SerializeField]
    private float orcMeleeRange;

    public float orcDamage;

    private bool canAttack = true;
    #endregion

    #region Life Handler for Player

    public Player playerLife;

    #endregion

    #region Orc Attack Visuals

    public GameObject orcAttack_L;
    public GameObject orcAttack_R;

    #endregion

    public Image orcHealth;

    public Image HealthIMG;

    private float calculateHealth;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //Tyler added code
        player = GameObject.FindGameObjectWithTag("Player");
        //end
        orcAttack_L.SetActive(false);
        orcAttack_R.SetActive(false);

        playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        HealthIMG.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (myHealth < maxHealth)
        {
            HealthIMG.gameObject.SetActive(true);
            calculateHealth = (float)myHealth / maxHealth;
            orcHealth.fillAmount = Mathf.MoveTowards(orcHealth.fillAmount, calculateHealth, Time.deltaTime);
        }
        else
        {
            HealthIMG.gameObject.SetActive(false);
        }

        if (Vector2.Distance(transform.position, player.transform.position) <= orcMeleeRange)
        {
            if (canAttack)
            {
                OrcAttack();
                
                StartCoroutine(playerLife.StunPlayer());
                
            }
        }
    }

    private void OrcAttack()
    {
        if (facingRight)
        {
            StartCoroutine(WaitBetweenVisual_Right());
            playerLife.TakeDamage(orcDamage);
            StartCoroutine(WaitBetweenAttack());
            
        }
        else
        {
            StartCoroutine(WaitBetweenVisual_Left());
            playerLife.TakeDamage(orcDamage);
            StartCoroutine(WaitBetweenAttack());
        }
    }

    IEnumerator WaitBetweenAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(3f);
        canAttack = true;
    }

    IEnumerator WaitBetweenVisual_Right()
    {
        orcAttack_R.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        orcAttack_R.SetActive(false);
    }

    IEnumerator WaitBetweenVisual_Left()
    {
        orcAttack_L.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        orcAttack_L.SetActive(false);
    }

    
}
