using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour {
    public GameObject target;
    public float speed = 5;
    Vector3[] path;
    int targetIndex;
    Vector3 startPos;
    public bool showPathGizmos;

    void Awake() {
        startPos = transform.position;
        RequestNewPath();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            StopAndReset();
        }
    }

    void RequestNewPath() {
        StopAndForgetPath();
        PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
    }

    public void StopAndReset() {
        StopAndForgetPath();
        transform.position = startPos; // Reset position to start position
    }

    void StopAndForgetPath() {
        StopCoroutine("FollowPath"); // Stop current path-following coroutine
        path = null; // Clear the path
        targetIndex = 0; // Reset target index
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if (pathSuccessful && newPath.Length > 0) {
            path = newPath;
            targetIndex = 0; 
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        if (path == null || path.Length == 0) yield break; // Ensure path is valid

        Vector3 currentWaypoint = path[0];

        while (true) {
            if (transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    Destroy(gameObject);
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos() {
        if (path != null && showPathGizmos) {
            Gizmos.color = new Color(0.75f, 0.75f, 0.75f, 0.3f); // Light transparent gray

            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.DrawCube(path[i], Vector3.one * 0.5f); 

                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
