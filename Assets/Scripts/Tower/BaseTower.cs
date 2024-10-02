using UnityEngine;

public class BaseTower : MonoBehaviour {
    // Properties for tower characteristics
    public virtual string TowerName { get; protected set; } // Name of the tower
    protected GameObject missilePrefab;                      // Prefab for the missile
    protected float shootCooldown;                           // Cooldown between shots
    protected float range;                                   // Range to detect enemy units
    protected float damage;                                  // Damage dealt by the tower
    protected int price;                                   // Price of the tower
    protected Transform firePoint;                             // Point where the missile spawns
    protected Transform currentTarget;                       // Current enemy target
    private float shootTimer;                                // Timer to track shooting cooldown
    public Color originalColor;

    // Public getters for tower attributes
    public float Damage => damage;                           // Getter for damage
    public float Range => range;                             // Getter for range
    public float ShootCooldown => shootCooldown;            // Getter for shoot cooldown
    public int Price => price;                             // Getter for tower price

    protected virtual void Awake() {
        firePoint = transform;                               // Set firePoint to be at the center of the tower's position
        shootTimer = shootCooldown;                          // Start with full cooldown time

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) {
            originalColor = renderer.material.color;
        } else {
            Debug.LogError("Renderer not found on the wall!");
        }
    }

    // Method to initialize tower's stats
    protected void InitializeTower(float damage, float range, float shootCooldown, GameObject missilePrefab, int price) {
        this.damage = damage;                                // Set tower damage
        this.range = range;                                  // Set tower range
        this.shootCooldown = shootCooldown;                  // Set tower cooldown
        this.missilePrefab = missilePrefab;                  // Set missile prefab
        this.price = price;                                  // Set tower price
        Debug.Log($"baseTower Initialized: Damage = {this.damage}, Range = {this.range}, ShootCooldown = {this.shootCooldown}, Price = {this.price}");
    }

    protected virtual void Update() {
        // Reduce shoot timer
        shootTimer -= Time.deltaTime;

        // Find and shoot at enemy targets if cooldown allows
        if (shootTimer <= 0f) {
            FindAndShootTarget();                            // Attempt to find and shoot at a target
            shootTimer = shootCooldown;                      // Reset cooldown timer after shooting
        }

        // Always face the current target if there is one
        if (currentTarget != null) {
            // Rotate tower towards the target
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f); // Rotate only on the Y-axis
        }
    }

    protected void FindAndShootTarget() {
        // Check if the current target is valid or in range
        if (currentTarget == null || !IsTargetInRange(currentTarget)) {
            currentTarget = FindNearestTarget();             // Find the nearest target
        }

        // If a target is found, shoot at it
        if (currentTarget != null) {
            ShootAtTarget(currentTarget);
        }
    }

    protected bool IsTargetInRange(Transform target) {
        return Vector3.Distance(transform.position, target.position) <= range; // Check if target is within range
    }

    protected Transform FindNearestTarget() {
        // Find all colliders within range
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        Transform nearestTarget = null;
        float minDistance = Mathf.Infinity;

        // Iterate through colliders to find the nearest valid target
        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Unit")) {               // Ensure the collider is a target
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < minDistance) {
                    minDistance = distance;                    // Update minimum distance
                    nearestTarget = collider.transform;        // Set new nearest target
                }
            }
        }

        return nearestTarget;                                 // Return the nearest target found
    }

    protected virtual void ShootAtTarget(Transform target) {
        // Instantiate a missile at the fire point
        GameObject missile = Instantiate(missilePrefab, firePoint.position, missilePrefab.transform.rotation);
        Missile missileComponent = missile.GetComponent<Missile>();

        // Set target and damage for the missile
        if (missileComponent != null) {
            missileComponent.SetTarget(target);               // Set the target for the missile
            missileComponent.SetDamage(damage);               // Set the damage for the missile
        }
    }

    protected virtual void OnDrawGizmosSelected() {
        // Visualize the tower's range in the Scene view
        Gizmos.color = Color.red;                           // Set color for the range visualization
        Gizmos.DrawWireSphere(transform.position, range);   // Draw a wire sphere to indicate range
    }
}
