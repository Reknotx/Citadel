/*
 * Author: Chase O'Connor
 * Date: 9/2/2021
 * 
 * Brief: This script contains lists of all of the spawnable rooms made by
 * the level designers.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{


    [CreateAssetMenu]
    public class RoomContainer : ScriptableObject
    {
        [Tooltip("A list of all")]
        public List<GameObject> BossRooms = new List<GameObject>();
        public List<GameObject> SpawnRooms = new List<GameObject>();
        public List<GameObject> ShopRooms = new List<GameObject>();
        public List<GameObject> RegularRooms = new List<GameObject>();
        public GameObject filledRoom;
    }

}