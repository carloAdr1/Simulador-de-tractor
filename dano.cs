using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dano : MonoBehaviour
{
    public float CantidadDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<vida>())
        {
            collision.gameObject.GetComponent<vida>().RecibirDamage(CantidadDamage);
        }
    }
}
