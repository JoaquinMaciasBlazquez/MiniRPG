using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup howToPlayCanvasGroup;

    private void Start() {
        Time.timeScale = 1f;
        ToggleCanvasGroup(mainMenuCanvasGroup, true);
        ToggleCanvasGroup(howToPlayCanvasGroup, false);
        MusicManager.Instance.PlayMainMenu();
        MusicManager.Instance.PitchRegular();
    }

    public void StartButton() {
        // Va a la escena del juego
        SceneManager.LoadScene(Constants.SCENE_GAME);
    }

    /// <summary>
    /// M�todo que se asignar� al bot�n para ir al men� de c�mo se juega
    /// </summary>
    public void HowToPlayButton() {
        ToggleCanvasGroup(howToPlayCanvasGroup, true);
        ToggleCanvasGroup(mainMenuCanvasGroup, false);
    }

    /// <summary>
    /// M�todo que se asignar� al bot�n para volver al men� principal
    /// </summary>
    public void ReturnMenuButton() {
        ToggleCanvasGroup(mainMenuCanvasGroup, true);
        ToggleCanvasGroup(howToPlayCanvasGroup, false);
    }

    public void ExitButton() {

#if UNITY_EDITOR // Si el script se ejecuta en el editor de Unity
        // Paramos la ejecuci�n del editor de unity
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE // Si el script se est� ejecutando en la app (en la build)
        // Cerramos la aplicaci�n
        Application.Quit();
#endif
    }

    /// <summary>
    /// Muestra o esconde el canvas que pasemos por par�metro en base al segundo par�metro.
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="enable"></param>
    private void ToggleCanvasGroup(CanvasGroup canvas, bool enable) {

        // Ponemos el alpha del canvas a uno si enable es verdadero o a 0 para que no se vea si es falso
        canvas.alpha = enable ? 1f : 0f;
        // Bloquea los raycasts solo en caso de que est� activo
        canvas.blocksRaycasts = enable;
        // Es interactable solo en caso de que est� activo
        canvas.interactable = enable;
    }
}