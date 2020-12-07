using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class photonlobby : MonoBehaviourPunCallbacks
{
    public static photonlobby lobby;  //its a single tin variable

    public GameObject battlebutton;  //one click and player will search for the available rooms;
    public GameObject cancelbutton; // to stop searching for the room

    private void Awake()
    {
        lobby = this; //this referes to the instabce of class in which this line of code exists (this instance of photonlobby class)
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();  //setting up the conection between player and photon server in the start function
        photonroom.room.ConnectionText.text = "Connecting with Available Server....";
    }




    public override void OnConnectedToMaster()    //this function will give the feedback whether the connection is set b/w player-server or not
    {
        photonroom.room.ConnectionText.text = "Connected";
        PhotonNetwork.AutomaticallySyncScene = true; // when master client loads the scene, players connected to it will also load that scene 
        battlebutton.SetActive(true);
        photonroom.room.Timer.gameObject.SetActive(true);
    }
    public void OnBattleButtonClicked()
    {
       // Debug.Log("Battle button was clicked");
        battlebutton.SetActive(false);
        cancelbutton.SetActive(true);

        PhotonNetwork.JoinRandomRoom(); //function by photon that help our player to join the random room
       // SceneManager.LoadScene("terrain");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) //if player failed to join room
    {
        photonroom.room.ConnectionText.text = "No Room Available\nCreating New Room";
        CreateRoom();

    }


    void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)multiplayersettings.multiplayerSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps); //Trying to create a room with the specified values.
    }

    public override void OnJoinedRoom()
    {

        base.OnJoinedRoom();
        Debug.Log("We are now in a room");
    }


    public override void OnCreateRoomFailed(short returncode, string message) // if we cant create new room
    {
        photonroom.room.ConnectionText.text = "Room Name Duplicate found";
        CreateRoom();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel button was clicked");
        //battlebutton.SetActive(true);
        //cancelbutton.SetActive(false);
        //PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("SampleScene");
        PlayerPrefs.DeleteAll();
    }

    
}


