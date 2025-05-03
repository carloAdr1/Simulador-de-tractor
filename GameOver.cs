using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public AudioSource sfxAlgo;
    public AudioClip sfxboton;
    public GameObject serialManagerPrefab; // ← Asignar en Inspector

    public void RestartGame()
{
    // 🔌 Cierra puerto UART si está abierto
    if (SerialSender.instance != null)
        SerialSender.instance.CerrarPuerto();

    // 🧠 Reinicia estado
    if (GameState.instance != null)
        GameState.instance.ReiniciarEstado();

    // 🔁 Reinicia valores del GameManager
    if (GameManager.Instance != null)
    {
        GameManager.Instance.vidaActual = 100;
        GameManager.Instance.gasolinaActual = 100;
        GameManager.Instance.puntos = 0;
    }
    
    // Recarga la escena del juego
    SceneManager.LoadScene(2); // ID de la escena del juego
}

    public void GoToMenu()
    {
        StartCoroutine(PlaySoundAndLoad(1)); // Llama a corrutina con ID del menú
    }

    private IEnumerator PlaySoundAndLoad(int sceneIndex)
    {
        if (SerialSender.instance != null)
            SerialSender.instance.CerrarPuerto();

        sfxAlgo.PlayOneShot(sfxboton);
        yield return new WaitForSeconds(sfxboton.length);
        SceneManager.LoadScene(sceneIndex);
    }
}