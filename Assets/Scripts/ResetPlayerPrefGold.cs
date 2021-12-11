//author: Beren Franklin
//12/10/2021
//description: used to reset the permanent gold by deleting the key in PlayerPrefs
// the key comes back when you start a new game, and it should theoretically not be stuck at 0 anymore

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPrefGold : MonoBehaviour
{
    public void ResetPermanentGold()
    {
        PlayerPrefs.DeleteKey("permanentGold_h872003871");
    }

    //for testing (obviously)
    public void TestPermanentGold()
    {

        if (PlayerPrefs.HasKey("permanentGold_h872003871"))
        {
            Debug.Log("The key " + "permanentGold_h872003871" + " exists");
        }
        else
            Debug.Log("The key " + "permanentGold_h872003871" + " does not exist");
    }
}
