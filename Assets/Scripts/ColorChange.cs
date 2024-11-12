using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockColorChange : MonoBehaviour
{
    private Color originalColor;  // Store the original color
    [SerializeField] private Color highlightColor; // Color to change to on hover

    private void Start()
    {
        // Store the original color of the block
        originalColor = GetComponent<SpriteRenderer>().color;
    }

    public void ChangeColorToHighlight()
    {
        // Change the color to highlight color when mouse enters
        GetComponent<SpriteRenderer>().color = highlightColor;
    }

    public void ResetColor()
    {
        // Revert to original color when mouse exits
        GetComponent<SpriteRenderer>().color = originalColor;
    }
}
