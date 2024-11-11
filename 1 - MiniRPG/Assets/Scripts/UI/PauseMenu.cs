using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    /// <summary>
    /// Se ejecutará cuando el jugador presione el botón de continuar
    /// </summary>
    public void Continue() {
        // Llamamos al método que alterna la pausa del GameManager
        GameManager.Instance.TogglePause();
    }
    /// <summary>
    /// Se ejecutará cuando el jugador presione el botón de salir al escritorio
    /// </summary>
    public void Desktop() {
        // Cerramos la aplicación
        Application.Quit();
    }
    /// <summary>
    /// Se ejecutará cuando el jugador presione el botón de volver al menú principal
    /// </summary>
    public void Menu() {
        // Cargamos la escena del menú principal
        SceneManager.LoadScene(Constants.SCENE_MAIN_MENU);
    }
}