using UnityEngine;
using TMPro; // Add this line for TextMesh Pro
using UnityEngine.UI;

public class InfoUI : MonoBehaviour {
    public TMP_Text text1; // Tower name
    public TMP_Text text2; // Damage
    public TMP_Text text3; // Attack Speed
    public TMP_Text text4; // Attack Range
    public TMP_Text waveText;      // Current Wave
    public TMP_Text livesText;     // Lives
    public TMP_Text goldText;      // Gold
    public TMP_Text iceBlocksText; // Ice Blocks
    public Text sellButtonText;

    public GameObject towerPanel;
    public GameObject emptyPanel;
    public GameObject wallPanel;

    private void Start() {
        // Initially show the empty panel
        ShowEmptyPanel();
    }

    public void UpdateTowerInfo(BaseTower tower) {
        if (tower == null) {
            Debug.LogError("Tower is null!");
            ShowEmptyPanel(); // Show empty panel if no tower is selected
            return;
        }

        // Set tower name using the getter
        text1.text = tower.TowerName; // Fallback for null
        text2.text = "Damage: " + tower.Damage.ToString(); // Set damage with description
        text3.text = "Att Speed: " + (1 / tower.ShootCooldown).ToString("F2"); // Attack speed with description
        text4.text = "Range: " + tower.Range.ToString(); // Set range with description
        sellButtonText.text = "Sell (" + tower.Price.ToString() + ")";

        ShowTowerPanel(); // Show tower panel when tower info is updated
    }
    
    public void UpdatePlayerInfo(Player player) {
        if (player == null) {
            Debug.LogError("Player is null!");
            return;
        }

        waveText.text = "Wave " + player.CurrentWave.ToString(); // Update wave
        livesText.text = "Lives: " + player.Lives.ToString(); // Update lives
        goldText.text = "Gold: " + player.Gold.ToString(); // Update gold
        iceBlocksText.text = "Ice Blocks: " + player.IceBlocks.ToString(); // Update ice blocks

        // Debugging output
        // Debug.Log($"Updated UI with Wave: {waveText.text}, Lives: {livesText.text}, Gold: {goldText.text}, Ice Blocks: {iceBlocksText.text}");
    }

    public void ShowTowerPanel() {
        towerPanel.SetActive(true);
        emptyPanel.SetActive(false);
        wallPanel.SetActive(false);
    }

    public void ShowEmptyPanel() {
        emptyPanel.SetActive(true);
        towerPanel.SetActive(false);
        wallPanel.SetActive(false);
    }
    public void ShowWallPanel() {
        wallPanel.SetActive(true);
        towerPanel.SetActive(false);
        emptyPanel.SetActive(false);
    }
}
