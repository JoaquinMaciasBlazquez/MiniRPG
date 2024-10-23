using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    // Referencia al animator
    [SerializeField] private Animator animator;
    // Referencia al character controller
    [SerializeField] private CharacterController characterController;

    [Header("Configuration")]
    // Velocidad a la que se desplazará el jugador.
    [SerializeField] private float speed;
    // Layer que tendrá asignado el suelo para diferenciarlo.
    [SerializeField] private LayerMask groundLayer;

    // Dirección a la que se tiene que desplazar el jugador.
    private Vector3 targetPosition;
    // Si true, indicará que está ya en movimiento.
    private bool isMoving;
    // Referencia a la cámara.
    private Camera mainCamera;
    // Variable que contrendrá el número de monedas actuales.
    private int currentCoins;

    private void Awake() {

        CheckReferences();
    }

    private void Start() {

        currentCoins = 0;
    }

    private void Update() {

        // Si se está moviendo...
        if (isMoving) {
            MoveToTarget();
        }

        CheckMovement();
        UpdateAnimator();
    }

    private void OnTriggerEnter(Collider other) {

        // Si el collider que hemos recuperado que es trigger tiene la interfaz IPickeable...
        if (other.TryGetComponent(out IPickeable pickeable)) {
            // Ejecutamos la lógica de pick up
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

    /// <summary>
    /// Comprueba las referencias que sean necesarias para la ejecución del script.
    /// </summary>
    private void CheckReferences() {

        // En el caso de que el animator sea nulo; es decir, no esté asignado...
        if (animator == null) {
            // Buscamos el componente animator en los hijos.
            animator = GetComponentInChildren<Animator>();
        }

        // En el caso de que el character controller sea nulo; es decir, no esté asignado...
        if (characterController == null) {
            // Buscamos el componente character controller en los componentens del mismo objeto.
            characterController = GetComponent<CharacterController>();
        }

        mainCamera = Camera.main;
    }

    /// <summary>
    /// Método que lanzará la lógica de movimiento cuando sea necesario.
    /// </summary>
    private void CheckMovement() {

        // Detectamos si el jugador hace click...
        if (Input.GetMouseButtonDown(0)) {

            // Nos creamos un rayo desde la posición en la que se encuentra el ratón en base a la cámara.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Lanzamos el rayo que hemos creado antes con una longitud infinita,
            // solamente entraremos en el if SI Y SOLO SI colisiona con un objeto que tenga el layer de ground
            // declaramos de manera local la variable hit para poder acceder a ella dentro del if.
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer)) {

                // Guardamos el punto al que tenemos que ir en base al punto en el que colisiona el raycast.
                targetPosition = hit.point;
                // Decimos que se inicie el movimiento
                isMoving = true;
            }
        }
    }

    /// <summary>
    /// Método que hará que el jugador se mueva hacia la posición deseada.
    /// </summary>
    private void MoveToTarget() {

        // Calculamos el vector dirección para que me diga hacia dónde se tiene que mover
        Vector3 direction = (targetPosition - transform.position).normalized;
        // Calculamos la distancia a la que se encuentra la posición final de la actual
        float distance = Vector3.Distance(transform.position, targetPosition);

        // De manera local creamos un vector3 que guardará la dirección y será el siguiente forward del personaje
        Vector3 nextForward = direction;
        // Anulamos el valor que tiene el forward en la Y para que el jugador no se oriente al suelo
        nextForward.y = 0f;
        // Le asignamos el forward nuevo
        transform.forward = nextForward;

        // En el caso de que la distancia que nos separa al destino sea menor que 0.1, daremos
        // por hecho que hemos llegado...
        if (distance <= 0.1f) {

            // Marcamos que dejamos de movernos.
            isMoving = false;
            // Salimos del método
            return;
        }

        // Aplicamos el movimiento al character controller. 
        // La cantidad de movimiento es siempre la dirección por la velocidad
        // Añadimos el Time.deltaTime (tiempo en segundos entre frames) para ajustar el movimiento
        // a todos los dispotivos tengan o no los mismos fps
        characterController.Move(direction * speed * Time.deltaTime);
    }

    /// <summary>
    /// Método que irá dándole los valores necesarios al animator para su correcto funcionamiento
    /// </summary>
    private void UpdateAnimator() {

        // Actualizamos el valor de "isMoving" del animator para que ejecute las animaciones pertinentes
        animator.SetBool(Constants.ANIM_PLAYER_IS_MOVING, isMoving);
    }

    /// <summary>
    /// Se ejecturá al recibir el evento de moneda recogida
    /// </summary>
    private void CollectCoin() {
        currentCoins++;
        Debug.Log($"El jugador ha cogido tremenda moneda, ahora tiene: {currentCoins}");
    }
}