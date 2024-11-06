 using UnityEngine;

public class Billboard : MonoBehaviour {

    // Referencia a la c�mara
    private Transform cam;

    private void Awake() {
        // Asignamos la referencia de la c�mara
        cam = Camera.main.transform.parent;
    }
    
    private void Update() {
        // Hacemos que el hud mire en direcci�n contraria al forward de la c�mara
        // Lo ponemos en el start porque la c�mara NUNCA va a girar
        transform.forward = -cam.forward;
    }
}