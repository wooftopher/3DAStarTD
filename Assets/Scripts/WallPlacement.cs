using UnityEngine;

public class WallPlacement : MonoBehaviour {
    public GameObject wallPrefab;

    void Update() {
        if (Input.GetMouseButtonDown(0)) { // Left mouse button
            Vector3 mouseWorldPosition = Grid.Instance.GetMouseWorldPosition();
            Node nodeUnderMouse = Grid.Instance.NodeFromWorldPoint(mouseWorldPosition);
            PlaceWallAtNode(nodeUnderMouse);
        }
        if (Input.GetMouseButtonDown(1)) { // Right mouse button
            Vector3 mouseWorldPosition = Grid.Instance.GetMouseWorldPosition();
            Node nodeUnderMouse = Grid.Instance.NodeFromWorldPoint(mouseWorldPosition);
            RemoveWallAtNode(nodeUnderMouse);
        }
    }

    void PlaceWallAtNode(Node node) {
        if (node == null) {
            Debug.LogError("Node is null!");
            return;
        }
        if (node.walkable && node.buildable) {
            GameObject wallObject = Instantiate(wallPrefab, node.worldPosition, Quaternion.identity);
            node.walkable = false;
            node.wallObject = wallObject;
        } else {
            Debug.Log("Node is not walkable.");
        }
    }

    void RemoveWallAtNode (Node node) {
        if (node == null) {
            Debug.LogError("Node is null!");
            return;
        }
        if (!node.walkable) {
            Destroy(node.wallObject);
            node.walkable = true;
            node.wallObject = null;
        } else {
            Debug.Log("Node is not walkable.");
        }
    }
}
