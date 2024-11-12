using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public string platformTag = "Platform"; // Tag for platforms
    public string droppableTag = "Droppable"; // Tag for droppable blocks
    // Start is called before the first frame update
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb =gameObject.GetComponent<Rigidbody2D>();
    }


    // single comment line
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Die();
        }
    }


    public void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        Invoke("RestartLevel", 2f);
        
    }

    private void RestartLevel()
    {
        DeleteAllSpawnedObjects();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    // Method to delete all platforms and droppable blocks cauyse this keeps crashing
    private void DeleteAllSpawnedObjects()
    {
        DeleteAllObjectsWithTag(platformTag); // Delete platforms
        DeleteAllObjectsWithTag(droppableTag); // Delete droppable blocks
    }

    // Helper method to find and delete all objects with a specific tag
    private void DeleteAllObjectsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj); // Destroy the object
        }
    }
}
