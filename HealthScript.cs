using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HealthScript : MonoBehaviour
{
    private EnemyAnimations enemy_Anim;
    private NavMeshAgent navAgent;
    private BoarController enemy_Controller;

    public float health = 100f;

    //private Stats player_Stats;

    private EnemyAudio enemyAudio;

    public bool  is_Boar, is_Cannibal;
    private bool is_Dead;
    void Awake()
    {
        if (is_Boar || is_Cannibal)
        {
            enemy_Anim = GetComponent<EnemyAnimations>();
            enemy_Controller = GetComponent<BoarController>();
            navAgent = GetComponent<NavMeshAgent>();

            //get enemy audio as well
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }
        
    }


    public void ApplyDamage(float damage)
    {
        if (is_Dead)
            return; // agr mar gya hu ga tou return kr jae ga agla code nh exe kr ga

        health -= damage;

        if (is_Boar || is_Cannibal)
        {
            if (enemy_Controller.Enemy_State == EnemyState.PATROL)
            {
                enemy_Controller.chase_Distance = 50f;
            }
            if (health <= 0f)
            {
                PlayerDied();
                is_Dead = true;
            }
        }
    }

    void PlayerDied()
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
        if (is_Boar)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_Controller.enabled = false;

            enemy_Anim.Dead();

            StartCoroutine(DeadSound());

            EnemyManager.instance.EnemyDied(false);
        }
       
       
    }//this dunction indicates player died

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        // dobara scene start sy load hu jae ga
    }
    void TurnOffTheGameObjects()
    {
        gameObject.SetActive(false);
    }
    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f); //itny time ky lye ruke ga 
        enemyAudio.Play_DiedSound();
    }
}
