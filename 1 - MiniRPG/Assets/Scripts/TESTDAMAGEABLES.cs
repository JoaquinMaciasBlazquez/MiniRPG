using UnityEngine;

public class TESTDAMAGEABLES : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out PlayerDamageable pDamageable)) {
            pDamageable.TakeDamage(10);
        }
        if (other.TryGetComponent(out EnemyDamageable eDamageable)) {
            eDamageable.TakeDamage(10);
        }
    }
}