using UnityEngine;

public class NetMissile : MonoBehaviour
{
    private Transform target;
    public float speed = 10f;
    private float destroyDistance = 1f; // Distance to stick to the target
    private float disableDuration; // How long the target is disabled (set by the tower)
    private float damageAmount; // Amount of damage (set by the tower)
    private bool targetHit = false; // Track if the net is attached

    public float spinSpeed = 360f; // Spin speed while moving

    // Desired final rotation when hitting the target
    private Vector3 finalRotation = new Vector3(6f, -342f, -4.5f); 

    void Update() {
        if (target != null && !targetHit) {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            // Spin the missile around the Y-axis
            SpinFrisbeeStyle();

            // Check if reached the target
            if (Vector3.Distance(transform.position, target.position) <= destroyDistance) {
                StickToTarget();
            }
        } else if (target == null)
            Destroy(gameObject);
        else if (targetHit) {
            // Stay attached to the target and set final rotation
            transform.position = target.position;
            transform.rotation = Quaternion.Euler(finalRotation); // Set the specific final rotation
        }
    }

    private void SpinFrisbeeStyle() {
        // Spin around the Y-axis
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }

    void StickToTarget() {
        if (target != null) {
            targetHit = true;

            // Stop the target's movement and apply stun and damage
            Unit unit = target.gameObject.GetComponent<Unit>();
            if (unit != null) {
                // Stun the target for the disableDuration
                unit.Stun(disableDuration);

                // Apply damage to the target
                unit.TakeDamage(damageAmount);
            }

            // Destroy the missile after the disableDuration
            Destroy(gameObject, disableDuration);
        }
        else {
            Destroy(gameObject); // Destroy if the target is null
        }
    }

    void LockRotationAfterHit() {
        // Set the missile's rotation to the desired final rotation
        transform.rotation = Quaternion.Euler(finalRotation); // Set the rotation to the specified angles
    }

    // Method to set target, damage amount, and stun duration, called by the tower
    public void SetTarget(Transform newTarget, float damage, float stunDuration) {
        target = newTarget;
        damageAmount = damage;
        disableDuration = stunDuration;
    }
}
