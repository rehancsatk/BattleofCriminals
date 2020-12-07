using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AvatarGet : MonoBehaviour
{
    // Start is called before the first frame update
    private PhotonView PV;
    public int characterValue;
    public GameObject mycharacter;


    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.pi.mySelectedCharacter);
        }

    }
    [PunRPC]
    void RPC_AddCharacter(int WhichCharacter)
    {
        characterValue = WhichCharacter;
        //  mycharacter = Instantiate(Playerinfo.pi.allcharacters[WhichCharacter], transform.position, transform.rotation, transform);
        mycharacter = Instantiate(PlayerInfo.pi.allcharacters[WhichCharacter], transform.position, transform.rotation);
    }
}
