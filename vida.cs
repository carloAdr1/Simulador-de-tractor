using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class vida : MonoBehaviour
{
    public float salud = 100;
    private float saludmax = 100;
    public Image BarraSalud;
    public CanvasGroup visionDano;

    void Update()
    {
        if (visionDano.alpha > 0)
        {
            visionDano.alpha -= Time.deltaTime;
        }
        ActualizarInterfaz();

        if (salud <= 0)
        {
            PerderJuego();
        }
    }

    public void RecibirDamage(float daño)
    {
        salud -= daño;
        visionDano.alpha = 0.3f;
    }

    void ActualizarInterfaz()
    {
        BarraSalud.fillAmount = salud / saludmax;
    }

    void PerderJuego()
    {
        SceneManager.LoadScene("Perdiste"); // Cambia "Perdiste" por el nombre real de la escena
    }
}