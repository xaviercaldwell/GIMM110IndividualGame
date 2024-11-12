using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleMovement : MonoBehaviour
{
    public float rotationSpeed = 15f;    // Speed at which the collectible rotates
    public float scaleSpeed = 0.5f;      // Speed at which the collectible grows and shrinks
    public float scaleAmount = 0.05f;    // Amount by which the collectible scales

    private Vector3 initialScale;        // The initial scale of the collectible

    void Start()
    {
        // Store the initial scale to use for scaling up and down
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Rotate the collectible back and forth between +-8 degrees
        float rotationAngle = Mathf.Sin(Time.time * rotationSpeed) * 8f;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

        // Scale the collectible up and down smoothly over time
        float scaleModifier = 1 + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
        transform.localScale = initialScale * scaleModifier;
    }
}
