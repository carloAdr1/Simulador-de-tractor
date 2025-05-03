using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    void Start()
    {
        // Carga automáticamente la primera escena del juego (tu menú)
        SceneManager.LoadScene("Menu"); // O cambia a "NombreDeTuMenu"
    }
}
