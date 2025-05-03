using UnityEngine;
using System.Collections.Generic;

public class ControladorCultivo : MonoBehaviour
{
    public static ControladorCultivo instance;

    public List<bool> zanahoriasRecolectadas = new List<bool>();
    public GameObject[] zanahoriasEnEscena;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GuardarEstado()
{
    zanahoriasRecolectadas.Clear();

    foreach (var zanahoria in zanahoriasEnEscena)
    {
        if (zanahoria == null)
        {
            Debug.LogWarning("⚠️ Zanahoria nula en la lista");
            zanahoriasRecolectadas.Add(true); // Consideramos recolectada
        }
        else
        {
            zanahoriasRecolectadas.Add(!zanahoria.activeSelf);
        }
    }
}


    public void RestaurarEstado()
    {
        for (int i = 0; i < zanahoriasEnEscena.Length; i++)
        {
            if (zanahoriasRecolectadas.Count > i)
            {
                zanahoriasEnEscena[i].SetActive(!zanahoriasRecolectadas[i]);
            }
        }
    }

    public void Replantar()
{
    // Asegura que las referencias estén actualizadas antes de activar
    zanahoriasEnEscena = GameObject.FindGameObjectsWithTag("Zanahoria");

    for (int i = 0; i < zanahoriasEnEscena.Length; i++)
    {
        zanahoriasEnEscena[i].SetActive(true);
    }

    GuardarEstado();
    Debug.Log("🌱 Replantado completado desde ControladorCultivo");
}

}

