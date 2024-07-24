using UnityEngine;

public class Node : IHeapItem<Node> {
    public bool walkable;
    public bool isBuildable;
    public Vector3 worldPosition;
    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public Node parent;
    int heapIndex;
    public GameObject wallObject;
    public GameObject towerObject;



    public Node(bool _walkable, bool _buildable, Vector3 _worldPosition, int _gridX, int _gridY){
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
        isBuildable = _buildable;
        wallObject = null;
        towerObject = null;
    }
    public int FCost {
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare) {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
            compare = hCost.CompareTo(nodeToCompare.hCost);
        return -compare;
    }
}
