using UnityEngine;

public class DataManager : MonoBehaviour {
    // Variable que hará referencia  a las monedas que tenga guardadas el jugador
    public int currentCoins;

    // Declaración de un singleton
     private static DataManager instance;
     public static DataManager Instance {
        get {
            return instance;
        }
     }
     private void Awake() {
        // Si mi instancia es nula; es decir, no tenemos ya un Singleton en la escena...
        if (!instance) {
            // Convertimos esta clase como singleton
            instance = this;
            // En cualquier otro caso; es decir, cuando ya tengamos un singleton...
        }else {
            // Destruimos este objeto
            Destroy(gameObject);
        }
        LoadData();
     }

    /// <summary>
    /// Método que ejecutará todo el guardado de datos
    /// </summary>
     public void SaveData() {
        // Guardamos la variable en la key que tiene la constante de current coins con el valor de la variable currentCoins
        PlayerPrefs.SetInt(Constants.KEY_CURRENT_COINS, currentCoins);
     }

    /// <summary>
    /// Método que ejecutará la carga de datos
    /// </summary>
    [ContextMenu("Load data")]
     public void LoadData() {
        // Si existe la key de 
        if (PlayerPrefs.HasKey(Constants.KEY_CURRENT_COINS)) {
            currentCoins = PlayerPrefs.GetInt(Constants.KEY_CURRENT_COINS);
        }
     }
}