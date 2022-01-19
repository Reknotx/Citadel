using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

   
}
