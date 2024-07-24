using UnityEngine;

public class NodeSelectionVisualizer : MonoBehaviour {
    public GameObject nodeVisualPrefab; // Assign your cube prefab in the inspector
    private GameObject currentNodeVisual;
    private Renderer nodeVisualRenderer;

    [HideInInspector]
    public Node nodeUnderMouse; // This will be updated by NodeSelector
    public float nodeDiameter;

    void Start() {
        // Instantiate the node visual but keep it disabled initially
        currentNodeVisual = Instantiate(nodeVisualPrefab);
        currentNodeVisual.SetActive(false);

        // Get the renderer component for changing the color
        nodeVisualRenderer = currentNodeVisual.GetComponent<Renderer>();
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
}
