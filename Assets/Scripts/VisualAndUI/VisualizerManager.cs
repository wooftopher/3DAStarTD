using UnityEngine;

public class VisualizerManager : MonoBehaviour {
    private NodeSelectionVisualizer nodeSelectionVisualizer;
    private WallPlacement builderManager;


    private void Awake(){
        nodeSelectionVisualizer = GetComponent<NodeSelectionVisualizer>();
    }

    public NodeSelectionVisualizer GetNodeSelectionVisualizer(){
        return nodeSelectionVisualizer;
    }
    
    // private void HighlightWall(Wall wall) {
    //     Wall lastHoveredWall = builderManager.GetLastHoveredWall();
    //     Wall selectedWall = builderManager.GetSelectedWall();
    //     if (lastHoveredWall != wall && selectedWall != wall) { // Only highlight if not already hovered or selected
    //         Renderer wallRenderer = wall.GetComponent<Renderer>();
    //         if (wallRenderer != null) {
    //             if (lastHoveredWall != null && lastHoveredWall != selectedWall) {
    //                 ResetWallColor(lastHoveredWall);
    //             }

    //             // lastHoveredWall = wall; // Set the current wall as hovered
    //             builderManager.SetLastHoveredWall(wall);

    //             // Set the color to a light bluish filter for hover
    //             if (selectedWall != wall)
    //                 wallRenderer.material.color = new Color(0.7f, 0.8f, 1f, 1f); 
    //         } else {
    //             Debug.LogError("Renderer not found on the hovered wall!");
    //         }
    //     }
    // }
    // protected void ResetWallColor(Wall wall) {
    //     if (wall != null) {
    //         Renderer wallRenderer = wall.GetComponent<Renderer>();
    //         if (wallRenderer != null) {
    //             wallRenderer.material.color = wall.originalColor; // Restore the original color
    //         }
    //     }
    // }
    
}
