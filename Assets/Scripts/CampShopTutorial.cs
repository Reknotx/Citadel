using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CampShopTutorial : MonoBehaviour
{
    public SceneManagerScript SM;

    #region Camp Scene Instructions;

    public Text campIinstruction_1;
    public Text campIinstruction_2;
    public Text campIinstruction_3;

    public Text[] instroTracker_camp;
    public int index_camp;

    public Button TipButton_camp;
    public Button TutButton_camp;

    private string currentScene_camp;

    private bool closeTut_camp = false;

    public bool openCampShop;





    #endregion



    // Start is called before the first frame update
    public void Start()
    {
        currentScene_camp = SceneManager.GetActiveScene().name;
        //PlayerPrefs.SetInt("TutorialHasPlayed_camp", 0); //<------DO NOT UNCOMMENT, here for testing purposes only

        
        
        index_camp = 0;
       
    }

    // Update is called once per frame
    public void Update()
    {
        if (PlayerPrefs.GetInt("TutorialHasPlayed_camp", 0) <= 0)
        {
            if (currentScene_camp == "CampScene")
            {
                if (openCampShop)
                {
                    instroTracker_camp[index_camp].gameObject.SetActive(true);
                    TipButton_camp.gameObject.SetActive(true);
                    TutButton_camp.gameObject.SetActive(true);
                    openCampShop = false;
                }

            }
        }

        if (index_camp + 1 >= instroTracker_camp.Length)
        {
            TipButton_camp.gameObject.SetActive(false);
        }

        if (closeTut_camp)
        {
            PlayerPrefs.SetInt("TutorialHasPlayed_camp", 1);
            for (int i = 0; i < instroTracker_camp.Length; i++)
            {
                instroTracker_camp[index_camp].gameObject.SetActive(false);
                TutButton_camp.gameObject.SetActive(false);
                TipButton_camp.gameObject.SetActive(false);

            }

        }
    }

    public void Next()
    {
        if (index_camp >= instroTracker_camp.Length)
        {
            instroTracker_camp[instroTracker_camp.Length].gameObject.SetActive(false);

        }
        else if (index_camp < instroTracker_camp.Length)
        {
            instroTracker_camp[index_camp].gameObject.SetActive(false);
            instroTracker_camp[index_camp + 1].gameObject.SetActive(true);
            index_camp += 1;
        }


    }

    public void CloseTutorial()
    {
        closeTut_camp = true;
    }
}

