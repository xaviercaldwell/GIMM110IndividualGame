using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BlockType;

public class BlockCollection : MonoBehaviour
{
    public BlockTypeEnum blockTypeToUnlock;  // The type of block this collectible will unlock
    public BlockSpawner blockSpawner;        // Reference to the BlockSpawner to unlock the block

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // Ensure the collectible only interacts with the player
        {
            blockSpawner.UnlockBlock(blockTypeToUnlock);  // Unlock the block type
            Destroy(gameObject);  // Destroy the collectible after it’s picked up
        }
    }
}
