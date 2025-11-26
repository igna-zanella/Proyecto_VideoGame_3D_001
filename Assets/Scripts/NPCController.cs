using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform[] patrolPoints;        // Puntos para caminar en circulo
    public float detectionRadius = 5f;      // Distancia para detectar al jugador
    public float fleeDistance = 10f;        // Distancia a la cual huye del jugador
    public AudioSource screamSound;         // Clip del grito

    private NavMeshAgent agent;
    private Animator anim;
    private int patrolIndex = 0;
    private Transform player;
    private bool isFleeing = false;

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

        // Actualiza animación según velocidad
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    // -------------------------------
    //        PATRULLA
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

        agent.speed = 1.5f; // velocidad caminando
        agent.SetDestination(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    // -------------------------------
    //     DETECCIÓN DEL JUGADOR
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
    //          HUIR
    // -------------------------------
    void StartFlee()
    {
        isFleeing = true;
        agent.speed = 4.5f; // velocidad corriendo

        if (!screamSound.isPlaying)
            screamSound.Play();
    }

    void UpdateFleeBehaviour()
    {
        Vector3 dirFlee = (transform.position - player.position).normalized;
        Vector3 targetPos = transform.position + dirFlee * fleeDistance;

        agent.SetDestination(targetPos);

        // Si se alejó lo suficiente, vuelve a patrullar
        if (Vector3.Distance(transform.position, player.position) > fleeDistance)
        {
            isFleeing = false;
            GoToNextPatrolPoint();
        }
    }

    // -------------------------------
    //     DIBUJO DEL RADIO
    // -------------------------------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
