using UnityEngine;

public class RestauradorDeZanahorias : MonoBehaviour
{
    void Start()
    {
        if (ControladorCultivo.instance != null)
        {
            // ⚠️ Volver a escanear zanahorias en escena
            ControladorCultivo.instance.zanahoriasEnEscena = GameObject.FindGameObjectsWithTag("Zanahoria");

            ControladorCultivo.instance.RestaurarEstado();
            Debug.Log("🌱 Zanahorias restauradas desde RestauradorDeZanahorias");
        }
        else
        {
            Debug.LogWarning("❌ No se encontró ControladorCultivo para restaurar zanahorias");
        }
    }
}

