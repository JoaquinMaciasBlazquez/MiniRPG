using UnityEngine;

public class ItemGenerator : MonoBehaviour {
    // Objetos que puede generar 
    [SerializeField] private GameObject[] itemPrefabs;
    // Radio de dispersión de los objetos generados (solo aparecerán en X y Z)
    [SerializeField] private float spawnRadius = 5f;
    // Offset para evitar que los objetos aparezcan en el centro (dentro del generador)
    [SerializeField] private float minOffset = 1f;
    // Cantidad de objetos a generar
    [SerializeField] private int spawnCount = 5;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        // Dibujamos el radio total
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Gizmos.color = Color.magenta;
        // Dibujamos el radio pequeño de offset
        Gizmos.DrawWireSphere(transform.position, minOffset);
    }

    public void GenerateItems() {
        // Radio efectivo
        float effectiveRadius = Mathf.Max(spawnRadius, minOffset + 0.1f);
        for (int i = 0; i < spawnCount; i++) {
            // Calculamos la posición aleatoria en la que se tiene que generar el objeto
            Vector3 randomPosition = GetRandomPositionOutsideOffset(effectiveRadius);
            // Los generamos
            Instantiate(itemPrefabs[Random.Range(0,itemPrefabs.Length)], randomPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPositionOutsideOffset(float effectiveRadius) {
        // Calculamos un rango mayor al minOffset
        float x = Random.Range(-effectiveRadius, effectiveRadius);
        float z = Random.Range(-effectiveRadius, effectiveRadius);
        // Calculamos una posible posición para la moneda
        // Mientras la posición siga dentro del minOffset...
        while (new Vector2(x,z).magnitude < minOffset) { // Buscando excusas para meteros el while :)
            // Vamos volviendo a generar posibles posiciones
            x = Random.Range(-effectiveRadius, effectiveRadius);
            z = Random.Range(-effectiveRadius, effectiveRadius);
        }
        // Devolvemos la posición final fuera del área de exclusión
        return new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    }
}