using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnOnFall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DropDespawn"))
        {
            Debug.Log("Destroying block");
            Destroy(gameObject);
        }
    }
}
