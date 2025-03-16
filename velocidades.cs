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
            if (playerMove.isMoving || playerMove.isReversing) 
            {
                // Mostrar la velocidad actual (+1 porque el índice inicia en 0)
                velocidadTexto.text = (playerMove.currentSpeedLevel + 1).ToString();
            }
            else
            {
                // Si el tractor no se mueve, mostrar 0
                velocidadTexto.text = "0";
            }
        }
    }
}

