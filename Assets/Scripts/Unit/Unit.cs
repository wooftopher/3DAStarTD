using System.Collections;
using System.Collections.Generic;  // Add this line to use List
using UnityEngine;

public class Unit : MonoBehaviour {
    public GameObject target;
    public float speed = 5;
    public float HP;
    private float currentHP;
    Vector3[] path;
    int targetIndex;
    Vector3 startPos;
    public bool showPathGizmos;
    
    private List<Coroutine> activeStuns = new List<Coroutine>(); // To track active stuns
    [SerializeField] HealthBar healthBar;

    void Start(){
        healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar == null) {
            Debug.LogError("HealthBar component not found in children of the unit prefab!");
        }
    }

    void Awake() {
        currentHP = HP;
        healthBar.UpdateHealthBar(currentHP, HP);
        startPos = transform.position;
        RequestNewPath();
    }

    void Update() {
        if (currentHP <= 0){
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount){
        currentHP -= amount;
        healthBar.UpdateHealthBar(currentHP, HP);
    }

    // Method to stun the unit (freeze it for a while)
    public void Stun(float duration) {
        Coroutine stunCoroutine = StartCoroutine(StunCoroutine(duration));
        activeStuns.Add(stunCoroutine); // Add the new stun coroutine to the list
    }

    // Coroutine to handle the stun duration
    IEnumerator StunCoroutine(float duration) {
        // Stop movement
        yield return new WaitForSeconds(duration); // Wait for stun duration
        
        // Remove this coroutine from the active stuns list
        activeStuns.Remove(activeStuns[activeStuns.Count - 1]); // Remove the last added coroutine

        // Check if there are any active stuns left
        if (activeStuns.Count == 0) {
            RequestNewPath(); // Resume movement if no active stuns
        }
    }

    void RequestNewPath() {
        if (activeStuns.Count == 0) {  // Only request path if not stunned
            StopAndForgetPath();
            PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
        }
    }

    void StopAndForgetPath() {
        StopCoroutine("FollowPath");  // Stop current path-following coroutine
        path = null;  // Clear the path
        targetIndex = 0;  // Reset target index
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if (pathSuccessful && newPath.Length > 0 && activeStuns.Count == 0) {  // Only follow path if not stunned
            path = newPath;
            targetIndex = 0; 
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        if (path == null || path.Length == 0) yield break;  // Ensure path is valid

        Vector3 currentWaypoint = path[0];

        while (true) {
            if (activeStuns.Count > 0) {
                yield return null; // Wait for the next frame if stunned
                continue; // Go back to the start of the loop
            }

            // Only move in the x and z axes, keeping the y-coordinate constant
            Vector3 targetPosition = new Vector3(currentWaypoint.x, transform.position.y, currentWaypoint.z);
            
            // Calculate the direction and rotation towards the target position
            Vector3 direction = (targetPosition - transform.position).normalized;

            if (direction != Vector3.zero) { // Ensure direction is not zero to avoid errors
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
            }

            // Move towards the targetPosition
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Check if the unit has reached the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    Destroy(gameObject);
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            yield return null; // Wait for the next frame
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
