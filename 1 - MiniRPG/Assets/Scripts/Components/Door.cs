using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject prompt;
    [SerializeField] private GameObject needKeyCanvas;
    [SerializeField] private NavMeshObstacle obstacle;
    private bool playerInside;
    private bool isOpened;

    private void Start() {
        // El jugador no está dentro
        playerInside = false;
        // Nos aseguramos de que el prompt está desactivado
        prompt.SetActive(false);
        // Nos aseguramos de que el indicador está desactivado
        needKeyCanvas.SetActive(false);
    }

    private void Update() {
        // Si el jugador está dentro...
        if (playerInside) {
            // Si el jugador presiona la Q...
            if (Input.GetKeyDown(KeyCode.Q)) {
                // Si el jugador tiene la llave...
                if (GameManager.Instance.HasKey) {
                    // Si no está abierta ya la puerta...
                    if (!isOpened) {
                        // La abrimos
                        isOpened = true;
                        // Ejecutamos su animación
                        animator.SetTrigger(Constants.ANIM_GATE_OPEN);
                        // Desactivamos el prompt
                        prompt.SetActive(false);
                        // Desactivamos el obstáculo de colisiones de la puerta
                        obstacle.enabled = false;
                    }
                    // Si no tenemos la llave
                } else {
                    // Activamos el indicador de que necesitamos la llave
                    needKeyCanvas.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        // Si el jugador colisiona con su caja de trigger...
        if (other.CompareTag("Player")){
            // decimos que puede interactuar con ella
            playerInside = true;
            if(!isOpened){
                prompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        // Si el jugador sale de su caja de trigger...
        if (other.CompareTag("Player")){
            // decimos que puede no interactuar con ella
            playerInside = false;
            // Si el prompt está activo, lo desactivamos
            if (prompt.activeSelf){
                prompt.SetActive(false);
            }
            // Si el indicador de que necesita llave está activo, lo desactivamos
            if (needKeyCanvas.activeSelf){
                needKeyCanvas.SetActive(false);
            }
        }
    }
}