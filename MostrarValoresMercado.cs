using UnityEngine;
using UnityEngine.UI;

public class MostrarValoresMercado : MonoBehaviour
{
    public Text vidaTexto;
    public Text gasolinaTexto;
    public Text puntosTexto;

    void Update()
    {
        if (GameManager.Instance != null)
        {
            vidaTexto.text = "Vida: " + GameManager.Instance.vidaActual.ToString();
            gasolinaTexto.text = "Gasolina: " + GameManager.Instance.gasolinaActual.ToString();
            puntosTexto.text = "Puntos: " + GameManager.Instance.puntos.ToString();
        }
        else
        {
            Debug.LogWarning("⚠️ GameManager no existe.");
            Debug.Log("VIDA = " + GameManager.Instance.vidaActual);
        }
    }
}
