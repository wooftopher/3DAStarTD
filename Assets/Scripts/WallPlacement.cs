using UnityEngine;

public class WallPlacement : MonoBehaviour {
    public GameObject wallPrefab;
    public GameObject towerPrefab;
    public Transform spawner;
    public Transform goal;

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
        } 
        else if (node.towerObject) {
            Debug.Log("Wall already has a tower!");
        } 
        else if (node.wallObject) {
            // Place tower on top of the existing wall
            Vector3 additionalObjectPosition = node.wallObject.transform.position;
            additionalObjectPosition.y += wallPrefab.GetComponent<Renderer>().bounds.size.y; // Adjust the y position to place it on top of the wall
            // additionalObjectPosition.z += 0.25f;
            GameObject towerObject = Instantiate(towerPrefab, additionalObjectPosition, Quaternion.identity);
            node.towerObject = towerObject;
        } 
        else if (node.walkable && node.buildable) {
            // Place a new wall
            Vector3 offsetPosition = node.worldPosition;
            offsetPosition.y += 0.3f;
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
            GameObject wallObject = Instantiate(wallPrefab, offsetPosition, randomRotation);
            node.walkable = false;
            node.wallObject = wallObject;

            // Check if the path is still valid
            bool pathExists = Pathfinding.Instance.DoesPathExist(spawner.position, goal.position);
            if (!pathExists) {
                Destroy(wallObject);
                node.walkable = true;
                node.wallObject = null;
                Debug.Log("Wall placement blocked the path. Wall removed.");
            }
        } 
        else {
            Debug.Log("Node is not walkable or buildable.");
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
