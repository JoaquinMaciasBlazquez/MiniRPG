using UnityEngine;
using UnityEngine.AI;

public class Orc : MonoBehaviour
{

    enum States
    {
        Patrol,
        Chase,
        Attack,
        Search
    }

    [Header("References")]
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] private Animator animator;   

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
    [Header("Search state fields")]
    [SerializeField] private float searchTime;

    private int currentWaypointIndex;
    private States currentState;
    private Transform target;
    private float searchTimeCounter;

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
        SetState(States.Patrol);    
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

            case States.Search:
            SearchUpdate();
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
        // En base al estado actual, ejecutamos su lógica de salida
        switch (currentState)
        {
            case States.Patrol:
            PatrolExit();
            break;
            case States.Chase:
            ChaseExit();
            break;
            case States.Attack:
            AttackExit();
            break;
            case States.Search:
            SearchExit();
            break;
            default:
            break;
        }
        
        // Actualizamos el estado actual cambiándolo por el que estado que vamos a cambiar
        currentState = nextState;
        
        // En base al estado al que hemos entrado, ejecutamos una entrada u otra
        switch (currentState)
        {
            case States.Patrol:
            PatrolEnter();
            break;
            case States.Chase:
            ChaseEnter();
            break;
            case States.Attack:
            AttackEnter();
            break;
            case States.Search:
            SearchEnter();
            break;
            default:
            break;
        }
    }

    /// <summary>
    /// Método que se ejecutará cuando se entre en el estado de patrulla
    /// </summary>
    private void PatrolEnter()
    {
        navMesh.speed = patrolSpeed;
        animator.SetFloat(Constants.ANIM_ORC_SPEED, 0f);
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
    /// Método que ejecutará toda la lógica que queramos que se ejecute al salir del estado de patrulla
    /// </summary>
    private void PatrolExit()
    {

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
    /// Método que se ejecutará cuando entremos en el estado de chase
    /// </summary>
    private void ChaseEnter()
    {
        navMesh.speed = chaseSpeed;
        animator.SetFloat(Constants.ANIM_ORC_SPEED, 1f);
    }

    /// <summary>
    /// Lógica del update del estado de chase
    /// </summary>
    private void ChaseUpdate()
    {
        // Si deja de ver al jugador...
        if (!CanSeePlayer())
        {
            // Vamos al estado de búsqueda
            SetState(States.Search);
            // Salimos del método de forma defensiva para ahorrarnos referencias nulas
            return;
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
    /// Método que se ejecutará en la salida del estado de chase
    /// </summary>
    private void ChaseExit()
    {

    }

    /// <summary>
    /// Método que se ejecutará en la entrada del estado de ataque
    /// </summary>
    private void AttackEnter()
    {

    }

    /// <summary>
    /// Lógica del update del estado de attack
    /// </summary>
    private void AttackUpdate()
    {
        Debug.Log("Attacking");
    }

    /// <summary>
    /// Método que se ejecutará en la salida del estado de ataque
    /// </summary>
    private void AttackExit()
    {

    }

    /// <summary>
    /// Entrada del estado de búsqueda
    /// </summary>
    private void SearchEnter()
    {
        // Paramos el nav mesh
        navMesh.isStopped = true;
        // Asignamos al temporizador el tiempo que dura la búsqueda
        searchTimeCounter = searchTime;
        // Ejecutamos la animación de búsqueda
        animator.SetTrigger(Constants.ANIM_ORC_SEARCH);
    }

    /// <summary>
    /// Update del estado de búsqueda
    /// </summary>
    private void SearchUpdate() 
    {
        // En el caso de que vuelva a ver al jugador
        if (CanSeePlayer())
        {
            // Pasamos al estado de chase
            SetState(States.Chase);
        }
        // Vamos restando el time.deltaTime al counter para que sea una cuenta regresiva
        searchTimeCounter -= Time.deltaTime;
        // En el caso de que termine el tiempo de búsqueda...
        if (searchTimeCounter <= 0f)
        {
            // Volvemos al estado de patrulla
            SetState(States.Patrol);
        }
    }

    /// <summary>
    /// Salida del estado de búsqueda
    /// </summary>
    private void SearchExit()
    {
        // Volvemos a hacer que el nav mesh se pueda mover
        navMesh.isStopped = false;
        // Le decimos al orco que vuelva a su animación de movimiento:
        // lo hacemos aquí directamente porque sabemos que de la búsqueda va a ir al movimiento
        animator.SetTrigger(Constants.ANIM_ORC_MOTION);
    }
}