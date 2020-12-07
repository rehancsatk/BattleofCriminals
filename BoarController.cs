using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BoarController : MonoBehaviour
{
    private EnemyAnimations enemy_Anim;
    private NavMeshAgent navAgent;

    private EnemyState enemy_State;

    public float walk_speed = 0.5f;
    public float run_speed = 4f;

    public float chase_Distance = 7f;
    private float current_chase_distance;
    public float attack_Distance = 1.8f;
    public float chase_after_attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_for_this_Time = 15f;
    private float patrol_Timer;

    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    private Transform target;

    public GameObject attack_Point;

    private EnemyAudio enemy_Audio;

    void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimations>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        enemy_Audio = GetComponentInChildren<EnemyAudio>();
    }
    void Start()
    {
        enemy_State = EnemyState.PATROL;

        patrol_Timer = patrol_for_this_Time;

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

        patrol_Timer += Time.deltaTime;

        if (patrol_Timer > patrol_for_this_Time)
        {
            SetNewRandomDestination();

            patrol_Timer = 0f;
        }
        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Walk(true);
        }
        else
        {
            enemy_Anim.Walk(false);
        }
        //it will test the distance b/w player and enemy
        if (Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {
            enemy_Anim.Walk(false);

            enemy_State = EnemyState.CHASE;

            //play sound
            enemy_Audio.Play_ScreamSound();
        }

    }
    void Chase()
    {
        // dobra enable kre ga aur sppedd ko barha dy ga
        navAgent.isStopped = false;
        navAgent.speed = run_speed;

        navAgent.SetDestination(target.position); //player ko chase kre ga


        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Run(true);
        }
        else
        {
            enemy_Anim.Run(false);
        }
        if (Vector3.Distance(transform.position, target.position) <= attack_Distance)
        {
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK; //animations stop kr ky attack hu ga phr

            if (chase_Distance != current_chase_distance)
            {
                chase_Distance = current_chase_distance; //dobara previous wala kam hu jae start sy
            }
        }
        else if (Vector3.Distance(transform.position, target.position) > chase_Distance)
        {
            enemy_Anim.Run(false);
            enemy_State = EnemyState.PATROL;
            //reset kr dy ga patrol fuction ko aur wo dobra start hu ga
            patrol_Timer = patrol_for_this_Time;
            if (chase_Distance != current_chase_distance)
            {
                chase_Distance = current_chase_distance; //dobara previous wala kam hu jae start sy
            }
        }
    }
    void Attack()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_Timer += Time.deltaTime;

        if (attack_Timer > wait_Before_Attack)
        {
            enemy_Anim.Attack();

            attack_Timer = 0f;

            //play attack sound

            enemy_Audio.Play_AttackSound();
        }
        if (Vector3.Distance(transform.position, target.position) >= attack_Distance + chase_after_attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
        }
    }
    void SetNewRandomDestination()
    {
        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);
        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1); // uski nahae posirion generate kr dy ga 
        navAgent.SetDestination(navHit.position);
    }



    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }
    void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }

    public EnemyState Enemy_State
    {
        get;
        set;
    }
}
