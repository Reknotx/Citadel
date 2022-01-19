/*
 * Author: Chase O'Connor
 * Date: 9/2/2021
 * 
 * Brief: This script contains info about the particular room and
 * will perform certain actions based on events.
 */

using System.Collections.Generic;
using UnityEngine;
using Minimap;

namespace Map
{
    public class Room : MonoBehaviour
    {
        #region Fields
        #region Public
        public Vector2 gridPos = Vector2.zero;

        [Tooltip("The list of all enemies populating a room.")]
        public List<GameObject> enemies = new List<GameObject>();

        [Tooltip("The list of all inantimate objects populating a room.")]
        public List<GameObject> inanimateObjs = new List<GameObject>();

        public RoomInfo roomInfo;
        public GameObject fog;
        public GameObject enemiesParent;

        [HideInInspector]
        public bool fogEnabledOnStart = true;

        #endregion

        #region Private
        bool _firstVisit = true;

        #endregion
        #endregion
        
        #region Properties
        bool FirstVisit
        {
            get => _firstVisit;

            set
            {
                _firstVisit = value;
                //turn off the fog
                fog.SetActive(false);
            }
        }

        #endregion


        private void Start()
        {
            //set enemies and inanimateObj to empty as all references will be
            //set on start.
            if (!fogEnabledOnStart) return;
            if (fog != null)
            {
                fog.SetActive(true);
                fog.GetComponent<MeshRenderer>().enabled = true;
            }

            if (enemiesParent == null) return;
            foreach (Transform enemy in enemiesParent.transform)
            {
                enemies.Add(enemy.gameObject);
                enemy.gameObject.SetActive(false);
            }
        }

        /// <summary> The function that will trigger when player enters the room. </summary>
        /// <remarks>Basically this function is expected to activate when
        /// the camera transition is finished. This function will activate enemies
        /// and spawn in everything in the room.</remarks>
        public void OnEnter()
        {
            FirstVisit = false;

            if (GraphMover.Instance != null)
                GraphMover.Instance.MoveGraph(transform.position + new Vector3(15, 15, 0));

            if (MiniMapManager.Instance != null)
                MiniMapManager.Instance.MoveMinimapCamera(transform.position + new Vector3(15, 15, 0));

            //Turn on all the enemies.
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                    enemy.SetActive(true);
            }
            //Adjust the grid position for the astar path   
            // Debug.Log("Entered " + name);
        }

        /// <summary> The function that will trigger when player leaves the room. </summary>
        /// <remarks>This function is expected to activate when the player leaves this
        /// room and moves on to another one. This function will deactivate all enemies
        /// and other objects that were in the game to avoid having everything
        /// spawned at once.</remarks>
        public void OnExit()
        {
            foreach (GameObject enemy in enemies)
            {
                if(enemy != null)
                    enemy.SetActive(false);
            }
        }

        public void TurnOffFog()
        {
            fog.SetActive(false);
        }



    }
}