using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.IO;

public class photonroom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //room info
    public static photonroom room;
    private PhotonView PV; //builtin variable to send messages from one player to the other within the room using RPC, synch the gameobject accross the netwrk

    public bool IsGameLoaded;
    public int currentScene;

    // players info
    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playersInGame;
    public Text RoomPlayerData;
    public Text Timer;
    public Text ConnectionText;
    //delayed start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxplayer;
    private float atMaxPlayer;
    private float timeToStart;

    // setting up single tin
    private void Awake()
    {
        if (photonroom.room == null)
        {
            photonroom.room = this;
        }
        else
        {
            if (photonroom.room != this)
            {
                Destroy(photonroom.room.gameObject);
                photonroom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);


    }


    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;  //when we will load a scene

    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxplayer = startingTime;
        atMaxPlayer = 10;
        timeToStart = startingTime; //initialized some private variables
    }

    // Update is called once per frame
    void Update()  // in update we are dealing with when to start the game and timer
    {
        if (multiplayersettings.multiplayerSettings.delayStart)
        {
            if (playersInRoom == 0)
            {
                RestartTimer();
            }
            if (!IsGameLoaded)
            {
                if (readyToStart)
                {
                    atMaxPlayer -= Time.deltaTime;
                    lessThanMaxplayer = atMaxPlayer;
                    timeToStart = atMaxPlayer;
                }
                else if (readyToCount)
                {
                    lessThanMaxplayer -= Time.deltaTime;
                    timeToStart = lessThanMaxplayer;
                }
                Timer.text = "Starting in " + timeToStart.ToString("f0");
                if (timeToStart <= 0)
                {
                    StartGame();
                }
                if (!readyToCount)
                {
                    timeToStart = startingTime;
                }

            }
        }
    }

    public override void OnJoinedRoom() //will be called whenever we joined room and if we have dalyed start we will check how many players there are 
    {                                   // if players are more than, we start the count and if we dont have delayed start we will just start the game

        base.OnJoinedRoom();
        Debug.Log("We are now in a room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        if (multiplayersettings.multiplayerSettings.delayStart)
        {
            RoomPlayerData.text = "Connected Players (" + playersInGame + "/" + multiplayersettings.multiplayerSettings.maxPlayers + ")"; // displays number of players in room out of total number that a room can hold
            if (playersInRoom >= 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == multiplayersettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false; // when players are full no more player can join the room untill we open it 
            }


        }
        else
        {
            StartGame();
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer) //when other players joinour room
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        if (multiplayersettings.multiplayerSettings.delayStart)
        {
            Debug.Log("Displays players out of max players in room(" + playersInRoom + ";" + multiplayersettings.multiplayerSettings.maxPlayers + ")");
            if (playersInRoom >= 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == multiplayersettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    void StartGame()
    {
        IsGameLoaded = true;

        if (!PhotonNetwork.IsMasterClient)
            return;
        if (multiplayersettings.multiplayerSettings.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(multiplayersettings.multiplayerSettings.multiplayerScene);
    }

    void RestartTimer()
    {
        lessThanMaxplayer = startingTime;
        timeToStart = startingTime;
        atMaxPlayer = 10;
        readyToCount = false;
        readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == multiplayersettings.multiplayerSettings.multiplayerScene)
        {
            IsGameLoaded = true;
            if (multiplayersettings.multiplayerSettings.delayStart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
            {
                RPC_CreatePlayer();
            }
        }
    }
    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
       
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
        // PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);

    }
    /*
        [PunRPC]
        private void RPC_CreateWeapon()
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "photonNetwrokweapon"), transform.position, Quaternion.identity, 0);
            // PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);

        }*/

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + "Has left the game");
        playersInGame--;
    }
}
