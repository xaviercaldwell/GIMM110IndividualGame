using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullPlayer : MonoBehaviour
{
    [Header("Pull Settings")]
    [SerializeField] private float pullRadius; // Radius within which the trap can pull the player
    [SerializeField] private float maxPullForce; // Maximum force applied when the player is right next to the trap
    [SerializeField] private LayerMask playerLayer; // Layer for the player

    private void FixedUpdate()
    {
        // Check for nearby players
        Collider2D[] playersInRange = Physics2D.OverlapCircleAll(transform.position, pullRadius, playerLayer);

        foreach (var player in playersInRange)
        {
            // Calculate the direction and distance to the player
            Vector2 direction = (transform.position - player.transform.position).normalized; //normalize so it doesn't multiply pullforce
            float distance = Vector2.Distance(transform.position, player.transform.position);

            // Calculate pull intensity based on distance with linear interpolation. Between MAX and 0, based on distance/radius
            float pullIntensity = Mathf.Lerp(maxPullForce, 0, distance / pullRadius);

            // Apply the force to the player's Rigidbody2D
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.AddForce(direction * pullIntensity, ForceMode2D.Force); //pull the player
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the pull radius in the editor for visualization cause thats hard to see
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
