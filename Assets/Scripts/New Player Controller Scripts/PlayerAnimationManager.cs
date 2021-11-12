using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public static PlayerAnimationManager Instance;
    
    public static string IDLE = "playerIdle";
    public static string LIGHT_ATTACK = "playerLightAttack";
    public static string HEAVY_ATTACK = "playerHeavyAttack";
    public static string RUNNING = "playerMove";


    
    public Animator animator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;
    }


    /// <summary>
    /// Pass in one of the static strings from the PlayerAnimationManager.
    /// </summary>
    /// <param name="animation">The name of the animation to play</param>
    /// <remarks>The name of the animation should be related to a 
    /// bool or trigger in animator</remarks>
    public void PlayAnimation(string animation)
    {
        

    }

    ///Actually the sword could be turned on by itself on it's own script
    ///and then have the message sent to the animation manager to
    ///play the animation on the same frame.
    ///THIS is how I will handle it, having the appropriate scripts 
    ///sending the messages themselves rather than needing the player to do it.
    ///using OnEnable
    /// 
    ///This will also make the addition of even more animations in the future a 
    ///lot easier to pull off. That is of course if we add more animations.
    public void TurnSwordOn()
    {

    }

    public void TurnSwordOff()
    {

    }

}
