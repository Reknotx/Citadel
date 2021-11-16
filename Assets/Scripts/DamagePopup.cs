using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMesh textMesh;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMesh>();
    }

    public void Setup(int damageAmount)
    {
        textMesh.text = "" + damageAmount;
    }
}
