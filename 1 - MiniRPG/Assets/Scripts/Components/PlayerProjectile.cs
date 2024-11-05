using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    // Velocidad que llevará la bala
    [SerializeField] private float speed;
    // Daño de la bala
    [SerializeField] private float damage;
    // Tiempo de vida de la bala
    [SerializeField] private float lifeTime;
    // Layer que tendrá que ignorar el layer
    [SerializeField] private LayerMask ignoreLayer;

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
        // En el momento que impacte destruimos la bala
        Destroy(gameObject);
    }
}