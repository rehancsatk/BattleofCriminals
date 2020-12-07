using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiplayersettings : MonoBehaviour
{

    public static multiplayersettings multiplayerSettings;
    //methods to load players in game i.e continous and  delayed (first fill then start the game method)

    public bool delayStart;
    public int maxPlayers;
    public int menuScene;
    public int multiplayerScene;

    private void Awake()
    {
        if (multiplayersettings.multiplayerSettings == null)
        {
            multiplayersettings.multiplayerSettings = this;
        }
        else
        {
            if (multiplayersettings.multiplayerSettings != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
