using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class vida : MonoBehaviour
{
    public float salud = 100;
    private float saludmax = 100;
    public Image BarraSalud;
    public CanvasGroup visionDano;

    public AudioSource sfxAlgo;
    public AudioClip sfxDamage;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            salud = GameManager.Instance.vidaActual;
        }

        salud = Mathf.Clamp(salud, 0, 100);
        GameManager.Instance.vidaActual = (int)salud;
    }

    void Update()
    {
        if (visionDano.alpha > 0)
        {
            visionDano.alpha -= Time.deltaTime;
        }

        GameManager.Instance.vidaActual = (int)salud;

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

        sfxAlgo.PlayOneShot(sfxDamage); // ✅ Sonido al recibir daño
    }

    void ActualizarInterfaz()
    {
        BarraSalud.fillAmount = salud / saludmax;
    }

    void PerderJuego()
    {
        SceneManager.LoadScene("Perdiste");
    }
}
