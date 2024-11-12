using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    private bool playerReachedGoal = false;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        // Check if player has reached the goal and hasn't triggered the win condition already
        if (coll.CompareTag("Player") && !playerReachedGoal)
        {
            playerReachedGoal = true; // Set win condition
            Win();
        }
    }

    private void Win()
    {
        // Load the win screen
        SceneManager.LoadScene("EndScreen");
    }
}
