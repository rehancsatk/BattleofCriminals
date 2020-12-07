using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mm : MonoBehaviour
{
    public void onClickCharacterPick(int WhichCharacter)
    {
        if (PlayerInfo.pi != null)
        {
            PlayerInfo.pi.mySelectedCharacter = WhichCharacter;
            PlayerPrefs.SetInt("myCharacter", WhichCharacter);
        }
    }
}
