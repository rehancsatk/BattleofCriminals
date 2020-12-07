using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Ontrigger : MonoBehaviour
{
    private PhotonView pv;
    Stats value;
    public Transform player1;
    public Transform player2;
    public Transform player3;
    public Transform player4;
    public Transform player5;
    public Transform player6;
    public Transform player7;
    public Transform player8;
    public Transform player9;
    public GameObject Bomb;
    public GameObject UI;
    public GameObject ExplosionEffect;
    public float delay = 3f;
    float countdown;
    bool hasExploaded = false; // this variable is to check ky pehle bomb blast hu chuka hh ya nh
    private Player_Audio player_audio;
    public float Distance = 1;
    private Stats characterStats { get { return GetComponent<Stats>(); } set { characterStats = value; } }


    public bool isPlayer1;
    public bool isPlayer2;
    public bool isPlayer3;
    public bool isPlayer4;
    public bool isPlayer5;
    public bool isPlayer6;
    public bool isPlayer7;
    public bool isPlayer8;
    public bool isPlayer9;
    public bool isPlayer10;
    public class SightSettings
    {
        public LayerMask sightLayers;                    // yeh check kre ga kn kn si layers walo ko attack krna hh
        public float sightRange = 30f;

    }
    public SightSettings sight;

    private Stats[] allCharacters;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        UI.SetActive(false);
        value = GetComponent<Stats>();
        //countdown = delay;
        player_audio = GetComponentInChildren<Player_Audio>();
        //GetAllCharacters();
      //  player1 = GameObject.FindGameObjectWithTag("Enemy").transform;
    }
 
    void Update()
    {
       

            if (Vector3.Distance(transform.position, player1.position) <= Distance)
            {
                UI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Invoke("Explode1", 5.0f); // call this function after 3seconds
                    Destroy(Bomb, 5.0f);
                    UI.SetActive(false);
                    hasExploaded = true;

                }

            }
            else
            {
                UI.SetActive(false);
            }

        
    }

//    //void GetAllCharacters()
//    //{
//    //    allCharacters = GameObject.FindObjectsOfType<Stats>();
//    //}
//      void Update()
//    {
//        if (pv.IsMine)
//        {
//            if (hasExploaded)
//                return;
//            if (isPlayer1)
//            {
//                if (Vector3.Distance(transform.position, player1.position) <= Distance || Vector3.Distance(transform.position, player2.position) <= Distance
//                   || Vector3.Distance(transform.position, player3.position) <= Distance || Vector3.Distance(transform.position, player4.position) <= Distance
//                    || Vector3.Distance(transform.position, player5.position) <= Distance || Vector3.Distance(transform.position, player6.position) <= Distance
//                    || Vector3.Distance(transform.position, player6.position) <= Distance || Vector3.Distance(transform.position, player8.position) <= Distance
//                    || Vector3.Distance(transform.position, player9.position) <= Distance)
//                {
//                    UI.SetActive(true);
//                    if (Input.GetKeyDown(KeyCode.Alpha2))
//                    {
//                        Invoke("Explode1", 5.0f); // call this function after 3seconds
//                        Destroy(Bomb, 5.0f);
//                        UI.SetActive(false);
//                        hasExploaded = true;

//                    }

//                }
//                else
//                {
//                    UI.SetActive(false);
//                }
//            }
//            if (isPlayer2)
//            {
//                if (Vector3.Distance(transform.position, player1.position) <= Distance || Vector3.Distance(transform.position, player2.position) <= Distance
//                    || Vector3.Distance(transform.position, player3.position) <= Distance || Vector3.Distance(transform.position, player4.position) <= Distance
//                     || Vector3.Distance(transform.position, player5.position) <= Distance || Vector3.Distance(transform.position, player6.position) <= Distance
//                     || Vector3.Distance(transform.position, player6.position) <= Distance || Vector3.Distance(transform.position, player8.position) <= Distance
//                     || Vector3.Distance(transform.position, player9.position) <= Distance)
//                {
//                    UI.SetActive(true);
//                    if (Input.GetKeyDown(KeyCode.Alpha2))
//                    {
//                        Invoke("Explode1", 5.0f); // call this function after 3seconds
//                        Destroy(Bomb, 5.0f);
//                        UI.SetActive(false);
//                        hasExploaded = true;

//                    }

//                }
//                else
//                {
//                    UI.SetActive(false);
//                }
//            }
//            if (isPlayer3)
//            {
//                if (Vector3.Distance(transform.position, player1.position) <= Distance || Vector3.Distance(transform.position, player2.position) <= Distance
//                   || Vector3.Distance(transform.position, player3.position) <= Distance || Vector3.Distance(transform.position, player4.position) <= Distance
//                    || Vector3.Distance(transform.position, player5.position) <= Distance || Vector3.Distance(transform.position, player6.position) <= Distance
//                    || Vector3.Distance(transform.position, player6.position) <= Distance || Vector3.Distance(transform.position, player8.position) <= Distance
//                    || Vector3.Distance(transform.position, player9.position) <= Distance)
//                {
//                    UI.SetActive(true);
//                    if (Input.GetKeyDown(KeyCode.Alpha2))
//                    {
//                        Invoke("Explode1", 5.0f); // call this function after 3seconds
//                        Destroy(Bomb, 5.0f);
//                        UI.SetActive(false);
//                        hasExploaded = true;

