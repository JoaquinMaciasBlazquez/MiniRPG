using UnityEngine;

public class TESTPLAYERDAMAGEABLE : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out PlayerDamageable damageable)) {
            damageable.TakeDamage(10);
        }
    }
}
