using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public static PlayerAnimationManager Instance;
    
    public struct TriggerAnimations
    {
        /// <summary> Activates the light attack animation trigger. </summary>
        public static string LIGHT_ATTACK = "castLightAttack";

        /// <summary> Activates the heavy attack animation trigger. </summary>
        public static string HEAVY_ATTACK = "castHeavyAttack";

        /// <summary> Activates the jump animation trigger. </summary>
        public static string JUMP = "castJump";

        /// <summary> Activates the landing animation trigger. </summary>
        public static string LANDING = "castLanded";
    }

    public struct BoolAnimations
    {
        public static string IDLE = "isIdle";

        public static string RUNNING = "isRunning";
    

        /// <summary> Reference to the falling animation boolean. </summary>
        /// <remarks>Remember all transitions into the falling animation can't have exit time.</remarks>
        public static string FALLING = "isFalling";

    }

    public struct SpellAnimations
    {
        public static string ICICLE = "castIcicle";
        public static string FIREWALL = "castFirewall";
    }

    private Animator animator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;

        if (animator == null)
            animator = GetComponent<Animator>();
    }


    /// <summary>
    /// Pass in one of the static strings from the PlayerAnimationManager.
    /// </summary>
    /// <param name="animation">The name of the animation to play</param>
    /// <remarks>The name of the animation shou ld be related to a 
    /// bool or trigger in animator</remarks>
    public void ActivateTrigger(string animation)
    {
        animator.SetTrigger(animation);
    }

    /// <summary> Turns the animation on based on the value of <paramref name="on"/> </summary>
    /// <param name="on">True indicates we are running, false indicates we are idle.</param>
    public void RunningAnimation(bool on)
    {
        animator.SetBool("isRunning", on);
        ///This is just a nice little way to ensure that if running is on idle
        ///will be off and we don't need to write an if block
        animator.SetBool("isIdle", on == true ? false : true);
    }

    public void SetBool(string animation, bool value)
    {
        if (animator.GetBool(animation) != value)
        {
            animator.SetBool(animation, value);
        }
    }

    public void TurnSwordOff()
    {
        Sword.ActiveSword.gameObject.SetActive(false);
    }
}
