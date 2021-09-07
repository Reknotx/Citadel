using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoomContainer : ScriptableObject
{
    [Tooltip("A list of all")]
    public List<GameObject> BossRooms = new List<GameObject>();
    public List<GameObject> SpawnRooms = new List<GameObject>();
    public List<GameObject> ShopRooms = new List<GameObject>();
    public List<GameObject> RegularRooms = new List<GameObject>();
}
