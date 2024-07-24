using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject missilePrefab;
    public float shootCooldown = 1f; // Cooldown between shots
    public float range = 10f; // Range to detect enemy units

    private Transform firePoint; // Point where the missile spawns
    private Transform currentTarget; // Current enemy target
    private float shootTimer; // Timer to track shooting cooldown
    [SerializeField]
    private float damage;

    void Start() {
        firePoint = transform; // Set firePoint to be at the center of the tower's position
        shootTimer = shootCooldown; // Start with full cooldown time
    }

    void Update() {
        // Reduce shoot timer
        shootTimer -= Time.deltaTime;

        // Find and shoot at enemy targets if cooldown allows
        if (shootTimer <= 0f) {
            FindAndShootTarget();
            shootTimer = shootCooldown; // Reset cooldown timer after shooting
        }

        // Always face the current target if there is one
        if (currentTarget != null) {
            // Rotate tower towards the target
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
        }

    }

    void FindAndShootTarget() {
        if (currentTarget == null || !IsTargetInRange(currentTarget)) {
            currentTarget = FindNearestTarget();
        }

        if (currentTarget != null) {
            // Shoot at the target
            ShootAtTarget(currentTarget);
        }
    }

    bool IsTargetInRange(Transform target) {
        return Vector3.Distance(transform.position, target.position) <= range;
    }

    Transform FindNearestTarget() {

        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        Transform nearestTarget = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Unit")) {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < minDistance) {
                    minDistance = distance;
                    nearestTarget = collider.transform;
                }
            }
        }

        return nearestTarget;
    }

    void ShootAtTarget(Transform target) {
        GameObject missile = Instantiate(missilePrefab, firePoint.position, missilePrefab.transform.rotation);
        Debug.Log(missilePrefab.transform.rotation);
        Missile missileComponent = missile.GetComponent<Missile>();
        
        if (missileComponent != null) {
            missileComponent.SetTarget(target);
            missileComponent.SetDamage(damage);
        }

        // Debug.Log("Tower: Shooting at target!");
    }


    void OnDrawGizmosSelected() {
        // Visualize the tower's range in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
