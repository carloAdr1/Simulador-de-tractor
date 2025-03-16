using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(0); // Carga la escena del juego (0)
    }

    public void Controls()
    {
        SceneManager.LoadScene(4); // Carga la escena del juego (0)
    }
}
