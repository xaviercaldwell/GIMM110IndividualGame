using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only set the parent if the platform is active
        if (gameObject.activeInHierarchy)
        {
            if (collision.gameObject.name == "Player")
            {
                collision.gameObject.transform.SetParent(transform);
            }

            if (collision.gameObject.CompareTag("Droppable"))
            {
                collision.gameObject.transform.SetParent(transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Only unset the parent if the platform is active
        if (gameObject.activeInHierarchy)
        {
            if (collision.gameObject.name == "Player")
            {
                collision.gameObject.transform.SetParent(null);
            }

            if (collision.gameObject.CompareTag("Droppable"))
            {
                collision.gameObject.transform.SetParent(null);
            }
        }
    }
}
