using UnityEngine;

public class TESTSHAKE : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out Shake shake)) {
            shake.Activate();
        }       
    }
}