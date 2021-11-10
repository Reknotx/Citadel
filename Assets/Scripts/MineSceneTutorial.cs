using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MineSceneTutorial : MonoBehaviour
{

    public SceneManagerScript SM;

    #region Mine Scene Instructions;

    public Text mineIinstruction_1;
    public Text mineIinstruction_2;
    public Text mineIinstruction_3;

    public Text[] instroTracker;
    public int index;

    public Button TipButton;
    public Button TutButton;

    private string currentScene;

    private bool closeTut = false;



    


    #endregion

    

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        //PlayerPrefs.SetInt("TutorialHasPlayed", 0); //<------DO NOT UNCOMMENT, here for testing purposes only


        index = 0;
        if (PlayerPrefs.GetInt("TutorialHasPlayed", 0) <= 0)
        {
            if (currentScene == "MineScene")
            {
                instroTracker[index].gameObject.SetActive(true);
                TipButton.gameObject.SetActive(true);
                TutButton.gameObject.SetActive(true);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScene == "MineScene")
        {
            if (index + 1 >= instroTracker.Length)
            {
                TipButton.gameObject.SetActive(false);
            }

            if (closeTut)
            {
                PlayerPrefs.SetInt("TutorialHasPlayed", 1);
                for (int i = 0; i < instroTracker.Length; i++)
                {
                    instroTracker[index].gameObject.SetActive(false);
                    TutButton.gameObject.SetActive(false);
                    TipButton.gameObject.SetActive(false);

                }

            }
        }     
    }

    public void Next()
    {
        if(index >= instroTracker.Length)
        {
            instroTracker[instroTracker.Length].gameObject.SetActive(false);
            
        }
        else if (index < instroTracker.Length)
        {
            instroTracker[index].gameObject.SetActive(false);
            instroTracker[index+1].gameObject.SetActive(true);
            index += 1;
        }

        
    }

    public void CloseTutorial()
    {
        closeTut = true;
    }
}
