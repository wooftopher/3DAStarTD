using System.Collections;
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

    [SerializeField] HealthBar healthBar;

    void Start(){
        healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar == null) {
            Debug.LogError("HealthBar component not found in children of the unit prefab!");
        }
    }

    void Awake() {
        // healthBar = GetComponent<HealthBar>();
        currentHP = HP;
        healthBar.UpdateHealthBar(currentHP, HP);
        startPos = transform.position;
        RequestNewPath();
    }

    void Update() {
        // if (Input.GetKeyDown(KeyCode.R)) {
        //     StopAndReset();
        // }
        if (currentHP <= 0){
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount){
        currentHP -= amount;
        healthBar.UpdateHealthBar(currentHP, HP);
        // Debug.Log("remaining HP" + currentHP);
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
            // Only move in the x and z axes, keeping the y-coordinate constant
            Vector3 targetPosition = new Vector3(currentWaypoint.x, transform.position.y, currentWaypoint.z);
            
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);

            if (transform.position == targetPosition) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    Destroy(gameObject);
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            // Move towards the targetPosition
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }


    public void OnDrawGizmos() {
        // Gizmos.color = Color.red;
        // Vector3 healthBarLenght = transform.position;
        // healthBarLenght.x += currentHP/100;
        // Gizmos.DrawLine(transform.position, healthBarLenght);
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
