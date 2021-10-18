using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : Enemy
{
    public Transform swipeSpawnPoint;

    ///Tentacles have their own individual health bars
    ///and are attached to squiggmar.
    ///
    ///Once a tentacle is killed it needs tobe removed from combat
    ///and put into a neutral state so that it can be reactivated
    ///later on and doesn't need to be spawned in. 
    ///

    public override float Health { get => base.Health; set => base.Health = value; }


    public void Swipe()
    {

    }
}
