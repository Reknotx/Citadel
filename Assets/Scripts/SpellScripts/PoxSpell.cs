using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoxSpell : MonoBehaviour
{

    public int startSize = 3;
    public int minSize = 1;
    public int maxSize = 6;

    public float speed = 2.0f;

    private Vector3 targetScale;
    private Vector3 baseScale;
    private int currScale;


    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
        transform.localScale = baseScale * startSize;
        currScale = startSize;
        targetScale = baseScale * startSize;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, speed * Time.deltaTime);
        ChangeSize(true);
    }


    public void ChangeSize(bool bigger)
    {

        if (bigger)
            currScale++;
        else
            currScale--;

        currScale = Mathf.Clamp(currScale, minSize, maxSize + 1);

        targetScale = baseScale * currScale;
    }
}
