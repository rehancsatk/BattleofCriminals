using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MyPlayer))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(Animator))]
public class PlayerAI : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navmesh;
    private MyPlayer characterMove { get { return GetComponent<MyPlayer>(); } set { characterMove = value; } }
    private Animator animator { get { return GetComponent<Animator>(); } set { animator = value; } }
    private Stats characterStats { get { return GetComponent<Stats>(); } set { characterStats = value; } }

    private WeaponHandlerAI weaponHandler { get { return GetComponent<WeaponHandlerAI>(); } set { weaponHandler = value; } }
    public enum AIState { Patrol, Attack}
    public AIState aiState;
    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    [System.Serializable]
    public class PatrolSettings
    {
        public WaypointBase[] waypoints;
       
    }
    public PatrolSettings patrolSettings;

    public Transform[] points;
    [System.Serializable]
    public class SightSettings
    {
        public LayerMask sightLayers;                    // yeh check kre ga kn kn si layers walo ko attack krna hh
        public float sightRange = 30f;
        public float fieldOfView = 120f;
        public float eyeheight;
    }
    public SightSettings sight;

    [System.Serializable]
    public class AttackSettings
    {
        public float fireChance = 0.1f;
    }
    public AttackSettings attack;

    private float currentWaitTime;
    private int waypointIndex;
    private Transform currentLookTransform;
    private bool walkingToDest;
    private bool setDestination;
    private bool reachDestination;

    private float forward;

    private bool aiming;

    private Transform target;
    private Vector3 targetLastKnownPosition;
    private Stats[] allCharacters;

    // Use this for initialization
    void Start()
    {
        navmesh = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
       
        if (navmesh == null)
        {
            Debug.LogError("We need a navmesh to traverse the world with.");
            enabled = false;
            return;
        }

        if (navmesh.transform == this.transform)
        {
            Debug.LogError("The navmesh agent should be a child of the character: " + gameObject.name);
            enabled = false;
            return;
        }

        navmesh.speed = 0;
        navmesh.acceleration = 0;
        navmesh.autoBraking = false;

        if (navmesh.stoppingDistance == 0)
        {
            Debug.Log("Auto settings stopping distance to 1.3f");
            navmesh.stoppingDistance = 1.3f;
        }

        
    }

    void GetAllCharacters()
    {
        allCharacters = GameObject.FindObjectsOfType<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        GetAllCharacters();
        //TODO: Animate the strafe when the enemy is trying to shoot us.
        characterMove.Animate(forward, 0);
        navmesh.transform.position = transform.position;

        weaponHandler.Aim(aiming);

        LookForTarget();

        switch (aiState)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Attack:
                FireatEnemy();
                break;

        }
    }

    void LookForTarget()
    {
       
        if (allCharacters.Length > 0)
        {
            foreach (Stats c in allCharacters)
            {
                if (c != characterStats && c.faction != characterStats.faction && c == ClosestEnemy())
                {
                    RaycastHit hit;
                    Vector3 start = transform.position + (transform.up * sight.eyeheight);
                    Vector3 dir = (c.transform.position + c.transform.up) - start;
                    float sightAngle = Vector3.Angle(dir, transform.forward);
                    if (Physics.Raycast(start, dir, out hit, sight.sightRange, sight.sightLayers) &&
                        sightAngle < sight.fieldOfView && hit.collider.GetComponent<Stats>())
                    {
                        target = hit.transform;
                        targetLastKnownPosition = Vector3.zero;
                    }
                    else
                    {
                        if (target != null)
                        {
                            targetLastKnownPosition = target.position;
                            target = null;
                            aiState = AIState.Patrol;
                        }
                    }
                }
            }
        }
    }

    Stats ClosestEnemy()
    {
        
        Stats closestCharacter = null;
        float minDistance = Mathf.Infinity;
        foreach (Stats c in allCharacters)
        {
            if (c != characterStats && c.faction != characterStats.faction)
            {
               
                float distToCharacter = Vector3.Distance(c.transform.position, transform.position);
                if (distToCharacter < minDistance)
                {
                    closestCharacter = c;
                    minDistance = distToCharacter;
                }
            }
        }

        return closestCharacter;
    }
   
    void Patrol()
    {
        if (target == null)
        {
            PatrolBehaiour();
            if (!navmesh.isOnNavMesh)
            {
                Debug.Log("We're off the navmesh");
                return;
            }
             

       

            if (!setDestination)
            {
                 SetNewRandomDestination();
                setDestination = true;
            }
           

            if ((navmesh.remainingDistance <= navmesh.stoppingDistance) || reachDestination && !navmesh.pathPending) 
            {
                setDestination = false;
                walkingToDest = false;
                forward = LerpSpeed(forward, 0, 15);
                currentWaitTime -= Time.deltaTime;

                if (patrolSettings.waypoints[waypointIndex].lookAtTarget != null)
                    currentLookTransform = patrolSettings.waypoints[waypointIndex].lookAtTarget;
                //if (currentWaitTime <= 0)
                //{
                //    waypointIndex = (waypointIndex + 1) % points.Length;
                //    reachDestination = false;
                //}
                //else
                //{
                //    reachDestination = true;
                //}
            }
            else
            {
                LookAtPosition(navmesh.steeringTarget);
                walkingToDest = true;
                forward = LerpSpeed(forward, 0.5f, 15);
                currentWaitTime = patrolSettings.waypoints[waypointIndex].waitTime;
                currentLookTransform = null;
            }
        }
        else
        {
            aiState = AIState.Attack;
        }
    }
    void SetNewRandomDestination()
    {
        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);
        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1); // uski nahae posirion generate kr dy ga 
        navmesh.SetDestination(navHit.position);
    }
    void FireatEnemy()
    {
        if(target !=null)
        {
            AttackBehaviour();
            LookAtPosition(target.position);
            Vector3 start = transform.position + transform.up;
            Vector3 dir = target.position - transform.position;
            Ray ray = new Ray(start, dir);
            if (Random.value <= attack.fireChance)
                weaponHandler.FireCurrentWeapon(ray);
           

        }

    }

    float LerpSpeed(float curSpeed, float destSpeed, float time)
    {
        curSpeed = Mathf.Lerp(curSpeed, destSpeed, Time.deltaTime * time);
        return curSpeed;
    }

    void LookAtPosition(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        lookRot.x = 0;
        lookRot.z = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * 5);
    }

    void OnAnimatorIK()
    {
        if (currentLookTransform != null && !walkingToDest)
        {
            animator.SetLookAtPosition(currentLookTransform.position);
            animator.SetLookAtWeight(1, 0, 0.3f, 0.2f);
        }
        else if (target != null)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if(dist >3)
            {
                animator.SetLookAtPosition(target.transform.position + transform.right * 0.3f);
                animator.SetLookAtWeight(1, 1, 0.3f, 0.2f);        
            }
            else
            {
                animator.SetLookAtPosition(target.transform.position + target.up + transform.right * 0.3f);
                animator.SetLookAtWeight(1, 1, 0.3f, 0.2f);
            }
        }
    }


    void PatrolBehaiour()
    {
        aiming = false;
    }
    void AttackBehaviour()
    {
        aiming = true;
        walkingToDest = false;
        setDestination = false;
        reachDestination = false;
        currentLookTransform = null;
        forward = LerpSpeed(forward, 0, 15);
    }
}

[System.Serializable]
public class WaypointBase
{
    public Transform destination;
    public float waitTime;
    public Transform lookAtTarget;
}