using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public static Grid Instance { get; private set; }
    public bool displayGridGizmos;
    public bool displayNodeUnderMouse;
    public Transform enemy;
    public LayerMask unwalkableMask;
    public LayerMask unbuildableMask;
    public LayerMask groundLayerMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public int test;
    Node[,] grid;

    public Transform unit;
    public Transform goal;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    Node nodeUnderMouse;
    Node unitNode;
    Node goalNode;

    void Awake() {
        // Ensure only one instance of the Grid exists
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Initialize the grid
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize {
        get {
            return  gridSizeX * gridSizeY;
        }
    }

    void CreateGrid(){
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++){
            for (int y = 0; y < gridSizeY; y++){
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                bool buildable = !Physics.CheckSphere(worldPoint, nodeRadius, unbuildableMask);
                grid[x,y] = new Node(walkable, buildable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node){
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                // Exclude the current node
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // Check if the neighbour is within the grid bounds
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    // Exclude diagonal neighbours if they would cut through corners or between walls
                    if (!(Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1 && (!grid[node.gridX + x, node.gridY].walkable || !grid[node.gridX, node.gridY + y].walkable)))
                        neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 wolrdPosition){
        float percentX = (wolrdPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float percentY = (wolrdPosition.z + gridWorldSize.y/2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
        return grid[x,y];
    }
    void Update() {
        if (displayNodeUnderMouse) {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            nodeUnderMouse = NodeFromWorldPoint(mouseWorldPosition);
        }
        if (unit != null) {
            unitNode = NodeFromWorldPoint(unit.position);
        }
        if (goal != null)
            goalNode = NodeFromWorldPoint(goal.position);
    }
    public Vector3 GetMouseWorldPosition() {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 0; // Set a reasonable z-coordinate based on the camera position
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }



    public List<Node> path;
    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 0.1f, gridWorldSize.y)); // Draw the wire cube for the ground

        if (nodeUnderMouse != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(new Vector3(nodeUnderMouse.worldPosition.x, transform.position.y + 0.05f, nodeUnderMouse.worldPosition.z), Vector3.one * (nodeDiameter - 0.1f)); // Draw cube under mouse
        }

        if (grid != null) {
            foreach (Node n in grid) {
                if (!n.buildable) {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawCube(new Vector3(n.worldPosition.x, transform.position.y + 0.05f, n.worldPosition.z), Vector3.one * (nodeDiameter - 0.1f)); // Draw non-buildable cubes
                }
                if (displayGridGizmos && !n.walkable) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(new Vector3(n.worldPosition.x, transform.position.y + 0.05f, n.worldPosition.z), Vector3.one * (nodeDiameter - 0.1f)); // Draw walkable/non-walkable cubes
                }
            }
        }
    }

}
