using UnityEngine;

public class Chest : MonoBehaviour {

    [SerializeField] private GameObject prompt;

    private bool playerInside;
    // Si true, ya estará abierto
    private bool isOpened;

    private void Start() {
        prompt.SetActive(false);
    }

    private void Update() {
        // Si el jugador está dentro de su radio de acción...
        if (playerInside) {
            // En el caso de que no esté abierto y el jugador presione la tecla Q...
            if (!isOpened && Input.GetKeyDown(KeyCode.Q)) {
                // Abrimos el cofre
                isOpened = true;
            }
        } 
    }

    private void OnTriggerEnter(Collider other) {
        // Si el objeto que entra en su trigger es el jugador...
        if (other.CompareTag("Player")) {
            playerInside = true;
            // Si el cofre no está ya abierto...
            if (!isOpened) {
                // activamos la indicación
                prompt.SetActive(true);
            }
        }        
    }

    private void OnTriggerExit(Collider other) {
        // Si el objeto que sale de su trigger es el jugador...
        if (other.CompareTag("Player")) {
            playerInside = false;
            // Si el prompt está activo...
            if (prompt.activeSelf) {
                // Lo desactivamos
                prompt.SetActive(false);
            }
        }
    }
}