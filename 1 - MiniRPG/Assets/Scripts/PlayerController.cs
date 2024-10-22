using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    // Referencia al animator
    [SerializeField] private Animator animator;
    // Referencia al character controller
    [SerializeField] private CharacterController characterController;

    [Header("Configuration")]
    // Velocidad a la que se desplazar� el jugador.
    [SerializeField] private float speed;
    // Layer que tendr� asignado el suelo para diferenciarlo.
    [SerializeField] private LayerMask groundLayer;

    // Direcci�n a la que se tiene que desplazar el jugador.
    private Vector3 targetPosition;
    // Si true, indicar� que est� ya en movimiento.
    private bool isMoving;
    // Referencia a la c�mara.
    private Camera mainCamera;

    private void Awake() {

        CheckReferences();
    }

    private void Update() {

        // Si se est� moviendo...
        if (isMoving) {
            MoveToTarget();
        }

        CheckMovement();
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

        // En el caso de que el character controller sea nulo; es decir, no est� asignado...
        if (characterController == null) {
            // Buscamos el componente character controller en los componentens del mismo objeto.
            characterController = GetComponent<CharacterController>();
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
                // Decimos que se inicie el movimiento
                isMoving = true;
            }
        }
    }

    /// <summary>
    /// M�todo que har� que el jugador se mueva hacia la posici�n deseada.
    /// </summary>
    private void MoveToTarget() {

        // Calculamos el vector direcci�n para que me diga hacia d�nde se tiene que mover
        Vector3 direction = (targetPosition - transform.position).normalized;
        // Calculamos la distancia a la que se encuentra la posici�n final de la actual
        float distance = Vector3.Distance(transform.position, targetPosition);

        // En el caso de que la distancia que nos separa al destino sea menor que 0.1, daremos
        // por hecho que hemos llegado...
        if (distance <= 0.1f) {

            // Marcamos que dejamos de movernos.
            isMoving = false;
            // Salimos del m�todo
            return;
        }

        // Aplicamos el movimiento al character controller. 
        // La cantidad de movimiento es siempre la direcci�n por la velocidad
        // A�adimos el Time.deltaTime (tiempo en segundos entre frames) para ajustar el movimiento
        // a todos los dispotivos tengan o no los mismos fps
        characterController.Move(direction * speed * Time.deltaTime);
    }
}