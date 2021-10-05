using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [Tooltip("Toggle this to activate the item's special effect.")]
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
