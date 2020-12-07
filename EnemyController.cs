using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    Collision ABC;
     Animator anim;
     private EnemyAnimations enemy_Anim;
    private NavMeshAgent navAgent;

    private EnemyState enemy_State;

    public float walk_speed = 1f;
    public float run_speed = 3f;

    public float chase_Distance = 15f;
    private float current_chase_distance;
    public float attack_Distance = 0.5f;
    public float chase_after_attack_Distance = 1f;
   float accuracyWp =5.0f;
    
   // public float patrol_for_this_Time = 15f;
    //private float patrol_Timer;
    float rotSpeed = 0.2f;
    public float wait_Before_Attack = 4f;
    private float attack_Timer;

    public Transform player;
    int currentWp=0;

    public GameObject attack_point;
   
    public GameObject[] waypoints;

    private EnemyAudio enemy_Audio;
    private Transform target;
    //public List<Transform> target = new List<Transform>();
    //Stats stat;

    void Awake()
    {
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        enemy_Anim = GetComponent<EnemyAnimations>();
        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
       // stat = GetComponent<Stats>();
        enemy_Audio = GetComponentInChildren<EnemyAudio>();
        

    }
    void Start()
    {
        enemy_State = EnemyState.PATROL;

       // patrol_Timer = patrol_for_this_Time;
        currentWp = 0;

        attack_Timer = wait_Before_Attack; //jb enemy pehle attack kre gi 

        current_chase_distance = chase_Distance;

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy_State == EnemyState.PATROL)
        {
            Patrol();
        }
        if (enemy_State == EnemyState.CHASE)
        {
            Chase();
        }
        if (enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }
    }
    void Patrol()
    {
        navAgent.isStopped = false; //navagent ko bate ga ky wo move kre
        navAgent.speed = walk_speed; //chale ga 
        navAgent.SetDestination(waypoints[currentWp].transform.position);

        Vector3 direction = target.position- this.transform.position;
        //direction.y = 0;
        if (waypoints.Length > 0)
        {


            enemy_Anim.Walk(true);
            if (Vector3.Distance(waypoints[currentWp].transform.position, transform.position) < accuracyWp) // hr waypoint py jae ga
            {
                // currentWp = Random.Range(0, waypoints.Length);
                currentWp++;
                if (currentWp >= waypoints.Length) // agr sb waypoints complete hu gye then restart from 1st
                {
                    currentWp = 0;
                }
            }
            direction = waypoints[currentWp].transform.position - transform.position;

            this.transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime); // towards waypoint with rotation of whole skeleton 

        }
        //it will test the distance b/w player and enemy
        if (Vector3.Distance(target.position, transform.position) <= chase_Distance)
        {
            enemy_Anim.Walk(false);

            enemy_State = EnemyState.CHASE;

            //play sound
            enemy_Audio.Play_ScreamSound();
        }

        }

    void Turn_On_AttackPoint()
    {
        attack_point.SetActive(true);
    }
    void Turn_Off_AttackPoint()
    {
        if (attack_point.activeInHierarchy)
        {
            attack_point.SetActive(false);
        }
    }

    void Chase()
    {
        // dobra enable kre ga aur sppedd ko barha dy ga
        navAgent.isStopped = false;
        navAgent.speed = run_speed;
        Vector3 direction = target.position - this.transform.position;
        navAgent.SetDestination(target.position); //player ko chase kre ga

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

        if (navAgent.velocity.sqrMagnitude > 1)
        {
            enemy_Anim.Run(true);
            anim.SetBool("Attack", false);
        }
        else
        {
            enemy_Anim.Run(false);

        }
        if (Vector3.Distance(target.position, transform.position) <= attack_Distance)
        {
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);

            
            enemy_State = EnemyState.ATTACK; //animations stop kr ky attack hu ga phr

            if (chase_Distance != current_chase_distance)
            {
                chase_Distance = current_chase_distance; //dobara previous wala kam hu jae start sy
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chase_Distance)
        {
            enemy_Anim.Run(false);

            enemy_State = EnemyState.PATROL;
            //reset kr dy ga patrol fuction ko aur wo dobra start hu ga
            currentWp = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

            if (chase_Distance != current_chase_distance)
            {
                chase_Distance = current_chase_distance; //dobara previous wala kam hu jae start sy
            }
        }
    }
    void Attack()
    {
        Vector3 direction = target.position - this.transform.position;
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_Timer += Time.deltaTime;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
        if (attack_Timer > wait_Before_Attack)
        {
            

            anim.SetBool("Attack", true);
            anim.SetBool("Run", false);
          
            attack_Timer = 0f;

            //play attack sound

            enemy_Audio.Play_AttackSound();
        }
      
        if (Vector3.Distance(target.position, transform.position) >= attack_Distance + chase_after_attack_Distance)
        {
            anim.SetBool("Attack", false);
            enemy_State = EnemyState.CHASE;
            
        }
    }
  

    
  
    public EnemyState Enemy_State
    {
        get;
        set;
    }
}
