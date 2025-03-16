using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(0); // Carga la escena 0 al reintentar
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(2); // Carga la escena del menú (2)
    }
}