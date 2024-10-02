using UnityEngine;
using System.Collections;

public class Level1MeleeTower : BaseTower {
    [SerializeField] private string level1TowerName; // Name of the tower
    [SerializeField] private float levelDamage; // Damage for Level 1 Tower
    [SerializeField] private float levelRange;   // Range for Level 1 Tower
    [SerializeField] private float levelShootCooldown; // Cooldown for Level 1 Tower
    [SerializeField] private GameObject axePrefab; // Prefab for the axe
    [SerializeField] private int level1TowerPrice; // Price for Level 1 Tower

    protected override void Awake() {
        base.Awake(); // Call base Awake method
        
        // Initialize the base tower with specific values for Level 1 Tower
        InitializeTower(levelDamage, levelRange, levelShootCooldown, null, level1TowerPrice); // No missile prefab for melee tower
        TowerName = level1TowerName;
    }

    protected override void ShootAtTarget(Transform target) {
        // Start the melee attack
        SwingAxeAtTarget(target);
    }

    private void SwingAxeAtTarget(Transform target) {
        if (target == null) return;

        // Instantiate the axe in front of the tower
        Vector3 axePosition = firePoint.position + firePoint.forward; // Position the axe slightly in front of the tower
        GameObject axeObject = Instantiate(axePrefab, axePosition, Quaternion.identity);

        // Rotate the axe to simulate a swing
        StartCoroutine(AxeSwing(axeObject, target));
    }

    private IEnumerator AxeSwing(GameObject axeObject, Transform target) {
        if (axeObject == null) {
            yield break; // Ensure axe is valid
        }

        // Grab the Unit component from the target
        Unit targetUnit = target != null ? target.GetComponent<Unit>() : null;
        if (targetUnit == null) {
            Destroy(axeObject); // Destroy axeObject if target doesn't have a Unit component
            yield break; // Ensure target has a Unit component
        }

        // Rotate the axe to simulate the swing
        float swingDuration = 0.5f; // Duration of the swing
        float elapsedTime = 0f;

        // Set the axe's initial rotation
        Quaternion initialRotation = axeObject.transform.rotation;

        // Swing the axe
        while (elapsedTime < swingDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / swingDuration;

            // Check if the target is still valid during the swing
            if (target == null) {
                Destroy(axeObject); // Destroy the axe if the target no longer exists
                yield break; // Exit the coroutine
            }

            // Calculate the direction from the firePoint to the target
            Vector3 directionToTarget = (target.position - firePoint.position).normalized;

            // Calculate the rotation that aligns the axe with the direction to the target
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Set the X rotation for the swing
            float currentXRotation = Mathf.Lerp(-65f, 65f, t); // Interpolating between -65 and 65 degrees
            axeObject.transform.rotation = Quaternion.Euler(currentXRotation, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

            // Position the axe based on the firePoint
            axeObject.transform.position = firePoint.position + firePoint.forward * 0.5f; // Adjust distance as needed

            yield return null; // Wait for the next frame
        }

        // Final rotation and position adjustments
        if (target != null) {
            Vector3 finalDirectionToTarget = (target.position - firePoint.position).normalized;
            Quaternion finalTargetRotation = Quaternion.LookRotation(finalDirectionToTarget);
            axeObject.transform.rotation = Quaternion.Euler(65, finalTargetRotation.eulerAngles.y, finalTargetRotation.eulerAngles.z); // Finalize rotation
        }

        // Check if the target is still valid after the swing duration
        if (target != null && targetUnit != null) {
            // Apply damage to the target's currentHP regardless of its current position
            targetUnit.TakeDamage(levelDamage); // Deal damage
            Debug.Log($"Dealt {levelDamage} damage to {target.name}");
        }

        // Destroy the axe object after the swing
        if (axeObject != null) {
            Destroy(axeObject);
        }
    }

    public int GetPrice() {
        return level1TowerPrice; // Method to get the price of the tower
    }
}
