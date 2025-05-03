using UnityEngine;

public class EscenaAnterior : MonoBehaviour
{
    public static EscenaAnterior instance;
    public int indiceEscenaAnterior = -1; // valor inicial imposible

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
}
