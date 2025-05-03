using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class Menu : MonoBehaviour
{

    public AudioSource sfxAlgo;
    public AudioClip sfxboton;
    public GameObject serialManagerPrefab;  // Asignado en el Inspector

    public void PlayGame()
    {
        
        
        if (GameState.instance != null)
        {
            GameState.instance.ReiniciarEstado();
        } 

        if (SerialSender.instance == null && serialManagerPrefab != null)
        {
            GameObject sm = Instantiate(serialManagerPrefab);
            sm.name = "SerialManager"; // Opcional, para mantener orden
        }
        sfxAlgo.PlayOneShot(sfxboton);
        SceneManager.LoadScene("main simulation"); // O usa indice si es necesario
    }

    public void Controls()
{
    StartCoroutine(PlaySoundAndLoad("Controles"));
}

IEnumerator PlaySoundAndLoad(string sceneName)
{
    sfxAlgo.PlayOneShot(sfxboton);
    yield return new WaitForSeconds(sfxboton.length);
    SceneManager.LoadScene(sceneName);
}
}
