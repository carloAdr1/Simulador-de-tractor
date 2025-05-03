using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState instance;

    // Estado del jugador
    public Vector3 posicionJugador = Vector3.zero;
    public int gasolina = 0;
    public int vida = 100;
    public int zanahorias = 0;

    public void ReiniciarEstado()
{
    posicionJugador = Vector3.zero;
    gasolina = 100;
    vida = 100;
    zanahorias = 0;
}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ✅ se mantiene entre escenas
            Debug.Log("🟢 GameState inicializado");
        }
        else
        {
            Destroy(gameObject); // ✅ previene duplicados
        }
    }
}
