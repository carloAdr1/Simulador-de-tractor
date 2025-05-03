using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 👑 Instancia única
    public static GameManager Instance;

    // Datos globales que quieres compartir
    public int vidaActual = 100;
    public int gasolinaActual = 100;
    public int puntos = 0;

    void Awake()
    {
        // Solo permite una instancia del GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ⛓️ Persiste al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // 🔥 Evita duplicados si ya hay uno en escena
        }
    }
}