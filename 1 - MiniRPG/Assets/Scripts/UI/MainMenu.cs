using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup howToPlayCanvasGroup;

    private void Start() {
        ToggleCanvasGroup(mainMenuCanvasGroup, true);
        ToggleCanvasGroup(howToPlayCanvasGroup, false);
    }

    public void StartButton() {
        // Va a la escena del juego
        SceneManager.LoadScene(Constants.SCENE_GAME);
    }

    /// <summary>
    /// Método que se asignará al botón para ir al menú de cómo se juega
    /// </summary>
    public void HowToPlayButton() {
        ToggleCanvasGroup(howToPlayCanvasGroup, true);
        ToggleCanvasGroup(mainMenuCanvasGroup, false);
    }

    /// <summary>
    /// Método que se asignará al botón para volver al menú principal
    /// </summary>
    public void ReturnMenuButton() {
        ToggleCanvasGroup(mainMenuCanvasGroup, true);
        ToggleCanvasGroup(howToPlayCanvasGroup, false);
    }

    public void ExitButton() {

#if UNITY_EDITOR // Si el script se ejecuta en el editor de Unity
        // Paramos la ejecución del editor de unity
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE // Si el script se está ejecutando en la app (en la build)
        // Cerramos la aplicación
        Application.Quit();
#endif
    }

    /// <summary>
    /// Muestra o esconde el canvas que pasemos por parámetro en base al segundo parámetro.
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="enable"></param>
    private void ToggleCanvasGroup(CanvasGroup canvas, bool enable) {

        // Ponemos el alpha del canvas a uno si enable es verdadero o a 0 para que no se vea si es falso
        canvas.alpha = enable ? 1f : 0f;
        // Bloquea los raycasts solo en caso de que esté activo
        canvas.blocksRaycasts = enable;
        // Es interactable solo en caso de que esté activo
        canvas.interactable = enable;
    }
}