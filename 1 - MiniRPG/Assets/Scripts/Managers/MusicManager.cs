using System.Collections;
using UnityEngine;

// Con esto obligamos a que el objeto que contenga el MusicManager, tenga un AudioSource
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip menuClip;
    [SerializeField] private AudioClip gameClip;
    // Tiempo que empleará en hacer el cambio de una música a otra
    [SerializeField, Range(1,3)] private float fadeTime;
    // Tiempo que tarde en ir del pitch actual al deseado
    [SerializeField, Range(0,2)] private float pitchTime = 1f;
    // Pitch cuando esté ralentizado
    [SerializeField] private float pitchSlow = 0.6f; 

    // Corrutina que controlará el fade in y fade out de la música
    private Coroutine fadeCoroutine;
    // Corrutina que controlará el cambio de pitch
    private Coroutine pitchCoroutine;

    private static MusicManager instance;
    public static MusicManager Instance => instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Lanzará la música del menú
    /// </summary>
    public void PlayMainMenu() {
        PlayAudioClip(menuClip);
    }

    /// <summary>
    /// Lanzará la música de juego
    /// </summary>
    public void PlayGame() {
        PlayAudioClip(gameClip);
    }

    /// <summary>
    /// Lanzará un nuevo audio clip
    /// </summary>
    /// <param name="clip"></param>
    private void PlayAudioClip(AudioClip clip) {
        if (audioSource.clip == clip) return;
        // En el caso de que tengamos un fade realizándose...
        if (fadeCoroutine != null) {
            // Lo paramos
            StopCoroutine(fadeCoroutine);
        }
        // Ejecutamos un nuevo fade
        fadeCoroutine = StartCoroutine(StartFadeAndChangeClip(clip));
    }

    [ContextMenu("SlowMo")]
    public void PitchSlow() {
        if (pitchCoroutine != null) StopCoroutine(pitchCoroutine);
        pitchCoroutine = StartCoroutine(StartSlowMo(true));
    }

    [ContextMenu("RegularMo")]
    public void PitchRegular() {
        if (pitchCoroutine != null) StopCoroutine(pitchCoroutine);
        pitchCoroutine = StartCoroutine(StartSlowMo(false));
    }

    /// <summary>
    /// Realiza el cambio de clip suavizando el sonido para que no quede brusco
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    private IEnumerator StartFadeAndChangeClip(AudioClip clip) {
        // Inicializamos el counter en la mitad del tiempo del fade; ya que,
        // la primera mitad será de fade out y la otra de fade in
        float counter = fadeTime / 2;
        while (counter > 0) {
            // Le asignamos la normalización del valor de counter entre lo que queda de fade
            audioSource.volume = counter / (fadeTime / 2);
            // Restamos el counter
            counter -= Time.deltaTime;
            yield return null;
        }
        // Modificamos el clip para asignarle el siguiente clip
        audioSource.clip = clip;
        // Hacemos que se vuelva a ejecutar el audioSource
        audioSource.Play();
        // Hacemos el fade in
        while (counter < fadeTime / 2) {
            // Le vamos asignando el valor que vaya correspondiendo al volumen del audioSource
            audioSource.volume = counter / (fadeTime / 2);
            // Vamos aumentando el contador de tiempo
            counter += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Incrementa o resetea el pitch
    /// </summary>
    /// <param name="slow"></param>
    /// <returns></returns>
    private IEnumerator StartSlowMo(bool slow) {
        // Obtenemos el pitch al que queremos ir dependiendo de la variable slow
        float target = slow ? pitchSlow : 1f;
        // Valor actual de pitch antes de empezar a modificarlo
        float current = audioSource.pitch;
        // Contador para iterar
        float counter = 0f;
        while (counter < pitchTime) {
            // Vamos lerpeando el valor del pitch desde el actual hasta el target de manera normalizada en el tiempo total
            audioSource.pitch = Mathf.Lerp(current, target, counter / pitchTime);
            // Vamos decrementando el contador
            counter += Time.deltaTime;
            yield return null;
        }
    }
}