//author: Beren Franklin
//12/10/2021
//description: used to reset the permanent gold by deleting the key in PlayerPrefs
// the key comes back when you start a new game, and it should theoretically not be stuck at 0 anymore

using UnityEngine;

public class ResetPlayerPrefGold : MonoBehaviour
{
    public void ResetPermanentGold()
    {
        PlayerPrefs.DeleteKey("permanentGold");
    }

    //for testing (obviously)
    public void TestPermanentGold()
    {

        if (PlayerPrefs.HasKey("permanentGold"))
        {
            Debug.Log("The key " + "permanentGold" + " exists");
        }
        else
            Debug.Log("The key " + "permanentGold" + " does not exist");
    }
}
