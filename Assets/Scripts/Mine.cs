using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mine : MonoBehaviour
{
    /// <summary> THIS IS A TEMPORARY VARIABLE TO BE SWAPPED OUT FOR THE TRUE SYSTEM. </summary>
    private static int GoldGained = 0;

    public float speed = 1f;
    public float baseTimeToComplete = 2;
    public int goldOnFill = 1;

    public Text goldText;

    private float _progress = 0f;

    public Image fillImage;

    public float Progress
    {
        get => _progress;

        set
        {
            _progress = Mathf.Clamp01(value);

            if (_progress == 1f)
            {
                ///Add gold
                GoldGained += goldOnFill;
                goldText.text = "Gold gained: " + GoldGained;
                fillImage.fillAmount = _progress;
            }

        }
    }


    private void Update()
    {
        Progress += Time.deltaTime * baseTimeToComplete * speed;
    }
}
