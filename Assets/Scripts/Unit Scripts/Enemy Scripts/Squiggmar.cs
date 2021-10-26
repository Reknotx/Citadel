using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squiggmar : Enemy
{
    ///Notes for Squiggmar
    ///1. The head needs to be invulnerable until all tentacles are killed.
    ///
    ///2. Need a way to reference the tentacles that he has
    ///to tell when they are dead and damage can be applied
    ///to the head.
    ///
    ///3. Squiggmar has two states, an invulnerable state when the 
    ///tentacles are still alive, and a vulnerable state.
    ///     3a. In the invulnerable state he attacks with his tentacles
    ///     as well as with a head slam
    ///     
    ///     3b. In the vulnerable state he falls to the ground and 
    ///     can be damaged by the player. He remains in this state
    ///     for a few seconds, or until his health hits 0.
    ///
    ///4. Squiggmar's head should be kept away from the terrain
    ///and be allowed to pass through everything. There shouldn't
    ///be ay form of clipping happening that makes him behave
    ///weirdly and eratic.
    ///
    ///
}
