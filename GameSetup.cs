using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;

    public Transform[] BotSpawnPoints;
    public Transform[] spawnPoints;
    private void OnEnable()
    {
        if (GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 1; i <= (multiplayersettings.multiplayerSettings.maxPlayers - PhotonNetwork.PlayerList.Length); i++)
            {
                int random = Random.Range(0, BotSpawnPoints.Length);
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAI"),
                    GameSetup.GS.BotSpawnPoints[random].position, GameSetup.GS.BotSpawnPoints[random].rotation, 0);
            }
        }
    }
    public void DisconnectPlayer()
    {
        Destroy(photonroom.room.gameObject);
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        PhotonNetwork.LoadLevel(multiplayersettings.multiplayerSettings.menuScene);
    }
}
