using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMesh textMesh;

    public GameObject pfDamagePopup;

    float moveYSpeed = 20f;

    private float disappearTimer;
    private Color textColor;
    float disappearSpeed = 3f;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    float increaseScaleAmount = 1f;
    float decreaseScaleAmount = 1f;

    private Vector3 moveVector;

    public GameObject popupText;

    public static DamagePopup Create(Vector3 position, float damageAmount)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);


        return damagePopup;
    }

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMesh>();
        textColor = textMesh.color;
    }

    public void Setup(float damageAmount)
    {
        textMesh.text = "" + damageAmount;
        textColor = textMesh.color;
        disappearTimer = 1f;

        moveVector = new Vector3(.7f, 1) * 15f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }
}
