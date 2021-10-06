using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.RoomScripts
{
    public class RoomTrigger : MonoBehaviour
    {
        public Room parentRoomScript;

        private void Awake()
        {
            parentRoomScript = transform.parent.GetComponent<Room>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
                parentRoomScript.OnEnter();
            if (other.gameObject.layer == 8 && !parentRoomScript.enemies.Contains(other.gameObject))
            {
                parentRoomScript.enemies.Add(other.gameObject);

            }

        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 7)
                parentRoomScript.OnExit();
            else if (other.gameObject.layer == 8 && parentRoomScript.enemies.Contains(other.gameObject))
                parentRoomScript.enemies.Remove(other.gameObject);

        }
    }
}