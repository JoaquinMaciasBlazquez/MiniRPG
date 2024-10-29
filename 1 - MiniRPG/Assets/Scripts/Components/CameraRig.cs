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
        // Ejecutamos el movimiento en el late update para evitar los jitters de c�mara
        Move();
    }

    private void Move() {

        // En el caso de que el target sea nulo, es decir, no tengamos la referencia del target asignado...
        if (target == null) {
            // Lanzamos un mensaje por consola diciendo el error
            Debug.LogWarning($"No se ha asignado target en {name}");
            // Salimos del m�todo para evitar referencias nulas
            return;
        }

        // Establecemos la rotaci�n de la c�mara
        transform.rotation = Quaternion.Euler(rotation);

        // Calculamos la posici�n deseada de la c�mara
        Vector3 desiredPosition = target.position + offset;

        // Suavizamos el movimiento de la c�mara usando un Lerp
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime / 0.2f);
    }
}