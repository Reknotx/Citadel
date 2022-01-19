using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Tester : ScriptableObject
{
    public enum TesterEnum
    {
        Hello,
        Hi,
        Goodbye
    }

    public TesterEnum tester;

}
