using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public void OnPickup()
    {
        MapGenerator.Instance.ExposeSpecialRooms();
    }
}
