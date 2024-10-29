using UnityEngine;

public class CameraRig : MonoBehaviour {

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotation;

    private void OnValidate() {
        // Se ejecuta cada vez que nota un cambio en el inspector del objeto
        transform.position = target.position + offset;
    }

    private void LateUpdate() {
        // Ejecutamos el movimiento en el late update para evitar los jitters de cámara
        Move();
    }

    private void Move() {

        // En el caso de que el target sea nulo, es decir, no tengamos la referencia del target asignado...
        if (target == null) {
            // Lanzamos un mensaje por consola diciendo el error
            Debug.LogWarning($"No se ha asignado target en {name}");
            // Salimos del método para evitar referencias nulas
            return;
        }

        // Establecemos la rotación de la cámara
        transform.rotation = Quaternion.Euler(rotation);

        // Calculamos la posición deseada de la cámara
        Vector3 desiredPosition = target.position + offset;

        // Suavizamos el movimiento de la cámara usando un Lerp
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime / 0.2f);
    }
}