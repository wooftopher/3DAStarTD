using UnityEngine;
using UnityEngine.UI;

public class BuildModeManager : MonoBehaviour {
    public Button[] buildModeButtons; // Array to hold the UI Button elements for build modes 1 to 4
    private WallPlacement wallPlacementScript; // Reference to the wall placement script

    void Start() {
        // Get the WallPlacement script component from the same GameObject
        wallPlacementScript = GetComponent<WallPlacement>();

        if (wallPlacementScript == null) {
            Debug.LogError("WallPlacement script not found on the same GameObject!");
        }
        SetBuildMode(1);
    }

    void Update() {
        // Check for key presses (1 to 4) and update the build mode
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SetBuildMode(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SetBuildMode(2);
        }
        // } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
        //     SetBuildMode(3);
        // } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
        //     SetBuildMode(4);
        // }
    }

    void SetBuildMode(int mode) {
        // Update the build mode in the wall placement script
        if (wallPlacementScript != null) {
            wallPlacementScript.SetBuildMode(mode);

            // Highlight the corresponding UI Button element
            for (int i = 0; i < buildModeButtons.Length; i++) {
                if (i == mode - 1) {
                    buildModeButtons[i].image.color = Color.yellow; // Highlight color
                } else {
                    buildModeButtons[i].image.color = Color.white; // Default color
                }
            }
        }
    }
}
