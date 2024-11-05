using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    // Referencia al animator
    [SerializeField] private Animator animator;
    // Referencia al nav mesh agent
    [SerializeField] private NavMeshAgent navMesh;

    [Header("Configuration")]
    // Velocidad a la que se desplazar� el jugador.
    [SerializeField] private float speed;
    // Aceleración que tendrá el personaje
    [SerializeField] private float acceleration;
    // Velocidad de giro que tendrá el personaje
    [SerializeField] private float angularSpeed;
    // Velocidad de rotación propia
    [SerializeField] private float rotationSpeed;
    // Distancia a la que se parará el nav mesh
    [SerializeField] private float stoppingDistance;
    // Layer que tendr� asignado el suelo para diferenciarlo.
    [SerializeField] private LayerMask groundLayer;
    // Cadencia de disparo
    [SerializeField] private float shootCadency;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;

    // Direcci�n a la que se tiene que desplazar el jugador.
    private Vector3 targetPosition;
    // Si true, indicar� que est� ya en movimiento.
    private bool isMoving;
    // Referencia a la c�mara.
    private Camera mainCamera;
    // Variable que contrendr� el n�mero de monedas actuales.
    private int currentCoins;
    // Si true, podrá disparar
    private bool canShoot;
    // Corrutina de disparo
    private Coroutine shootCoroutine;

    private void Awake() {

        CheckReferences();
    }

    private void Start() {

        currentCoins = 0;
        canShoot = true;
        InitializeAgent();
    }

    private void Update() {

        // Si se est� moviendo...
        if (isMoving) {
            MoveToTarget();
        }
    
        Inputs();
        CheckMovement();
        UpdateAnimator();
    }

    private void OnTriggerEnter(Collider other) {

        // Si el collider que hemos recuperado que es trigger tiene la interfaz IPickeable...
        if (other.TryGetComponent(out IPickeable pickeable)) {
            // Ejecutamos la l�gica de pick up
            pickeable.PickUp();
        }
    }

    private void OnEnable() {
        // Nos suscribimos al evento que lanza la moneda
        Coin.OnCoinCollected += CollectCoin;   
    }

    private void OnDisable() {
        // Nos desuscribimos al evento que lanza la moneda
        Coin.OnCoinCollected -= CollectCoin;   
    }

    private void Inputs() {

        // Si se presiona la tecla E...
        if (Input.GetKeyDown(KeyCode.E)) {
            // Intentamos disparar
            TryShoot();
        }
    }

    /// <summary>
    /// Comprueba las referencias que sean necesarias para la ejecuci�n del script.
    /// </summary>
    private void CheckReferences() {

        // En el caso de que el animator sea nulo; es decir, no est� asignado...
        if (animator == null) {
            // Buscamos el componente animator en los hijos.
            animator = GetComponentInChildren<Animator>();
        }

        // En el caso de que el nav mesh sea nulo; es decir, no est� asignado...
        if (navMesh == null) {
            // Buscamos el componente nav mesh agent en los componentens del mismo objeto.
            navMesh = GetComponent<NavMeshAgent>();
        }

        mainCamera = Camera.main;
    }

    /// <summary>
    /// M�todo que lanzar� la l�gica de movimiento cuando sea necesario.
    /// </summary>
    private void CheckMovement() {

        // Detectamos si el jugador hace click...
        if (Input.GetMouseButtonDown(0)) {

            // Nos creamos un rayo desde la posici�n en la que se encuentra el rat�n en base a la c�mara.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Lanzamos el rayo que hemos creado antes con una longitud infinita,
            // solamente entraremos en el if SI Y SOLO SI colisiona con un objeto que tenga el layer de ground
            // declaramos de manera local la variable hit para poder acceder a ella dentro del if.
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer)) {

                // Guardamos el punto al que tenemos que ir en base al punto en el que colisiona el raycast.
                targetPosition = hit.point;
                // Volvemos a hacer que se mueva el navmesh
                navMesh.isStopped = false;
                // Actualizamos el destino del nav mesh
                navMesh.SetDestination(targetPosition);
                // Decimos que se inicie el movimiento
                isMoving = true;
            }
        }
    }

    /// <summary>
    /// M�todo que har� que el jugador se mueva hacia la posici�n deseada.
    /// </summary>
    private void MoveToTarget() {

        // En el caso de que la distancia que le quede al nav mesh para llegar a su destino sea 
        // menor o igual a la distancia de parada...
        if (navMesh.remainingDistance <= navMesh.stoppingDistance)
        {
            // Deja de moverse
            isMoving = false;
            // Salimos del método
            return;
        }        

        // Calculamos el vector direcci�n para que me diga hacia d�nde se tiene que mover
        Vector3 direction = (navMesh.steeringTarget - transform.position).normalized;
        // Anulamos el valor que tiene en la Y para evitar que el character mire al suelo
        direction.y = 0f;
        // Si la dirección a la que tenemos que ir es diferente de cero, es decir, ya estamos mirando en la dirección correcta...
        if (direction != Vector3.zero)
        {
            // Calculamos la rotación que tiene que tener el objeto en base a la dirección a la que tiene que ir
            Quaternion targetRotation = Quaternion.LookRotation(direction);            
            // Vamos girando de manera gradual el muñeco para que de mejor sensación
            // El Slerp es IGUAL que el Lerp pero para los Quaternions.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Método encargado de inicializar correctamente el nav mesh asignando todas sus variables
    /// </summary>
    private void InitializeAgent() {
        // Actualizamos el valor de la velocidad del navmesh 
        navMesh.speed = speed;
        // Actualizamos el valor de la velocidad de giro del navmesh 
        navMesh.angularSpeed = angularSpeed;
        // Actualizamos el valor de la aceleración del navmesh 
        navMesh.acceleration = acceleration;
        // Actualizamos el valor de la distancia de parada del navmesh 
        navMesh.stoppingDistance = stoppingDistance;
    }

    /// <summary>
    /// M�todo que ir� d�ndole los valores necesarios al animator para su correcto funcionamiento
    /// </summary>
    private void UpdateAnimator() {

        // Actualizamos el valor de "isMoving" del animator para que ejecute las animaciones pertinentes
        animator.SetBool(Constants.ANIM_PLAYER_IS_MOVING, isMoving);
    }

    /// <summary>
    /// Se ejectur� al recibir el evento de moneda recogida
    /// </summary>
    private void CollectCoin() {
        currentCoins++;
        Debug.Log($"El jugador ha cogido tremenda moneda, ahora tiene: {currentCoins}");
    }

    /// <summary>
    /// Método encargado de realizar el disparo siempre que sea posible
    /// </summary>
    private void TryShoot() {

        // Si no puede disparar, salimos del método
        if (!canShoot) return;
        // Paramos el navmesh
        navMesh.isStopped = true;   
        // Decimos que no se mueve
        isMoving = false;     
        // Actualizamos el animator para que haga la animación de disparo
        animator.SetTrigger(Constants.ANIM_PLAYER_SHOOT);
        // Dejamos de poder disparar
        canShoot = false;
        // Generamos el proyectil en el sitio del shootPoint con su rotación
        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        // Si la corrutina ya se está ejecutando
        if (shootCoroutine != null)
        {
            // La paramos
            StopCoroutine(shootCoroutine);
        }
        // Volvemos a empezarla
        shootCoroutine = StartCoroutine(ShootCoroutine());
    }

    /// <summary>
    /// Corrutina de disparo
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootCoroutine() {
        // Esperamos el tiempo que diga la cadencia de disparo
        yield return new WaitForSeconds(shootCadency);
        // Marcamos que se pueda volver a disparar
        canShoot = true;
    }
}