using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Puntos : MonoBehaviour
{
    public Text puntosTexto;
    public int puntos = 0;
    public int puntosParaGanar = 20;
    private bool ledEnviado = false; // Para evitar envío repetido

    void Start()
{
    if (GameManager.Instance != null)
    {
        puntos = GameManager.Instance.puntos;
    }
    else if (GameState.instance != null)
    {
        puntos = GameState.instance.zanahorias;
    }

    GameManager.Instance.puntos = puntos;
    ActualizarTexto();

    if (SerialSender.instance != null)
    {
        SerialSender.instance.EnviarValor(0xAD); // Apagar LED verde al iniciar
        ledEnviado = false;
    }
}
    public void SumarPunto(int cantidad)
    {
        puntos += cantidad;
        GameManager.Instance.puntos = puntos;
        ActualizarTexto();

        // Encender o apagar LED según puntos
        if (SerialSender.instance != null)
        {
            if (puntos >= puntosParaGanar && !ledEnviado)
            {
                SerialSender.instance.EnviarValor(0xAC); // Encender LED (puntos ganados)
                ledEnviado = true;
            }
            else if (puntos < puntosParaGanar && ledEnviado)
            {
                SerialSender.instance.EnviarValor(0xAD); // Apagar LED
                ledEnviado = false;
            }

            SerialSender.instance.EnviarValor(puntos); // Mantiene tu funcionalidad original
        }

        if (puntos >= puntosParaGanar)
        {
            GanarJuego();
        }
    }

    void ActualizarTexto()
    {
        if (puntosTexto != null)
        {
            puntosTexto.text = puntos.ToString();
        }
    }

    void GanarJuego()
    {
        SceneManager.LoadScene(3); // Ganaste
        }
}
