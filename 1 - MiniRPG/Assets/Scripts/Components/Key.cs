using UnityEngine;

public class Key : MonoBehaviour {

    private void Start() {
        // Si ya tenemos la llave, la eliminamos de la escena para que no vuelva a aparecer
        if (DataManager.Instance.hasKey) Destroy(gameObject); 
    }

    private void OnTriggerEnter(Collider other) {
        // En el caso de que el objeto que colisiona con la llave sea el jugador...
        if (other.CompareTag("Player")) {
            // Adquirimos la llave
            GameManager.Instance.CollectKey();
            // Destruimos el objeto
            Destroy(gameObject);
        }
    }   
}