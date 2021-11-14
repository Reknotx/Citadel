using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public static PlayerAnimationManager Instance;
    
    public static string IDLE = "isIdle";

    /// <summary> Activates the light attack animation trigger. </summary>
    public static string LIGHT_ATTACK = "castLightAttack";

    /// <summary> Activates the heavy attack animation trigger. </summary>
    public static string HEAVY_ATTACK = "castHeavyAttack";
    public static string RUNNING = "isMoving";
    
    /// <summary> Activates the jump animation trigger. </summary>
    public static string JUMP = "castJump";

    /// <summary> Activates the falling animation trigger. </summary>
    /// <remarks>Remember all transitions into the falling animation can't have exit time.</remarks>
    public static string FALLING = "castFalling";

    /// <summary> Activates the landing animation trigger. </summary>
    public static string LANDING = "castLanding";

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
    public void ActivateTrigger(string animation)
    {
        animator.SetTrigger(animation);

    }

    /// <summary> Turns the animation on based on the value of <paramref name="on"/> </summary>
    /// <param name="on">True indicates we are running, false indicates we are idle.</param>
    public void RunAnimation(bool on)
    {
        animator.SetBool("isRunning", on);
        ///This is just a nice little way to ensure that if running is on idle
        ///will be off and we don't need to write an if block
        animator.SetBool("isIdle", on == true ? false : true);
    }

    public void TurnSwordOff()
    {
        Sword.ActiveSword.gameObject.SetActive(false);
    }
}
