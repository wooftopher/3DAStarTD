using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private int gold = 100;      // Initial gold amount
    [SerializeField] private int lives = 10;      // Initial lives
    [SerializeField] private int iceBlocks = 5;   // Initial ice block amount
    private int currentWave = 1;
    private InfoUI infoUI;

    public int Gold { get { return gold; } private set { gold = value; UpdatePlayerInfo(); } }
    public int Lives { get { return lives; } private set { lives = value; UpdatePlayerInfo(); } }
    public int IceBlocks { get { return iceBlocks; } private set { iceBlocks = value; UpdatePlayerInfo(); } }
    public int CurrentWave { get { return currentWave; } private set { currentWave = value; UpdatePlayerInfo(); } }

    void Start() {
        infoUI = FindObjectOfType<InfoUI>();
        UpdatePlayerInfo(); // Update UI at the start
    }

    public void SetCurrentWave(int wave) {
        CurrentWave = wave; // This will call UpdatePlayerInfo
        UpdatePlayerInfo(); // Update UI at the start
    }

    // Method to spend gold, returns true if successful, false if not enough gold
    public bool SpendGold(int amount) {
        if (Gold >= amount) {
            Gold -= amount; // This will call UpdatePlayerInfo
            return true;
        } else {
            Debug.LogWarning("Not enough gold!");
            return false;
        }
    }

    // Method to earn gold
    public void EarnGold(int amount) {
        Gold += amount; // This will call UpdatePlayerInfo
    }

    // Method to lose a life
    public void LoseLife() {
        if (Lives > 0) {
            Lives--; // This will call UpdatePlayerInfo
            if (Lives <= 0) {
                GameOver();
            }
        }
    }

    // Method to use an ice block, returns true if successful, false if not enough ice blocks
    public bool UseIceBlock() {
        if (IceBlocks > 0) {
            IceBlocks--; // This will call UpdatePlayerInfo
            return true;
        } else {
            Debug.LogWarning("Not enough ice blocks!");
            return false;
        }
    }

    // Method to earn ice blocks
    public void EarnIceBlock(int amount) {
        IceBlocks += amount; // This will call UpdatePlayerInfo
    }

    // Check if player can build a tower (based on cost)
    public bool CanBuildTower(int towerCost) {
        return Gold >= towerCost;
    }

    // Check if player can build a wall (based on ice blocks)
    public bool CanBuildWall() {
        return IceBlocks > 0;
    }

    // Game over logic (you can customize this method)
    private void GameOver() {
        Debug.Log("Game Over! The player has lost all lives.");
        // Add game over handling here (e.g., reload scene, show game over UI, etc.)

        // Get all game objects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Loop through and destroy each object
        foreach (GameObject obj in allObjects) {
            Destroy(obj);
        }
    }

    // Method to update the UI with the current player's info
    private void UpdatePlayerInfo() {
        if (infoUI != null) {
            infoUI.UpdatePlayerInfo(this); // Pass this player instance to update the UI
        } else {
            Debug.LogError("InfoUI component not found!");
        }
    }
}
