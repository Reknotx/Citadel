//author: Beren Franklin
//description: used to 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPrefGold : MonoBehaviour
{
    public void ResetPermanentGold()
    {
        PlayerPrefs.DeleteKey("permanentGold_h872003871");
    }

    public void TestPermanentGold()
    {

        if (PlayerPrefs.HasKey("permanentGold_h872003871"))
        {
            Debug.Log("The key " + "permanentGold_h872003871" + " exists");
        }
        else
            Debug.Log("The key " + "permanentGold_h872003871" + " does not exist");

        //PlayerPrefs.DeleteKey("permanentGold_h872003871");
    }
}
