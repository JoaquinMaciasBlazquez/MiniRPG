using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    // Imagen que se irá actualizando en la interfaz haciendo referencia a la vida actual del personaje
    [SerializeField] private Image healthImage;

    private float targetFillAmount;

    private void Update() {

        CheckUpdateFillAmount();
    }

    /// <summary>
    /// Método que irá actualizando el fill amount siempre que sea necesario
    /// </summary>
    private void CheckUpdateFillAmount() {

        // En el caso de que el fill amount ya sea el que queremos, salimos del método
        if (healthImage.fillAmount == targetFillAmount) return;

        // Movemos el fill amount de manera progresiva
        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, targetFillAmount, Time.deltaTime / 0.2f);
    }

    /// <summary>
    /// Método que actualizará el fill amount de la imagen de la vida.
    /// </summary>
    /// <param name="newFillAmount"></param>
    public void UpdateTargetFillAmount(float newFillAmount) {
        // Actualizamos el fill amount
        targetFillAmount = newFillAmount;
    }
}