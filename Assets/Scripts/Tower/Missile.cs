using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Transform target;
    public float speed = 10f;
    public float rotateSpeed = 10f;
    public float destroyDistance = 1f; // Distance threshold to consider reaching the target
    public float damage;

    void Update() {
        if (target != null) {
            // Calculate the direction vector towards the target
            Vector3 direction = (target.position - transform.position).normalized;

            // Calculate the rotation to face the target direction
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Preserve the initial X rotation and current Z rotation
            float initialXRotation = transform.rotation.eulerAngles.x;
            float currentZRotation = transform.rotation.eulerAngles.z;

            // Create a new rotation with preserved X and Z, and Y from lookRotation
            Quaternion targetRotation = Quaternion.Euler(initialXRotation, lookRotation.eulerAngles.y, currentZRotation);

            // Smoothly interpolate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            // Move towards the target using MoveTowards
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Check if reached the target
            if (Vector3.Distance(transform.position, target.position) <= destroyDistance) {
                HandleTargetHit();
            }
        }
        else {
            // Destroy the missile if the target is null (despawned)
            Destroy(gameObject);
        }
    }


    void HandleTargetHit() {
        if (target != null) {
            // Apply damage or destroy the target
            // Destroy(target.gameObject); // Destroy the target game object
            target.gameObject.GetComponent<Unit>().TakeDamage(damage);
            Destroy(gameObject); // Destroy the missile
        }
        else {
            // Destroy the missile if the target is null (despawned)
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget) {
        target = newTarget;

        // Face the new target direction while keeping X rotation horizontal
        if (target != null) {
            // Calculate the direction vector towards the target
            Vector3 direction = (target.position - transform.position).normalized;

            // Calculate the rotation to face the target direction
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Preserve the initial X rotation
            float initialXRotation = transform.rotation.eulerAngles.x;

            // Apply the rotation while preserving the initial X rotation
            transform.rotation = Quaternion.Euler(initialXRotation, rotation.eulerAngles.y, 0);
        }
    }



    public void SetDamage(float amount){
        damage = amount;
    }
}
