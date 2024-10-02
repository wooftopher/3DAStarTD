using UnityEngine;

public class NodeSelectionVisualizer : MonoBehaviour {
    public GameObject nodeVisualPrefab; // Assign your cube prefab in the inspector
    public GameObject towerVisualPrefab;
    private GameObject currentTowerVisual;
    private GameObject currentNodeVisual;
    private Renderer nodeVisualRenderer;
    private Renderer towerVisualRenderer;

    [HideInInspector]
    public Node nodeUnderMouse; // This will be updated by NodeSelector
    public float nodeDiameter;

    void Start() {
        // Instantiate the node visual but keep it disabled initially
        currentNodeVisual = Instantiate(nodeVisualPrefab);
        currentNodeVisual.SetActive(false);

        // Get the renderer component for changing the color
        nodeVisualRenderer = currentNodeVisual.GetComponent<Renderer>();

        // currentTowerVisual = Instantiate(towerVisualPrefab);
        // currentTowerVisual.SetActive(false);
        // towerVisualRenderer = currentTowerVisual.GetComponent<Renderer>();
    }

    public void ShowNodeVisual(Node node){
        if (node != null) {
            // Set the position of the visual indicator
            Vector3 position = new Vector3(node.worldPosition.x, transform.position.y + 0.05f, node.worldPosition.z);
            currentNodeVisual.transform.position = position;
            currentNodeVisual.transform.localScale = Vector3.one * (nodeDiameter - 0.1f);
            currentNodeVisual.SetActive(true); // Enable the visual indicator

            // Change color based on node properties
            if (node.isBuildable) {
                nodeVisualRenderer.material.color = Color.green; // Buildable
            } else {
                nodeVisualRenderer.material.color = Color.red; // Not buildable
            }
        }
    }

    void Update() {
   
    }

    public void HideVisualizer() {
        if (currentNodeVisual != null) {
            currentNodeVisual.SetActive(false); // Hide the visual indicator
        }
    }

    public void ShowTowerVisual(Wall wall) {
        if (wall != null) {
            Vector3 position = wall.transform.position;
            position.y += wall.GetComponent<Renderer>().bounds.size.y; // Adjust the y position to place it on top of the wall
            currentTowerVisual.transform.position = position;
            currentTowerVisual.SetActive(true);
        }
    }

    public void HideTowerVisual() {
        currentTowerVisual.SetActive(false);
    }
}
