using System;
using UnityEngine;

public class Coin : MonoBehaviour, IPickeable {

    // Referencia al sistema de partículas de la moneda
    [SerializeField] private ParticleSystem pickUpParticles;
    // Referencia al audio source que lanzará el sonido cuando la monida sea obtenida
    [SerializeField] private AudioSource audioSource;

    public static Action OnCoinCollected;

    private void Awake() {

        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PickUp() {
        // Lanzamos el evento (mensaje) de que una moneda ha sido recogida
        // El '?' es para que se lance siempre que tenga suscriptoresº
        OnCoinCollected?.Invoke();
        // Reproducimos el sonido
        audioSource.Play();
        // Generamos las partículas en el sitio que esté la moneda y con la rotación base que tuviera el sistema de partículas
        Instantiate(pickUpParticles, transform.position, Quaternion.identity);
        // Destruimos el gameObject; es decir, el objeto que tuviera el script, en este caso la moneda
        Destroy(gameObject, 0.2f);
    }
}