//                    }

//                }
//                else
//                {
//                    UI.SetActive(false);
//                }
//            }
//            if (isPlayer4)
//            {
//                if (Vector3.Distance(transform.position, player1.position) <= Distance || Vector3.Distance(transform.position, player2.position) <= Distance
//                   || Vector3.Distance(transform.position, player3.position) <= Distance || Vector3.Distance(transform.position, player4.position) <= Distance
//                    || Vector3.Distance(transform.position, player5.position) <= Distance || Vector3.Distance(transform.position, player6.position) <= Distance
//                    || Vector3.Distance(transform.position, player6.position) <= Distance || Vector3.Distance(transform.position, player8.position) <= Distance
//                    || Vector3.Distance(transform.position, player9.position) <= Distance)
//                {
//                    UI.SetActive(true);
//                    if (Input.GetKeyDown(KeyCode.Alpha2))
//                    {
//                        Invoke("Explode1", 5.0f); // call this function after 3seconds
//                        Destroy(Bomb, 5.0f);
//                        UI.SetActive(false);
//                        hasExploaded = true;

//                    }

//                }
//                else
//                {
//                    UI.SetActive(false);
//                }
//            }
//            if (isPlayer5)
//            {
//                if (Vector3.Distance(transform.position, player1.position) <= Distance || Vector3.Distance(transform.position, player2.position) <= Distance
//                   || Vector3.Distance(transform.position, player3.position) <= Distance || Vector3.Distance(transform.position, player4.position) <= Distance
//                    || Vector3.Distance(transform.position, player5.position) <= Distance || Vector3.Distance(transform.position, player6.position) <= Distance
//                    || Vector3.Distance(transform.position, player6.position) <= Distance || Vector3.Distance(transform.position, player8.position) <= Distance
//                    || Vector3.Distance(transform.position, player9.position) <= Distance)
//                {
//                    UI.SetActive(true);
//                    if (Input.GetKeyDown(KeyCode.Alpha2))
//                    {
//                        Invoke("Explode1", 5.0f); // call this function after 3seconds
//                        Destroy(Bomb, 5.0f);
//                        UI.SetActive(false);
//                        hasExploaded = true;

//                    }

//                }
//                else
//                {
//                    UI.SetActive(false);
//                }
//            }
//            if (isPlayer6)
//            {
//                if (Vector3.Distance(transform.position, player1.position) <= Distance || Vector3.Distance(transform.position, player2.position) <= Distance
//                   || Vector3.Distance(transform.position, player3.position) <= Distance || Vector3.Distance(transform.position, player4.position) <= Distance
//                    || Vector3.Distance(transform.position, player5.position) <= Distance || Vector3.Distance(transform.position, player6.position) <= Distance
//                    || Vector3.Distance(transform.position, player6.position) <= Distance || Vector3.Distance(transform.position, player8.position) <= Distance
//                    || Vector3.Distance(transform.position, player9.position) <= Distance)
//                {
//                    UI.SetActive(true);
//                    if (Input.GetKeyDown(KeyCode.Alpha2))
//                    {
//                        Invoke("Explode1", 5.0f); // call this function after 3seconds
//                        Destroy(Bomb, 5.0f);
//                        UI.SetActive(false);
//                        hasExploaded = true;

//                    }

//                }
//                else
//                {
//                    UI.SetActive(false);
//                }
//            }
//            if (isPlayer7)
//            {
//                if (Vector3.Distance(transform.position, player1.position) <= Distance || Vector3.Distance(transform.position, player2.position) <= Distance
//                   || Vector3.Distance(transform.position, player3.position) <= Distance || Vector3.Distance(transform.position, player4.position) <= Distance
//                    || Vector3.Distance(transform.position, player5.position) <= Distance || Vector3.Distance(transform.position, player6.position) <= Distance
//                    || Vector3.Distance(transform.position, player6.position) <= Distance || Vector3.Distance(transform.position, player8.position) <= Distance
//                    || Vector3.Distance(transform.position, player9.position) <= Distance)
//                {
//                    UI.SetActive(true);
//                    if (Input.GetKeyDown(KeyCode.Alpha2))
//                    {
//                        Invoke("Explode1", 5.0f); // call this function after 3seconds
//                        Destroy(Bomb, 5.0f);
//                        UI.SetActive(false);
//                        hasExploaded = true;

//                    }

