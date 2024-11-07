using UnityEngine;
using UnityEngine.Playables;

public class PlayerProjectile : MonoBehaviour {

    // Velocidad que llevará la bala
    [SerializeField] private float speed;
    // Daño de la bala
    [SerializeField] private float damage;
    // Tiempo de vida de la bala
    [SerializeField] private float lifeTime;
    // Layer que tendrá que ignorar el layer
    [SerializeField] private LayerMask ignoreLayer;
    // Referencia al audioSource de la bala que irá ejecutando sus sonidos
    [SerializeField] private AudioSource audioSource;
    // Clip de sonido que ejecutará cuando la bala se dispare
    [SerializeField] private AudioClip shootClip;
    // Clip de sonido que se ejecutará cuando la bala impacte
    [SerializeField] private AudioClip impactClip;
    // Referencia al mesh renderer
    [SerializeField] private MeshRenderer meshRenderer;
    // Referencia al collider
    [SerializeField] private Collider bulletCollider;

    private void Awake() {
        // En el caso de que no hayamos asignado el audioSource a mano, adquiriremos la referencia desde código
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        // Ejecutamos el sonido de disparo
        PlayAudioSource(shootClip, false);
        // En el caso de que no tengamos asignado el renderer, cogemos su referencia de manera directa
        if (meshRenderer == null) meshRenderer = GetComponentInChildren<MeshRenderer>();
        // En el caso de que no tengamos asignado el collider, cogemos su referencia de manera directa
        if (bulletCollider == null) bulletCollider = GetComponent<Collider>();
        ToggleProjectile(true);
    }

    private void Start() {
        // Destruimos la bala en el tiempo que digamos en el lifeTime, en caso de que no colisione en todo ese tiempo
        Destroy(gameObject, lifeTime);
    }

    private void Update() {
        // Hacemos un movimiento kinemático; es decir, movemos directamente el objeto manipulando su transform
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {

        // En el caso de que el layer que tenga el objeto sea igual al layer que tenemos que ignorar, salimos del método
        if ((ignoreLayer & (1 << other.gameObject.layer)) != 0) return;

        // Si lo que colisiono tiene como componente un EnemyDamageable, lo guardo en una variable local
        if (other.TryGetComponent(out EnemyDamageable damageable)) {
            // Le hacemos daño al damageable
            damageable.TakeDamage(damage);
        }
        // Ejecitamos el audio de impacto
        PlayAudioSource(impactClip, false);
        // Desactivamos sus componentes visuales y su colisión
        ToggleProjectile(false);
        // En el momento que impacte destruimos la bala
        Destroy(gameObject, 0.25f);
    }

    /// <summary>
    /// Método que ejecuta el sonido dicho en el clip
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="isLoop"></param>
    private void PlayAudioSource(AudioClip clip, bool isLoop) {
        // Asignamos el clip que se va a reproducir
        audioSource.clip = clip;
        // Nos aseguramos de que no esté en loop para que no se reproduzca siempre el clip
        audioSource.loop = isLoop;
        // Ejecutamos el clip
        audioSource.Play();
    }

    /// <summary>
    /// Activa o desactiva lo visual y su colisión
    /// </summary>
    /// <param name="enabled"></param>
    private void ToggleProjectile(bool enabled) {
        // Activamos o desactivamos el renderer en base a lo que digamos por parámetro
        meshRenderer.enabled = enabled;
        // Activamos o desactivamos el collider en base a lo que digamos por parámetro
        bulletCollider.enabled = enabled;
    }
}