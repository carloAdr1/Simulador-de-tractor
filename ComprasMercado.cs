using UnityEngine;
using UnityEngine.UI;

public class ComprasMercado : MonoBehaviour
{
    public Text vidaTexto;
    public Text gasolinaTexto;
    public Text puntosTexto;

    public AudioSource sfxAlgo;
    public AudioClip sfxCompra;

    public void ComprarVida()
    {
        if (GameManager.Instance.puntos >= 5)
        {
            GameManager.Instance.puntos -= 5;
            GameManager.Instance.vidaActual = 100;
            sfxAlgo.PlayOneShot(sfxCompra); // ✅ Sonido
            ActualizarTexto();
        }
    }

    public void ComprarGasolina()
    {
        if (GameManager.Instance.puntos >= 5)
        {
            GameManager.Instance.puntos -= 5;
            GameManager.Instance.gasolinaActual = 100;
            sfxAlgo.PlayOneShot(sfxCompra); // ✅ Sonido
            ActualizarTexto();
        }
    }

    public void ComprarCultivo()
    {
        if (GameManager.Instance.puntos >= 10)
        {
            GameManager.Instance.puntos -= 10;
            ControladorCultivo.instance.Replantar();
            sfxAlgo.PlayOneShot(sfxCompra); // ✅ Sonido
            // Puedes añadir ActualizarTexto() si lo deseas
        }
    }

    void ActualizarTexto()
    {
        vidaTexto.text = "Vida: " + GameManager.Instance.vidaActual;
        gasolinaTexto.text = "Gasolina: " + GameManager.Instance.gasolinaActual;
        puntosTexto.text = "Puntos: " + GameManager.Instance.puntos;
    }
}



