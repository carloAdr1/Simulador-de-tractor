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
        SceneManager.LoadScene("Perdiste"); // Cambia "Perdiste" por el nombre real de la escena
    }
}
