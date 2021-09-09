/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/1/2021
 * 
 * Brief:this script keeps the camera 
 * correctly positioned relative to the player
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    ///<summary>This targets the camera to the player.</summary>
    public GameObject target;

    ///<summary>This is the camera's distance from the player.</summary>
    [Range(0, 20)]
    public int CameraDist;

   
    // Update is called once per frame
    void FixedUpdate()
    {
        ///<summary>This resets the camera position.</summary>
        Vector3 pos = Vector3.zero;

        ///<summary>This sets the camera's position based on the inputted range.</summary>
        pos.x = target.transform.position.x;
        pos.z = -CameraDist;
        pos.y = target.transform.position.y;

        ///<summary>This actually changed the camera's position.</summary>
        GetComponent<Transform>().position = pos;
    }
}
