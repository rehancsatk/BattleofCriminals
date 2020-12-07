using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class photonplayer : MonoBehaviour
{

    private PhotonView PV;
    public GameObject myAvatar;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            return;
        }
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        if (PV.IsMine)
        {
            //base.OnJoinedRoom();
            //SceneManager.LoadScene("Main Menu"); 
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
              GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
           // ((MonoBehaviour)myAvatar.GetComponent("MyPlayer")).enabled = true;
           // ((MonoBehaviour)myAvatar.GetComponent("WeaponHandler")).enabled = true;
           // ((MonoBehaviour)myAvatar.GetComponent("UserInput")).enabled = true;
           //// ((MonoBehaviour)myAvatar.GetComponentsInChildren("CameraRig")).enabled = true;
           //// myAvatar.transform.FindChild("Main Camera").gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}

