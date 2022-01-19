using UnityEngine;
using UnityEngine.UI;

public class PlayerSpellUIHandler : MonoBehaviour
{
    public GameObject player;

    public Image LightAttackIMG_1;
    public Image LightAttackIMG_2;
    public Image LightAttackIMG_3;

    public Image HeavyAttackIMG_1;
    public Image HeavyAttackIMG_2;
    public Image HeavyAttackIMG_3;

    public Image FireWallIMG_1;
    public Image FireWallIMG_2;
    public Image FireWallIMG_3;

    public Image IcicleIMG_1;
    public Image IcicleIMG_2;
    public Image IcicleIMG_3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<Player>().Attack1 == "Light Attack")
        {
            LightAttackIMG_1.enabled = true;
            HeavyAttackIMG_1.enabled = false;
            FireWallIMG_1.enabled = false;
            IcicleIMG_1.enabled = false;
        }
        else if(player.GetComponent<Player>().Attack1 == "Heavy Attack")
        {
            LightAttackIMG_1.enabled = false;
            HeavyAttackIMG_1.enabled = true;
            FireWallIMG_1.enabled = false;
            IcicleIMG_1.enabled = false;
        }
        else if(player.GetComponent<Player>().Attack1 == "Fire Wall")
        {
            LightAttackIMG_1.enabled = false;
            HeavyAttackIMG_1.enabled = false;
            FireWallIMG_1.enabled = true;
            IcicleIMG_1.enabled = false;
        }
        else if (player.GetComponent<Player>().Attack1 == "Icicle")
        {
            LightAttackIMG_1.enabled = false;
            HeavyAttackIMG_1.enabled = false;
            FireWallIMG_1.enabled = false;
            IcicleIMG_1.enabled = true;
        }
        else
        {
            LightAttackIMG_1.enabled = true;
            HeavyAttackIMG_1.enabled = false;
            FireWallIMG_1.enabled = false;
            IcicleIMG_1.enabled = false;
        }

        if(player.GetComponent<Player>().Attack2 == "Light Attack")
        {
            LightAttackIMG_2.enabled = true;
            HeavyAttackIMG_2.enabled = false;
            FireWallIMG_2.enabled = false;
            IcicleIMG_2.enabled = false;
        }
        else if (player.GetComponent<Player>().Attack2 == "Heavy Attack")
        {
            LightAttackIMG_2.enabled = false;
            HeavyAttackIMG_2.enabled = true;
            FireWallIMG_2.enabled = false;
            IcicleIMG_2.enabled = false;
        }
        else if (player.GetComponent<Player>().Attack2 == "Fire Wall")
        {
            LightAttackIMG_2.enabled = false;
            HeavyAttackIMG_2.enabled = false;
            FireWallIMG_2.enabled = true;
            IcicleIMG_2.enabled = false;
        }
        else if (player.GetComponent<Player>().Attack2 == "Icicle")
        {
            LightAttackIMG_2.enabled = false;
            HeavyAttackIMG_2.enabled = false;
            FireWallIMG_2.enabled = false;
            IcicleIMG_2.enabled = true;
        }
        else
        {
            LightAttackIMG_2.enabled = true;
            HeavyAttackIMG_2.enabled = false;
            FireWallIMG_2.enabled = false;
            IcicleIMG_2.enabled = false;
        }

        if(player.GetComponent<Player>().Attack3 == "Light Attack")
        {
            LightAttackIMG_3.enabled = true;
            HeavyAttackIMG_3.enabled = false;
            FireWallIMG_3.enabled = false;
            IcicleIMG_3.enabled = false;
        }
        else if (player.GetComponent<Player>().Attack3 == "Heavy Attack")
        {
            LightAttackIMG_3.enabled = false;
            HeavyAttackIMG_3.enabled = true;
            FireWallIMG_3.enabled = false;
            IcicleIMG_3.enabled = false;
        }
        else if (player.GetComponent<Player>().Attack3 == "Fire Wall")
        {
            LightAttackIMG_3.enabled = false;
            HeavyAttackIMG_3.enabled = false;
            FireWallIMG_3.enabled = true;
            IcicleIMG_3.enabled = false;
        }
        else if (player.GetComponent<Player>().Attack3 == "Icicle")
        {
            LightAttackIMG_3.enabled = false;
            HeavyAttackIMG_3.enabled = false;
            FireWallIMG_3.enabled = false;
            IcicleIMG_3.enabled = true;
        }
        else
        {
            LightAttackIMG_3.enabled = true;
            HeavyAttackIMG_3.enabled = false;
            FireWallIMG_3.enabled = false;
            IcicleIMG_3.enabled = false;
        }
    }
}
