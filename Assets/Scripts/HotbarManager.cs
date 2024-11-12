using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    public Image[] blockImages; // Array of images for the blocks
    public GameObject[] blockPanels; // Array of panels for each block's background

    private int activeBlockIndex = -1; // Index of the currently active block
    private bool[] blockUnlocked; // Track which blocks are unlocked

    void Start()
    {
        // Initialize block unlocked status
        blockUnlocked = new bool[blockImages.Length];
        UpdateHotbarVisuals(); // Ensure the UI starts off without any blocks visible
    }

    // Call this function to update the hotbar whenever a block is unlocked
    public void UpdateHotbar(BlockType.BlockTypeEnum unlockedBlock)
    {
        int blockIndex = (int)unlockedBlock;
        if (blockIndex < 0 || blockIndex >= blockImages.Length) return;

        // Mark the block as unlocked
        blockUnlocked[blockIndex] = true;

        // Enable the image in gameobject and set it to half transparent by default
        blockImages[blockIndex].gameObject.SetActive(true);
        Color transparentColor = blockImages[blockIndex].color;
        transparentColor.a = 0.5f; // Set to transparent
        blockImages[blockIndex].color = transparentColor;

        // Set the active block to the newly unlocked block if none is active
        if (activeBlockIndex == -1 || !blockUnlocked[activeBlockIndex])
        {
            SetActiveBlock(blockIndex); //  set it to the newly unlocked block
        }

        UpdateHotbarVisuals();
    }

    // Method to update a specific hotbar slot
    public void UpdateHotbarSlot(int index, BlockType.BlockTypeEnum blockType)
    {
        // Ensure the index  of the unlocked block type is valid
        if (index < 0 || index >= blockImages.Length) return;

        // Update the slot if the block is being unlocked for the first time
        if (!blockUnlocked[index])
        {
            // Call UpdateHotbar to unlock the block and set visuals correctly
            UpdateHotbar(blockType);
        }
        else
        {
           
            Debug.Log($"Block at index {index} is already unlocked. Dont know how you triggered this.");
        }
    }

    // Set the active block based on the index of the given keypress
    public void SetActiveBlock(int index)
    {
        // Only allow setting active blocks that are unlocked
        if (index < 0 || index >= blockImages.Length || !blockUnlocked[index]) return;

        // Reset the previously active block
        if (activeBlockIndex != -1)
        { //everyblock by default will be this transparent, just override it, since for whatever reason when its unlocked and then unselected the crap wont stay transparent.
            Color inactiveColor = blockImages[activeBlockIndex].color;
            inactiveColor.a = 0.5f; // Set to transparent
            blockImages[activeBlockIndex].color = inactiveColor;
            blockPanels[activeBlockIndex].GetComponent<Image>().color = new Color32(115, 115, 115, 255); // Reset panel color
        }

        // Set the new active block like actually. make it less transparent
        activeBlockIndex = index;
        Color activeColor = blockImages[activeBlockIndex].color;
        activeColor.a = 1f; // Set to fully opaque
        blockImages[activeBlockIndex].color = activeColor;
        blockPanels[activeBlockIndex].GetComponent<Image>().color = new Color32(90, 90, 90, 255); // Darken the panel
    }

    // Update the visuals based on the current active block
    public void UpdateHotbarVisuals()
    {
        for (int i = 0; i < blockImages.Length; i++)
        {
            if (!blockUnlocked[i])
            {
                blockPanels[i].SetActive(false); // Hide panel if block is not unlocked
                blockImages[i].gameObject.SetActive(false); // Hide image if block is not unlocked
            }
            else
            {
                blockPanels[i].SetActive(true); // Show panel if block is unlocked
                blockImages[i].gameObject.SetActive(true); // Show image if block is unlocked
            }
        }
    }

    // Handle block selection based on number keys
    public void HandleBlockSelection(int index)
    {
        if (index < 1 || index > 3) return; // Only handle keys 1, 2, and 3
        SetActiveBlock(index - 1); // Convert 1-3 to 0-2
    }
}
