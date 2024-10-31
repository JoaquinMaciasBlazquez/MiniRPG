using UnityEngine;
using UnityEngine.AI;

public class Orc : MonoBehaviour
{

    enum States
    {
        Patrol,
        Chase,
        Attack
    }

    [Header("References")]
    [SerializeField] private NavMeshAgent navMesh;

    [Header("Configuration")]
    [SerializeField] private float fieldOfView;
    [SerializeField] private float stopDistance;
    [SerializeField] private bool drawGizmos;
    [SerializeField] private LayerMask targeteableLayer;
    [Header("Patrol state fields")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float patrolSpeed;
    [Header("Chase state fields")]
    [SerializeField] private float chaseSpeed; 
    [Header("Attack state fields")]
    [SerializeField] private float attackDistance;

    private int currentWaypointIndex;
    private States currentState;
    private Transform target;

    private void OnDrawGizmos() 
    {
        // Si no dibujamos los gizmos, nos salimos del método
        if (!drawGizmos) return;
        // Ponemos el color de los gizmos en rojo
        Gizmos.color = Color.red;
        // Dibujamos la esfera alrededor del enemigo
        Gizmos.DrawWireSphere(transform.position, fieldOfView);
    }

    private void Awake() 
    {
        currentState = States.Patrol;    
    }

    private void Update() 
    {
        switch (currentState)
        {
            case States.Patrol:
            PatrolUpdate();
            break;

            case States.Chase:
            ChaseUpdate();
            break;

            case States.Attack:
            AttackUpdate();
            break;

            default:
            Debug.LogWarning("No sé cómo he llegado aquí.");
            break;
        }
    }

    /// <summary>
    /// Método encargado de cambiar el estado actual al siguiente estado pasado por parámetro
    /// </summary>
    /// <param name="nextState"></param>
    private void SetState(States nextState)
    {
        switch (nextState)
        {
            case States.Patrol:
            navMesh.speed = patrolSpeed;
            break;
            case States.Chase:
            navMesh.speed = chaseSpeed;
            break;
            default:
            break;
        }
        currentState = nextState;
    }

    /// <summary>
    /// Lógica del update del estado de patrol
    /// </summary>
    private void PatrolUpdate()
    {
        // En el caso de que no esté inicializado el array de waypoint o no tenga waypoints...
        if (waypoints == null || waypoints.Length <= 0)
        {
            // Avisamos por consola
            Debug.LogError($"No tienes waypoints asignados en {name}");
            // Salimos del método
            return;
        }
        // Le decimos al nav mesh que se dirija directamente a la posición del waypoint que le toque
        navMesh.SetDestination(waypoints[currentWaypointIndex].position);
        // Si la distancia para llegar al waypoint es menor o igual que la de parada...
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) <= stopDistance)
        {
            // Cambiamos al siguiente waypoint del array. Si llegamos al final, vuelve al primero
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
        // Si el jugador está en el rango de visión...
        if (CanSeePlayer())
        {
            // Cambiamos al estado de chase
            SetState(States.Chase);
        }
    }

    /// <summary>
    /// Función que devuelve true si el jugador está detro del campo de visión del enemigo
    /// </summary>
    /// <returns></returns>
    private bool CanSeePlayer()
    {
        // Creamos un array de collider para usar el overlapsphere para ver si entra el player
        Collider[] colliders = Physics.OverlapSphere(transform.position, fieldOfView, targeteableLayer);
        // Si tenemos collider, se lo asignamos al target, si no es nulo
        target = colliders.Length > 0 ? colliders[0].transform : null;
        // Devolvemos si el target es diferente de nulo; es decir, si tenemos al jugador dentro del campo de visión
        return target != null;
    }

    /// <summary>
    /// Lógica del update del estado de chase
    /// </summary>
    private void ChaseUpdate()
    {
        // Si deja de ver al jugador...
        if (!CanSeePlayer())
        {
            // Volvemos al estado de patrulla
            SetState(States.Patrol);
        }
        // Si la distancia con el jugador es mejor que la de ataque...
        if (Vector3.Distance(transform.position, target.position) <= attackDistance)
        {
            // Pasamos al estado de ataque
            SetState(States.Attack);
        }
        // Mientras tanto, para el chaseo, le asignamos como destino la posición del target (el jugador)
        navMesh.SetDestination(target.position);
    }

    /// <summary>
    /// Lógica del update del estado de attack
    /// </summary>
    private void AttackUpdate()
    {
        Debug.Log("Attacking");
    }
}