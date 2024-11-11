using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance {
        get {
         return instance;
        }
    }

    [SerializeField] private GameObject pauseCanvas;

    // True si juego está ya pausado
    private bool gamePaused;

    /// <summary>
    /// Devolverá si el juego está o no en pausa
    /// </summary>
    public bool GamePaused {
        get {
            // devuelve el valor de la variable gamePaused
            // lo hacemos así para que sea solo de lectura
           return gamePaused;
        }
    }

    private void Awake() {
        // Declaración de un singleton
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        // Nos aseguramos que no esté en pause
        gamePaused = false;
        // Que el tiempo esté activo
        Time.timeScale = 1f;
        // El canvas desactivado
        pauseCanvas.SetActive(false);
        MusicManager.Instance.PlayGame();
        MusicManager.Instance.PitchRegular();
    }

    private void Update() {
        // Si detectamos que se ha presionado la tecla de escape...
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    /// <summary>
    /// Método que se ejecutará cuando detecte algún input de pausa
    /// </summary>
    public void TogglePause() {
        // Marcamos que el juego tiene el estado contrario al actual; es decir:
        // si gamePaused es true, pasará a ser false y viceversa.
        gamePaused = !gamePaused;
        // Modificamos el time scale para hacer que se pare el juego o se retome en base a si está o no pausado
        // timeScale a 1 - juego a velocidad normal
        // timeScale a 0 - juego parado
        // timeScale a 0.5f - juego a mitad de velocidad
        // timeScale a 2f - juego con el doble de velocidad
        Time.timeScale = gamePaused ? 0f : 1f;
        // Activamos el canvas o no según el valor que tenga game paused
        pauseCanvas.SetActive(gamePaused);
    }
}