using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverAJugar : MonoBehaviour
{
    public AudioSource sfxAlgo;
    public AudioClip sfxBoton;

    public void RegresarASimulacion()
{
    Debug.Log("Intentando regresar a la escena anterior...");

    if (EscenaAnterior.instance != null)
    {
        int indice = EscenaAnterior.instance.indiceEscenaAnterior;
        Debug.Log("Escena a cargar: " + indice);
        StartCoroutine(PlaySoundAndLoad(indice));
    }
    else
    {
        Debug.LogWarning("EscenaAnterior no está inicializado.");
    }

    // 💡 Llama al reinicio si el SerialManager ya existe
    if (SerialSender.instance != null)
    {
        UIController controller = FindObjectOfType<UIController>();
        if (controller != null)
        {
            controller.Reiniciar();
        }
    }
}

    private System.Collections.IEnumerator PlaySoundAndLoad(int sceneIndex)
    {
        sfxAlgo.PlayOneShot(sfxBoton);
        yield return new WaitForSeconds(sfxBoton.length);
        SceneManager.LoadScene(sceneIndex);
    }
}
