using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mine : MonoBehaviour
{
    /// <summary> THIS IS A TEMPORARY VARIABLE TO BE SWAPPED OUT FOR THE TRUE SYSTEM. </summary>
    private static int GoldGained = 0;

    public float speed = 1f;
    public float baseTimeToComplete = 1;
    public int goldOnFill = 1;

    public Text goldText;

    private float _progress = 0f;


    public Image fillImage;

    bool refill = true;

    public float Progress
    {
        get => _progress;

        set
        {
            _progress = Mathf.Clamp(value * baseTimeToComplete, 0, baseTimeToComplete);

            if (_progress >= baseTimeToComplete)
            {
                ///Add gold
                GoldGained += goldOnFill;
                Progress = 0;
                if (goldText != null)
                    goldText.text = "Gold gained: " + GoldGained;
            }

        }
    }

    public void FixedUpdate()
    {
        if (Progress == 0f && refill)
        {
            refill = false;
            StartCoroutine(Lerp());
        }
    }

    IEnumerator Lerp()
    {

        bool lerping = true;
        float timeStart = Time.time;

        float p0 = 0f;
        float p1 = 1;
        float p01;


        while (lerping)
        {
            float u = ((Time.time - timeStart) * speed) / baseTimeToComplete;
            if (u >= 1)
            {
                u = 1;
                lerping = false;
            }
            
            p01 = (1 - u) * p0 + u * p1;

            Progress = p01;
            fillImage.fillAmount = p01;

            yield return new WaitForFixedUpdate();
        }
        refill = true;
    }

}
