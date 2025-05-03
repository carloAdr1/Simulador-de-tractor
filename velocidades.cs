using UnityEngine;
using UnityEngine.UI;

public class Velocidades : MonoBehaviour
{
    public Text velocidadTexto; // Referencia al Text (Legacy) en la UI

    private PlayerMove playerMove; // Referencia al script del movimiento

    void Start()
    {
        playerMove = GetComponent<PlayerMove>(); // Buscar automáticamente el script en el mismo objeto
    }

    void Update()
    {
        if (playerMove != null && velocidadTexto != null)
        {
            velocidadTexto.text = playerMove.MarchaVisual.ToString();
        }
    }
}

