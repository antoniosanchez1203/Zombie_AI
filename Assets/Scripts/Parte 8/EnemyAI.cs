using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Chase, Attack }

    [Header("Configuración de Rangos")]
    [SerializeField] private float chaseRange = 15f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Configuración de Ataque")]
    [SerializeField] private float attackCooldown = 1.5f; // Segundos entre cada golpe
    private float attackTimer = 0f;

    private NavMeshAgent agent;
    private Animator anim;
    private Transform player;
    private EnemyState currentState = EnemyState.Idle;

    // Estos hashes apuntan EXACTAMENTE a los parámetros de tu controlador ZombieActions
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // Obtiene el Animator conectado a ZombieActions
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        bool canSee = CanSeePlayer(dist);

        // Máquina de estados optimizada
        UpdateState(dist, canSee);

        // Control y ejecución del comportamiento
        ExecuteState();
    }

    private void UpdateState(float dist, bool canSee)
    {
        if (dist <= attackRange && canSee)
            currentState = EnemyState.Attack;
        else if (dist <= chaseRange && canSee)
            currentState = EnemyState.Chase;
        else
            currentState = EnemyState.Idle;
    }

    private void ExecuteState()
    {
        switch (currentState)
        {
            case EnemyState.Chase:
                agent.isStopped = false;
                agent.SetDestination(player.position);
                // Pasa la velocidad real del NavMeshAgent al Float 'Speed' de ZombieActions
                anim.SetFloat(SpeedHash, agent.velocity.magnitude, 0.1f, Time.deltaTime);
                break;

            case EnemyState.Attack:
                agent.isStopped = true;
                // Frena la animación de movimiento de forma suavizada
                anim.SetFloat(SpeedHash, 0f, 0.1f, Time.deltaTime);

                // Temporizador profesional para que el Trigger de ataque no se sature en bucle
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackCooldown)
                {
                    anim.SetTrigger(AttackTrigger); // Dispara 'AttackTrig' una sola vez
                    attackTimer = 0f;
                }
                break;

            case EnemyState.Idle:
                agent.isStopped = true;
                anim.SetFloat(SpeedHash, 0f, 0.1f, Time.deltaTime);
                // Resetea el temporizador para que ataque instantáneamente al volver a alcanzar al jugador
                attackTimer = attackCooldown;
                break;
        }
    }

    private bool CanSeePlayer(float dist)
    {
        Vector3 dir = (player.position - transform.position).normalized;
        // Lanzamiento de rayo a 1.5 metros de altura (altura estimada de los ojos del enemigo)
        return !Physics.Raycast(transform.position + Vector3.up * 1.5f, dir, dist, obstacleMask);
    }

    // Dibujado visual de rangos en la pestaña "Scene" para facilitar tu diseño
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}