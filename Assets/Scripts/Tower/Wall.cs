using UnityEngine;

public class Wall : MonoBehaviour, ISelectable {
    private WallPlacement builderManager;
    private InfoUI infoUI;
    private Renderer wallRenderer;
    public GameObject towerObject;
    public Color originalColor;
    public Node node;
    void Start() {
        builderManager = FindObjectOfType<WallPlacement>();//<-- to fix
        if (builderManager == null) {
            Debug.LogError("No WallPlacement found in the scene!");
        }
        infoUI = FindObjectOfType<InfoUI>();  // This will find the first InfoUI component in the scene
        if (infoUI == null) {
            Debug.LogError("No InfoUI found in the scene!");
        }
        wallRenderer = GetComponent<Renderer>();
        if (wallRenderer != null) {
            originalColor = wallRenderer.material.color;
        } else {
            Debug.LogError("Renderer not found on the wall!");
        }
    }

    public void ResetColor() {
        if (wallRenderer != null) {
            wallRenderer.material.color = originalColor;
        }
    }

    public void Select() {
        ISelectable lastSelected = builderManager.GetLastSelectedSelectable();
        if (lastSelected != null) {
            lastSelected.ResetColor();  // Reset color on the last selected object
        } 
        builderManager.SetLastSelectedSelectable(this);
        builderManager.SetSelectedWall(this);// <--- fix the rest so we dont need

        // Ensure infoUI is not null before calling the method
        if (infoUI == null) {
            Debug.LogError("InfoUI is not assigned!");
            return;
        }
        infoUI.ShowWallPanel();
        if (wallRenderer != null) {
            wallRenderer.material.color = new Color(0.2f, 0.3f, 0.6f, 1f); // Darker blue filter
        } else {
            Debug.LogError("Renderer not found on the selected wall!");
        }
    }

    // private void HighlightWall(Wall wall) {
    //     if (lastHoveredWall != wall && selectedWall != wall) { // Only highlight if not already hovered or selected
    //         Renderer wallRenderer = wall.GetComponent<Renderer>();
    //         if (wallRenderer != null) {
    //             if (lastHoveredWall != null && lastHoveredWall != selectedWall) {
    //                 lastHoveredWall.ResetColor();
    //             }

    //             lastHoveredWall = wall; // Set the current wall as hovered

    //             // Set the color to a light bluish filter for hover
    //             if (selectedWall != wall)
    //                 wallRenderer.material.color = new Color(0.7f, 0.8f, 1f, 1f); 
    //         } else {
    //             Debug.LogError("Renderer not found on the hovered wall!");
    //         }
    //     }
    // }

}
