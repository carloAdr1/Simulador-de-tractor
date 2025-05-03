using UnityEngine;

public class DarPunto : MonoBehaviour
{
    public int cantidadPuntos = 1;

    public AudioSource sfxAlgo;       // Asignar en Inspector
    public AudioClip sfxRecolectar;   // Sonido de recoger punto

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Puntos>())
        {
            other.GetComponent<Puntos>().SumarPunto(cantidadPuntos);

            if (sfxAlgo && sfxRecolectar)
            {
                sfxAlgo.PlayOneShot(sfxRecolectar);
            }

            Destroy(gameObject); // Elimina la zanahoria
        }
    }
}


