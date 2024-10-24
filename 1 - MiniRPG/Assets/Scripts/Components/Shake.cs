using UnityEngine;

public class Shake : MonoBehaviour {

    // Intensidad del shake
    [SerializeField, Range(0f, 0.5f)] private float intensity = 0.04f;
    // Velocidad del shake
    [SerializeField] private float speed = 80f;
    // Duración del shake
    [SerializeField] private float duration = 0.2f;
    // Objeto que realmente se va a mover
    [SerializeField] private Transform objectToShake;

    // True si el shake está activo
    private bool shakeActive;
    // Contador del tiempo
    private float timeCounter;

    private void Start() {
        // Iniciamos el contador con la duración del tiempo
        timeCounter = duration;
    }

    private void Update() {

        // En el caso de que no esté activo el shake, no hacemos nada; es decir, salimos del update
        if (!shakeActive) return;

        // De manera local, le aplicamos el seno con la velocidad en base al tiempo que llevemos en ese frame
        objectToShake.localPosition = Vector3.up * intensity * Mathf.Sin(Time.time * speed);
        // Vamos descontando el contador
        timeCounter -= Time.deltaTime;

        // En el caso de que el contador haya llegado a cero o sea menor...
        if (timeCounter <= 0f) {
            // Volvemos a la posición origen el objeto que estamos moviendo para el shake
            objectToShake.localPosition = Vector3.zero;
            // Volvemos a iniciar el contador con la duración
            timeCounter = duration;
            // Desactivamos el shake
            shakeActive = false;
        }
    }

    /// <summary>
    /// Método que activa el efecto de shake.
    /// </summary>
    [ContextMenu("Activate shake")]
    public void Activate() {
        // Decimos que el shake está activo
        shakeActive = true;
    }
}