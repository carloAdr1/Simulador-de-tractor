using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteraccionMercado : MonoBehaviour
{
    public CanvasGroup letraEUI;
    public GameObject jugador; // <- Referencia al jugador
    private bool jugadorDentro = false;
    private bool yaEntramos = false;

    void Start()
    {
        if (letraEUI != null)
            letraEUI.alpha = 0;

        if (EscenaAnterior.instance != null && EscenaAnterior.instance.indiceEscenaAnterior == -1)
        {
            EscenaAnterior.instance.indiceEscenaAnterior = SceneManager.GetActiveScene().buildIndex;
            Debug.Log("🟢 EscenaAnterior auto-guardada en Start: " + SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Update()
    {
        if (letraEUI == null || yaEntramos) return;

        if (jugadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            if (EscenaAnterior.instance != null)
            {
                EscenaAnterior.instance.indiceEscenaAnterior = SceneManager.GetActiveScene().buildIndex;
                Debug.Log("🔁 Escena actual guardada: " + SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Debug.LogWarning("⚠️ EscenaAnterior.instance es null");
            }

            if (GameState.instance != null)
            {
                GameState.instance.posicionJugador = jugador.transform.position;

                Gasolina gasolinaScript = jugador.GetComponent<Gasolina>();
                if (gasolinaScript != null)
                    GameState.instance.gasolina = Mathf.RoundToInt(gasolinaScript.gasolina);

                vida vidaScript = jugador.GetComponent<vida>();
                if (vidaScript != null)
                    GameState.instance.vida = Mathf.RoundToInt(vidaScript.salud);

                Puntos puntosScript = FindObjectOfType<Puntos>();
                if (puntosScript != null)
                    GameState.instance.zanahorias = puntosScript.puntos;

                Debug.Log("💾 Estado guardado en GameState");
            }

            // ✅ NUEVO bloque seguro para zanahorias
            if (ControladorCultivo.instance != null)
            {
                try
                {
                    Debug.Log("✅ Se encontró ControladorCultivo, intentando guardar zanahorias...");
                    ControladorCultivo.instance.GuardarEstado();
                    Debug.Log("🌱 Estado de zanahorias guardado con éxito.");
                }
                catch (System.Exception e)
                {
                    Debug.LogError("❌ Error al guardar zanahorias: " + e.Message);
                }
            }
            else
            {
                Debug.LogWarning("❌ ControladorCultivo.instance es null");
            }

            yaEntramos = true;
            Debug.Log("➡️ Cargando escena Mercado...");
            SceneManager.LoadScene(6); // Mercado
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            if (letraEUI != null)
                letraEUI.alpha = 1;

            Debug.Log("👟 Jugador entró al trigger de la casa.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            if (letraEUI != null)
                letraEUI.alpha = 0;

            Debug.Log("🚪 Jugador salió del trigger de la casa.");
        }
    }
}
