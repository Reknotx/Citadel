using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public override int Health 
    { 
        get => base.Health; 
        set => base.Health = value; 
    }


    public override void Update()
    {
        base.Update();
    }
}
