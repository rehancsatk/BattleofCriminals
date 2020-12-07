using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Photon.Pun;

public class Stats : MonoBehaviour
{
    //Ontrigger triggerValue;
   
    private EnemyAnimations enemy_Anim;
    private NavMeshAgent navAgent;
    private BoarController enemy_Controller;
    private EnemyAudio enemyAudio;
    private PhotonView Pv;
    private Player_Audio player_audio;
 
   private CharacterController charactercontroller { get { return GetComponent<CharacterController>(); } set { charactercontroller = value;  } }
   private RagdolHandler ragdollhanndler { get { return GetComponentInChildren<RagdolHandler>(); } set { ragdollhanndler = value; } }

    [Range(0, 100)] public float health = 100;
    public int faction;

    bool isbomb;
    public MonoBehaviour[] scripttoDisable;


    public float sec = 2f;

    public Color damageColor;
    public GameObject DamageImage;
    float colorSmoothing =6f;
    bool isTakingDamage=false;

    public bool  is_Boar, is_Cannibal,is_Player;
    public bool is_Dead;
    bool bomb;
    public bool AIPlayer;
   
    void Awake()
    {
       
        if (is_Player)
        {
            Pv = GetComponent<PhotonView>();
            player_audio = GetComponentInChildren<Player_Audio>();
            DamageImage = GameObject.FindGameObjectWithTag("Damage");
            DamageImage.SetActive(false);
        }

        if(AIPlayer)
        {
            Pv = GetComponent<PhotonView>();
            player_audio = GetComponentInChildren<Player_Audio>();
        }
        if( is_Cannibal)
        {
            Pv = GetComponent<PhotonView>();
            enemyAudio = GetComponentInChildren<EnemyAudio>();
           // enemy_Anim = GetComponent<EnemyAnimations>();
           // enemy_Controller = GetComponent<BoarController>();
           // navAgent = GetComponent<NavMeshAgent>();

            //get enemy audio as well
           // enemyAudio = GetComponentInChildren<EnemyAudio>();
        }
           if (is_Boar)
           {
               Pv = GetComponent<PhotonView>();
               enemyAudio = GetComponentInChildren<EnemyAudio>();
           }

    }

    void Update()
    {
        
       
        if (Pv.IsMine)
        {
            health = Mathf.Clamp(health, 0, 100);
            //Display_HealthStats(health);
            if (is_Player)
            {
                if (isTakingDamage)
                {
                    DamageImage.SetActive(true);
                }
                else
                {
                    DamageImage.SetActive(false);
                    //DamageImage.color = Color.Lerp(DamageImage.color, Color.clear, colorSmoothing * Time.deltaTime);
                }
            }
            isTakingDamage = false;
        }
    }
    public void Damage ()
    {
        if(AIPlayer)
        {
            health -= 10;
            player_audio.hit_Sound();
          

            if (health <= 0)
                Die();
        }
        
        if (is_Player )
        {
            health -= 10;
            player_audio.hit_Sound();
            isTakingDamage = true; 

            if (health <= 0)
                Die();
            isTakingDamage = true;
        }
        if (is_Cannibal)
        {
            enemyAudio.Play_ScreamSound();
            health -= 20;

            if (health <= 0)
                Died();
        }
        if (is_Boar )
        {
            if (is_Dead)
                return;
            enemyAudio.Play_ScreamSound();
               health -= 50;
              if (health <= 0)
                Died();
          
         
            
        }
       
        
    }
    public void Died()
    {
        charactercontroller.enabled = false;
        //Destroy(player0, 5f);
        StartCoroutine(DeadSound());
        if (scripttoDisable.Length == 0)
        {
            Debug.Log("All scripts which are running on it but player is dead");
            return;
        }
            
            foreach (MonoBehaviour script in scripttoDisable)
                script.enabled = false;

            if (ragdollhanndler != null)
                ragdollhanndler.Ragdoll();
            Destroy(gameObject, 10f);
        
    }
    public void Die()
    {
        Pv.RPC("Rpc_Die", RpcTarget.All);


    }
    [PunRPC]
   public void Rpc_Die()
    {
      

        charactercontroller.enabled = false;
        //Destroy(player0, 5f);
        StartCoroutine(PlayerDeathSound());
        if (scripttoDisable.Length == 0)
        {
            Debug.Log("All scripts which are running on it but player is dead");
            return;
        }
        
        
        //if (is_Boar)
        //{
        //    navAgent.velocity = Vector3.zero;
        //    navAgent.isStopped = true;
        //    enemy_Controller.enabled = false;

        //    enemy_Anim.Dead();
        //    Debug.Log("yeh animation chali chaye");
        //    StartCoroutine(DeadSound());
        //    EnemyManager.instance.EnemyDied(false);
        //    is_Dead = true;
        //    Destroy(gameObject,10f);
        //}
     

        foreach (MonoBehaviour script in scripttoDisable)
          script.enabled = false;

        if (ragdollhanndler != null)
            ragdollhanndler.Ragdoll();
    }
 
    /*void PlayerDied()
    {
        if (is_Cannibal)
        {
            // GetComponent<Animator>().enabled = false;
            // GetComponent<BoxCollider>().isTrigger = false;
            // GetComponent<Rigidbody>().AddTorque(-transform.forward * 5f); // it move some backwaeds to its position which indicates that it died

            enemy_Controller.enabled = false;
            navAgent.enabled = false;
            enemy_Anim.enabled = false;
            Debug.Log("POUNCH AYA HN");
            enemy_Anim.Dead();

            StartCoroutine(DeadSound());
            // because we don't have death animations
            EnemyManager.instance.EnemyDied(true);
        }
     


    }//this function indicates player died
    */

    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f); //itny time ky lye ruke ga 
        enemyAudio.Play_DiedSound();
    }
    IEnumerator PlayerDeathSound()
    {
        yield return new WaitForSeconds(0.3f); //itny time ky lye ruke ga 
        player_audio.DiedSound();
    }
    
}
