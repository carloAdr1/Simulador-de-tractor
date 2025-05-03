using UnityEngine;

public class EstadoJugadorRestaurador : MonoBehaviour
{
    void Start()
{
    Debug.Log("🧠 Restaurador ejecutado");

    if (GameState.instance == null) return;

    // Restaurar posición
    transform.position = GameState.instance.posicionJugador;

    // Gasolina
    Gasolina gas = GetComponent<Gasolina>();
    if (gas != null)
    {
        gas.gasolina = (GameState.instance.gasolina > 0) ? GameState.instance.gasolina : 100f;
        Debug.Log("⛽ Gasolina restaurada: " + gas.gasolina);
    }

    // Vida
    vida vidaScript = GetComponent<vida>();
    if (vidaScript != null)
    {
        vidaScript.salud = GameState.instance.vida;
        Debug.Log("❤️ Salud restaurada: " + vidaScript.salud);
    }

    // Puntos
    Puntos puntosScript = FindObjectOfType<Puntos>();
    if (puntosScript != null)
    {
        puntosScript.puntos = GameState.instance.zanahorias;
        Debug.Log("🥕 Puntos restaurados: " + puntosScript.puntos);
    }

    // Zanahorias (estado de recolección)
    if (ControladorCultivo.instance != null)
    {
        ControladorCultivo.instance.RestaurarEstado();
        Debug.Log("🌱 Estado de zanahorias restaurado");
    }
}
}