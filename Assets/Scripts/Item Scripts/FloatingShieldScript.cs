using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class FloatingShieldScript : MonoBehaviour
{
    public bool isHit = false;

    public Collider myCollider;
    public Material myMaterial;
    public GameObject Player;


    // Start is called before the first frame update
    void Awake()
    {
        myCollider = this.gameObject.GetComponent<Collider>();

        Color alpha = myMaterial.color;
        alpha.a = 1f;
        myMaterial.color = alpha;
    }

    // Update is called once per frame
    void Update()
    {


        if (myCollider == enabled)
        {
            Player.GetComponent<Player>().shieldActive = true;
        }
        else
        {
            Player.GetComponent<Player>().shieldActive = false;
        }

        if (isHit == true)
        {

            StartCoroutine(wasHitCoroutine());
            isHit = false;
        }
    }


    public IEnumerator wasHitCoroutine()
    {
        Color alpha = myMaterial.color;
        alpha.a -= .5f;
        myMaterial.color = alpha;
        myCollider.enabled = false;
        yield return new WaitForSeconds(2f);
        alpha = myMaterial.color;
        alpha.a += .5f;
        myMaterial.color = alpha;
        myCollider.enabled = true;

    }
}
