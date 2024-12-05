using UnityEngine;

public class BaseTower : MonoBehaviour, ISelectable {

    private WallPlacement builderManager;
    private InfoUI infoUI;
    private Renderer towerRenderer;
    private TowerDataSO towerData;
    protected GameObject missilePrefab;
    protected Transform firePoint;
    protected Transform currentTarget;
    private float shootTimer;
    public Color originalColor;
    

    protected virtual void Awake() {
        builderManager = FindObjectOfType<WallPlacement>();//<-- to fix
        if (builderManager == null) {
            Debug.LogError("No WallPlacement found in the scene!");
        }
        infoUI = FindObjectOfType<InfoUI>();  // This will find the first InfoUI component in the scene
        if (infoUI == null) {
            Debug.LogError("No InfoUI found in the scene!");
        }
        firePoint = transform;

        towerRenderer = GetComponent<Renderer>();
        if (towerRenderer != null) {
            originalColor = towerRenderer.material.color;
        } else {
            Debug.LogError("Renderer not found on the wall!");
        }
    }

    protected void InitializeTower(TowerDataSO towerData, GameObject missilePrefab) {
        this.towerData = towerData;
        this.missilePrefab = missilePrefab;
    }

    protected virtual void Update() {
        // Reduce shoot timer
        shootTimer -= Time.deltaTime;

        // Find and shoot at enemy targets if cooldown allows
        if (shootTimer <= 0f) {
            FindAndShootTarget();                            // Attempt to find and shoot at a target
            shootTimer = towerData.shootCooldown;                      // Reset cooldown timer after shooting
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
        return Vector3.Distance(transform.position, target.position) <= towerData.range; // Check if target is within range
    }

    protected Transform FindNearestTarget() {
        // Find all colliders within range
        Collider[] colliders = Physics.OverlapSphere(transform.position, towerData.range);
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
            missileComponent.SetDamage(towerData.damage);               // Set the damage for the missile
        }
    }

    protected virtual void OnDrawGizmosSelected() {
        // Visualize the tower's range in the Scene view
        Gizmos.color = Color.red;                           // Set color for the range visualization
        Gizmos.DrawWireSphere(transform.position, towerData.range);   // Draw a wire sphere to indicate range
    }

    public TowerDataSO GetTowerData() {
        return towerData;
    }

    public void ResetColor() {
        if (towerRenderer != null) {
            towerRenderer.material.color = originalColor;
        }
    }

    public void Select() {
        ISelectable lastSelected = builderManager.GetLastSelectedSelectable();
        if (lastSelected != null) {
            lastSelected.ResetColor();  // Reset color on the last selected object
        }
        builderManager.SetLastSelectedSelectable(this);
    
        if (infoUI == null) {
            Debug.LogError("InfoUI is not assigned!");
            return;
        }
        infoUI.ShowTowerPanel();
        infoUI.UpdateTowerInfo(GetTowerData());

        if (towerRenderer != null) {
            towerRenderer.material.color = new Color(0.6f, 1f, 0.6f, 1f); // Moderate light green filter
        } else {
            Debug.LogError("Renderer not found on the selected tower!");
        }
    }
}
