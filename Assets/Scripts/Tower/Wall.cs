using UnityEngine;

public class Wall : MonoBehaviour {
    public GameObject towerObject;
    public Color originalColor;
    public Node node;
    void Start() {
        // Initialize the original color when the wall is created
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) {
            originalColor = renderer.material.color;
        } else {
            Debug.LogError("Renderer not found on the wall!");
        }
    }
}
