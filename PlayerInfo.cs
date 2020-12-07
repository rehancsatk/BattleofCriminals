using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    public static PlayerInfo pi;
    public int mySelectedCharacter;
    public GameObject[] allcharacters;

    
    public void OnEnable()
    {
        if (PlayerInfo.pi == null)
        {
            PlayerInfo.pi = this;
        }
        else
        {
            if (PlayerInfo.pi != this)
            {
                Destroy(PlayerInfo.pi.gameObject);
                PlayerInfo.pi = this;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("myCharacter"))
        {
            mySelectedCharacter = PlayerPrefs.GetInt("myCharacter");
        }
        else
        {
            mySelectedCharacter = 0;
            PlayerPrefs.SetInt("myCharacter", mySelectedCharacter);
        }
    }

}
