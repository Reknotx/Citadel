/*
 * Author: Chase O'Connor
 * Date: 9/2/2021
 * 
 * Brief: This script contains info about the particular room and
 * will perform certain actions based on events.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    #endregion

    #region Private


    #endregion
    #endregion


    #region Properties


    #endregion


    private void Start()
    {
        ///set enemies and inanimateObj to empty as all references will be
        ///set on start.
        
    }

    /// <summary> The function that will trigger when player enters the room. </summary>
    /// <remarks>Basically this function is expected to activate when
    /// the camera transition is finished. This function will activate enemies
    /// and spawn in everything in the room.</remarks>
    public void OnEnter()
    {

    }

    /// <summary> The function that will trigger when player leaves the room. </summary>
    /// <remarks>This function is expected to activate when the player leaves this
    /// room and moves on to another one. This function will deactivate all enemies
    /// and other objects that were in the game to avoid having everything
    /// spawned at once.</remarks>
    public void OnExit()
    {
        
    }
}
