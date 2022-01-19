/*
 * Author: Hunter Lawrence-Emanuel
 * Date: 9/1/2021
 * 
 * Brief:this script keeps the camera 
 * correctly positioned relative to the player
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{
    ///<summary>This targets the camera to the player.</summary>
    public Transform target;
    
    public float smoothSpeed;

    public Vector3 offset;

    public string sceneName;


    private void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "MineScene")
        {
            //target = GameObject.FindGameObjectWithTag("PlayerModel").transform;
            target = Player.Instance.transform;
        }
    }


    private void LateUpdate()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if(sceneName != "MineScene")
        {

        
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position =  smoothedPos;
        }
    }


}
