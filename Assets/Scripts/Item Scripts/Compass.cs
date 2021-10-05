using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public bool triggerEffect = false;

    void Update()
    {
        if (triggerEffect)
        {
            OnPickup();
            Destroy(gameObject);
        }
    }


    public void OnPickup()
    {
        MapGenerator.Instance.ExposeSpecialRooms();
    }
}
