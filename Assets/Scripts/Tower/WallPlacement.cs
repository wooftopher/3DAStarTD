using UnityEditor;
using UnityEngine;

public class WallPlacement : MonoBehaviour {

    // private Grid grid;
    private NodeSelectionVisualizer Nodevisualizer;
    public GameObject wallPrefab;
    public GameObject towerPrefab;
    public Transform spawner;
    public Transform goal;
    private int currentBuildMode;

    void Start(){
        Nodevisualizer = GetComponent<NodeSelectionVisualizer>();
    }

    public void SetBuildMode(int mode) {
        currentBuildMode = mode;
        // Add logic here to handle different build modes based on currentBuildMode
    }

    void Update() {
        if (currentBuildMode != 2)
            Nodevisualizer.HideVisualizer();
        if (currentBuildMode == 1) {
            
        }
        else if (currentBuildMode == 2) {
            Vector3 mouseWorldPosition = Grid.Instance.GetMouseWorldPosition();
            Node nodeUnderMouse = Grid.Instance.NodeFromWorldPoint(mouseWorldPosition);
            Nodevisualizer.ShowNodeVisual(nodeUnderMouse);
            if (Input.GetMouseButtonDown(0)) { // Left mouse button
                PlaceWallAtNode(nodeUnderMouse);
            }
        }
        else if (currentBuildMode == 3) {
            Wall wallUnderMouse = GetWallUnderMouse();
            if (Input.GetMouseButtonDown(0) && wallUnderMouse) { // Left mouse button
                PlaceTowerOnWall(wallUnderMouse);
            }
        }
        else if (currentBuildMode == 4) {
            
        }

        if (Input.GetMouseButtonDown(1)) { // Right mouse button
            Vector3 mouseWorldPosition = Grid.Instance.GetMouseWorldPosition();
            Node nodeUnderMouse = Grid.Instance.NodeFromWorldPoint(mouseWorldPosition);
            RemoveWallAtNode(nodeUnderMouse);
        }
    }

    Wall GetWallUnderMouse() {
        // Raycast from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit)) {
            // Check if the hit object is a wall
            Wall wall = hit.collider.GetComponent<Wall>();
            return wall;
        }

        // No wall found
        return null;
    }

    void PlaceTowerOnWall(Wall wall){
        if (wall == null) {
            Debug.LogError("Wall is null! Cannot place tower.");
            return;
        }
        // Place tower on top of the existing wall
        Vector3 additionalObjectPosition = wall.transform.position;
        additionalObjectPosition.y += wallPrefab.GetComponent<Renderer>().bounds.size.y; // Adjust the y position to place it on top of the wall
        // additionalObjectPosition.z += 0.25f;
        GameObject towerObject = Instantiate(towerPrefab, additionalObjectPosition, Quaternion.identity);
        wall.towerObject = towerObject;    
    }

    void PlaceWallAtNode(Node node) {
        if (node == null) {
            Debug.LogError("Node is null!");
        } 
        else if (node.towerObject) {
            Debug.Log("Wall already has a tower!");
        } 
        // else if (node.wallObject) {
        //     // Place tower on top of the existing wall
        //     Vector3 additionalObjectPosition = node.wallObject.transform.position;
        //     additionalObjectPosition.y += wallPrefab.GetComponent<Renderer>().bounds.size.y; // Adjust the y position to place it on top of the wall
        //     // additionalObjectPosition.z += 0.25f;
        //     GameObject towerObject = Instantiate(towerPrefab, additionalObjectPosition, Quaternion.identity);
        //     node.towerObject = towerObject;
        // } 
        else if (node.walkable && node.isBuildable) {
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
            Debug.Log("Node is not walkable or Buildable.");
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
