using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float detectionRadius = 5f;
    public float fleeDistance = 10f;
    public float minScreamDistance = 3f;    // Distancia mínima para que grite
    public AudioSource screamSound;

    private NavMeshAgent agent;
    private Animator anim;
    private int patrolIndex = 0;
    private Transform player;
    private bool isFleeing = false;
    private bool screamPlayed = false;      // Evita repetir el grito

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (isFleeing)
        {
            UpdateFleeBehaviour();
        }
        else
        {
            PatrolBehaviour();
            DetectPlayer();
        }

        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    // -------------------------------
    //            PATRULLA
    // -------------------------------
    void PatrolBehaviour()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.speed = 1.5f;
        agent.SetDestination(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;

        screamPlayed = false; // Para permitir gritar en el próximo encuentro
    }

    // -------------------------------
    //        DETECCIÓN DEL JUGADOR
    // -------------------------------
    void DetectPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detectionRadius)
        {
            StartFlee();
        }
    }

    // -------------------------------
    //              HUIR
    // -------------------------------
    void StartFlee()
    {
        isFleeing = true;
        agent.speed = 4.5f;

        float playerDistance = Vector3.Distance(transform.position, player.position);

        // Solo gritar si el jugador está muy cerca y aún no gritó
        //if (!screamPlayed && playerDistance <= minScreamDistance)
            Debug.LogWarning(playerDistance);
        screamSound.Play();

        if (playerDistance <= minScreamDistance)
            {
            screamPlayed = true;
           

        }
        else
        {
            print("grito");
        }
    }

    void UpdateFleeBehaviour()
    {
        Vector3 dirFlee = (transform.position - player.position).normalized;
        Vector3 targetPos = transform.position + dirFlee * fleeDistance;

        agent.SetDestination(targetPos);

        if (Vector3.Distance(transform.position, player.position) > fleeDistance)
        {
            isFleeing = false;
            GoToNextPatrolPoint();
        }
    }

    // -------------------------------
    //    DIBUJO DEL RADIO DE ALARMA
    // -------------------------------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minScreamDistance);
    }
}
