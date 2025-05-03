using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gasolina : MonoBehaviour
{
    public float gasolina = 100f;
    private float gasolinaMax = 100f;
    public Image barraGasolina;
    private PlayerMove playerMove;
    private float consumoTimer = 0f;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();

        if (GameManager.Instance != null)
        {
            gasolina = GameManager.Instance.gasolinaActual;
        }

        gasolina = Mathf.Clamp(gasolina, 0, 100);
        GameManager.Instance.gasolinaActual = (int)gasolina;
    }

    void Update()
    {
        if (playerMove != null && playerMove.isMoving)
        {
            consumoTimer += Time.deltaTime;
            if (consumoTimer >= 1f)
            {
                gasolina -= (playerMove.currentSpeedLevel + 1);
                consumoTimer = 0f;
            }
        }

        gasolina = Mathf.Clamp(gasolina, 0, gasolinaMax);
        ActualizarInterfaz();

        // Actualizar GameManager en cada frame
        GameManager.Instance.gasolinaActual = (int)gasolina;

        // Enviar señal de gasolina baja al hardware (VHDL)
        if (SerialSender.instance != null)
        {
            if (gasolina <= 50)
            {
                SerialSender.instance.EnviarValor(0xAA); // Encender LED
            }
            else
            {
                SerialSender.instance.EnviarValor(0xAB); // Apagar LED
            }
        }

        if (gasolina <= 0)
        {
            PerderJuego();
        }
    }

    void ActualizarInterfaz()
    {
        if (barraGasolina != null)
        {
            barraGasolina.fillAmount = gasolina / gasolinaMax;
        }
    }

    void PerderJuego()
    {
        SceneManager.LoadScene("Perdiste"); // Cambia "Perdiste" por el nombre real
        }
}