//                }
//                else
//                {
//                    UI.SetActive(false);
//                }
//            }
//            if (isPlayer8)
//            {
//                if (Vector3.Distance(transform.position, player1.position) <= Distance || Vector3.Distance(transform.position, player2.position) <= Distance
//                   || Vector3.Distance(transform.position, player3.position) <= Distance || Vector3.Distance(transform.position, player4.position) <= Distance
//                    || Vector3.Distance(transform.position, player5.position) <= Distance || Vector3.Distance(transform.position, player6.position) <= Distance
//                    || Vector3.Distance(transform.position, player6.position) <= Distance || Vector3.Distance(transform.position, player8.position) <= Distance
//                    || Vector3.Distance(transform.position, player9.position) <= Distance)
//                {
//                    UI.SetActive(true);
//                    if (Input.GetKeyDown(KeyCode.Alpha2))
//                    {
//                        Invoke("Explode1", 5.0f); // call this function after 3seconds
//                        Destroy(Bomb, 5.0f);
//                        UI.SetActive(false);
//                        hasExploaded = true;

//                    }

//                }
//                else
//                {
//                    UI.SetActive(false);
//                }
//            }
//            if (isPlayer9)
//            {
//                if (Vector3.Distance(transform.position, player1.position) <= Distance || Vector3.Distance(transform.position, player2.position) <= Distance
//                    || Vector3.Distance(transform.position, player3.position) <= Distance || Vector3.Distance(transform.position, player4.position) <= Distance
//                     || Vector3.Distance(transform.position, player5.position) <= Distance || Vector3.Distance(transform.position, player6.position) <= Distance
//                     || Vector3.Distance(transform.position, player6.position) <= Distance || Vector3.Distance(transform.position, player8.position) <= Distance
//                     || Vector3.Distance(transform.position, player9.position) <= Distance)
//                {
//                    UI.SetActive(true);
//                    if (Input.GetKeyDown(KeyCode.Alpha2))
//                    {
//                        Invoke("Explode1", 5.0f); // call this function after 3seconds
//                        Destroy(Bomb, 5.0f);
//                        UI.SetActive(false);
//                        hasExploaded = true;

//                    }

//                }
//                else
//                {
//                    UI.SetActive(false);
//                }
//            }
//            if (isPlayer10)
//            {
//                if (Vector3.Distance(transform.position, player1.position) <= Distance || Vector3.Distance(transform.position, player2.position) <= Distance
//                    || Vector3.Distance(transform.position, player3.position) <= Distance || Vector3.Distance(transform.position, player4.position) <= Distance
//                     || Vector3.Distance(transform.position, player5.position) <= Distance || Vector3.Distance(transform.position, player6.position) <= Distance
//                     || Vector3.Distance(transform.position, player6.position) <= Distance || Vector3.Distance(transform.position, player8.position) <= Distance
//                     || Vector3.Distance(transform.position, player9.position) <= Distance)
//                {
//                    UI.SetActive(true);
//                    if (Input.GetKeyDown(KeyCode.Alpha2))
//                    {
//                        Invoke("Explode1", 5.0f); // call this function after 3seconds
//                        Destroy(Bomb, 5.0f);
//                        UI.SetActive(false);
//                        hasExploaded = true;

//                    }

//                }
//                else
//                {
//                    UI.SetActive(false);
//                }
//            }

//        }
//    }

    void Explode1()
    {
        pv.RPC("RPC_Explode",RpcTarget.All);
    }
       void RPC_Explode()
       {
    GameObject bombblast = Instantiate(ExplosionEffect, transform.position, Quaternion.identity) as GameObject;
        player_audio.bombblast();
        Destroy(bombblast, 1);
        characterStats.health = 0;
        characterStats.Die();


    }
}

    //    void  Update()
    //    {
    //        lookforTarget();
    //    }
    //    void lookforTarget()
    // {
    //     if (allCharacters.Length > 0)
    //     {
    //         foreach (Stats c in allCharacters)
    //         {
    //             if (c != characterStats && c.faction != characterStats.faction && c == ClosestEnemy())
    //             {

    //                 if(Input.GetKeyDown(KeyCode.Alpha2))
    //                 {
    //                     Invoke("Explode1", 5.0f); // call this function after 3seconds
    //                      Destroy(Bomb);
    //                        UI.SetActive(false);    
    //                     hasExploaded = true;
    //                 }
    //                 else
    //                 {
    //                     UI.SetActive(false);
    //                 }

    //                     }
    //                 }
    //             }
    //         }

    //  Stats ClosestEnemy()
    //    {
    //        Stats closestCharacter = null;
    //        float minDistance = 5f;
    //        foreach (Stats c in allCharacters)
    //        {
    //            if (c != characterStats && c.faction != characterStats.faction)
    //            {
    //                float distToCharacter = Vector3.Distance(c.transform.position, transform.position);
    //                if (distToCharacter < minDistance)
    //                {
    //                    UI.SetActive(true);
    //                    closestCharacter = c;
    //                    minDistance = distToCharacter;

    //                }
    //            }
    //        }

    //        return closestCharacter;
    //    }
    //}


  
    //Instantiate(ExplosionEffect, transform.position, transform.rotation);
            //Debug.Log("2.cst");
            //Destroy(gameObject);
        
    
    //IEnumerator Explode()
    //{
    //    yield return new WaitForSeconds(0.3f); //itny time ky lye ruke ga 
    //    Explode1();
    //}

