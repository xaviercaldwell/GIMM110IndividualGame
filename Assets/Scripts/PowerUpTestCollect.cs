using UnityEngine;

public class PowerUpTestCollect : MonoBehaviour
{
    #region Variables
    public PlatformManager platformManagerScript;
    public Movement2D move;
    #endregion

    #region Unity Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int i = 0; //Random.Range(0,2) if i wanted a random power up from an enum or something from 0-2 different powerups

        switch (i)
        { 
            case 0:
                move.setJumpForce(move.getJumpForce() * 1.25f);
                break;
            
        }
        Destroy(this.gameObject);
                
        // Calls the NewPlatform method from the PlatformManager script.
        platformManagerScript.NewPlatform();
    }
    #endregion
}
