using UnityEngine;

public class Billboard : MonoBehaviour {

    // Referencia a la cámara
    private Transform cam;

    private void Awake() {
        // Asignamos la referencia de la cámara
        cam = Camera.main.transform.parent;
    }

    private void Start() {
        // Hacemos que el hud mire en dirección contraria al forward de la cámara
        // Lo ponemos en el start porque la cámara NUNCA va a girar
        transform.forward = -cam.forward;
    }
}