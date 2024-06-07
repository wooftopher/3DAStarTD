using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    // public Transform seeker, target;
    PathRequestManager RequestManager;

    Grid grid;

    void Awake(){
        grid = GetComponent<Grid>();
        RequestManager = GetComponent<PathRequestManager>();
    }

    // void Update(){
    //     // if (Input.GetButtonDown("Jump"))
    //         FindPath(seeker.position, target.position);
    // }
    public void StartFindPath(Vector3 startPos, Vector3 targetPos){
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos){

        // Stopwatch sw  = new();
        // sw.Start();
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        // List<Node> openSet = new();
        Heap<Node> openSet = new(grid.MaxSize);
        HashSet<Node> closeSet = new();
        openSet.Add(startNode);

        while (openSet.Count > 0){
            Node currentNode = openSet.RemoveFirst();
            // Node currentNode = openSet[0];
            // for (int i = 1; i < openSet.Count; i++){
            //     if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost){
            //         currentNode = openSet[i];
            //     }
            // }
            // openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            if (currentNode == targetNode){
                // sw.Stop();
                // print("Path found: " + sw.ElapsedMilliseconds + "ms");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode)){
                if (!neighbour.walkable || closeSet.Contains(neighbour))
                    continue;

                int newMouvementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMouvementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
                    neighbour.gCost = newMouvementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
            
        }
        yeild return null;
    }

    void RetracePath(Node startNode, Node endNode){
        List<Node> path = new();
        Node currentNode = endNode;
        
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }
    
    int GetDistance(Node nodeA, Node nodeB){
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
