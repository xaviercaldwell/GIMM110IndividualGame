using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public float levelTime = 10f; // Set the timer to 10 seconds
    private float timeRemaining;
    public TMP_Text timerText; // Reference to the TextMeshPro component

    public GameObject player; // Reference to the player GameObject in the inspector
    private PlayerLife playerLife; // Reference to PlayerLife script
    private bool hasTimeExpired = false; // Flag to prevent repeated calls to TimeUp

    void Start()
    {
        timeRemaining = levelTime;
        UpdateTimerDisplay();

        if (player != null)
        {
            playerLife = player.GetComponent<PlayerLife>();
            if (playerLife == null)
            {
                Debug.LogError("PlayerLife script not found on the Player object");
            }
        }
        else
        {
            Debug.LogError("Player is not assigned in the inspector");
        }
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Decrease time
            UpdateTimerDisplay();
        }
        else if (!hasTimeExpired) // Check if time has expired and hasn't already triggered
        {
            hasTimeExpired = true; // Set the flag to true to prevent repeated calls
            TimeUp(); // Trigger player death
        }
    }

    void UpdateTimerDisplay()
    {
        // Format the time in minutes and seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeRemaining <= 60)
        {
            timerText.color = Color.red; // Change color to red
        }
    }

    void TimeUp()
    {

        timerText.text = "Time's up";

        // Logic for losing when time runs out
        Debug.Log("Time's up");

        // Call the die method on the playerLife script
        if (playerLife != null)
        {
            playerLife.Die(); // Call die from playerLife
        }
        else
        {
            Debug.LogError("PlayerLife script not found on the assigned player object!");
        }
    }
}
