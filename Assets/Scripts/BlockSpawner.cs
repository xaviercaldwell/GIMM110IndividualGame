using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[] blockPrefabs;   // Array to hold different block prefabs
    private Camera mainCamera;          // Reference to the main camera

    public List<BlockType.BlockTypeEnum> unlockedBlocks;  // List of unlocked block types
    private int currentBlockIndex;      // Index of the currently selected block
    public LayerMask blockLayer;        // LayerMask for blocks
    public bool canSpawn = true;        // Controls if spawning a block is allowed 
    public HotbarManager hotbarManager;

    // Create a dictionary to map block types to their hotbar slot indices
    private Dictionary<BlockType.BlockTypeEnum, int> blockToHotbarIndex;

    void Start()
    {
        mainCamera = Camera.main;
        unlockedBlocks = new List<BlockType.BlockTypeEnum>();
        currentBlockIndex = -1;  // Start with no block selected

        // Initialize the mapping for block types to hotbar slots
        blockToHotbarIndex = new Dictionary<BlockType.BlockTypeEnum, int>
        {
            { BlockType.BlockTypeEnum.SquareBlock, 0 },
            { BlockType.BlockTypeEnum.WideBlock, 1 },
            { BlockType.BlockTypeEnum.StairBlock, 2 }
        };
    }

    void Update()
    {
        HandleMouseHover();  // Handle highlighting blocks

        if (Input.GetMouseButtonDown(0))  // Left-click to delete or spawn the block
        {
            if (!TryDeleteBlock())  // Attempt to delete a block
            {
                if (canSpawn) SpawnBlock(); // Only spawn if no block was deleted
            }
        }

        // Cycle through blocks using the number keys (1, 2, 3)
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectBlock(BlockType.BlockTypeEnum.SquareBlock); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectBlock(BlockType.BlockTypeEnum.WideBlock); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectBlock(BlockType.BlockTypeEnum.StairBlock); }
    }

    // Handle highlighting over blocks
    void HandleMouseHover()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check for overlapping colliders at the mouse position while ignoring the "Ignore Input" layer
        Collider2D hit = Physics2D.OverlapPoint(mousePosition, blockLayer); // Use blockLayer here

        if (hit != null && hit.CompareTag("Droppable"))
        {
            BlockColorChange colorChange = hit.GetComponent<BlockColorChange>();
            if (colorChange != null)
            {
                colorChange.ChangeColorToHighlight();  // Change color to highlight
            }
        }
        else
        {
            // Reset color for all blocks when not hovering
            foreach (var block in GameObject.FindGameObjectsWithTag("Droppable"))
            {
                BlockColorChange colorChange = block.GetComponent<BlockColorChange>();
                if (colorChange != null)
                {
                    colorChange.ResetColor();  // Revert to original color
                }
            }
        }
    }

    // This function spawns a block based on the mouse's x-position but always from the top of the screen
    void SpawnBlock()
    {
        if (currentBlockIndex == -1) return;  // Do nothing if no block is selected

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;  // Ensure it spawns in front of the camera

        // Get the world position from the mouse x-coordinate
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Set the spawn height to the top of the screen relative to the camera
        float spawnHeight = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 10)).y;

        // Use the calculated height as the y-position for the block
        worldPosition.y = spawnHeight;

        // Instantiate the currently selected block type based on the hotbar slot
        GameObject spawnedBlock = Instantiate(blockPrefabs[currentBlockIndex], worldPosition, Quaternion.identity);
        spawnedBlock.GetComponent<Rigidbody2D>().isKinematic = false;  // Enable gravity on the block
    }

    // Unlock a new block type
    public void UnlockBlock(BlockType.BlockTypeEnum newBlockType)
    {
        if (!unlockedBlocks.Contains(newBlockType))
        {
            unlockedBlocks.Add(newBlockType);

            int hotbarIndex = blockToHotbarIndex[newBlockType];  // Get the UI slot for this block
            hotbarManager.UpdateHotbarSlot(hotbarIndex, newBlockType);  // Update the specific slot

            // If this is the first unlocked block, select it automatically
            if (currentBlockIndex == -1)
            {
                SelectBlock(newBlockType);
            }

            Debug.Log("Unlocked: " + newBlockType.ToString());
        }
    }

    // Select a block from the fixed slots in the hotbar
    public void SelectBlock(BlockType.BlockTypeEnum blockType)
    {
        if (!unlockedBlocks.Contains(blockType)) return;  // Only allow selection if the block is unlocked

        int hotbarIndex = blockToHotbarIndex[blockType];  // Get the hotbar slot for the selected block
        currentBlockIndex = hotbarIndex;  // Set the block index based on its slot

        hotbarManager.SetActiveBlock(hotbarIndex);  // Update the UI to show the active block
    }

    // Attempt to delete the block under the mouse
    bool TryDeleteBlock()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check for overlapping colliders at the mouse position while ignoring the "Ignore Input" layer
        Collider2D hit = Physics2D.OverlapPoint(mousePosition, blockLayer); // Use blockLayer here
        if (hit != null && hit.CompareTag("Droppable"))  // Ensure it's actually deletable
        {
            Debug.Log("Attempting to delete: " + hit.name);  // Log the block being deleted
            Destroy(hit.gameObject);  // Destroy the block
            Debug.Log("Block Deleted: " + hit.name);
            return true;
        }

        // Debugging
        Debug.Log("No block to delete)");
        return false;
    }
}
