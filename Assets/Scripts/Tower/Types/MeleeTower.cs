using UnityEngine;
using System.Collections;

public class MeleeTower : BaseTower {
    [SerializeField] private GameObject axePrefab;

    [SerializeField] private TowerDataSO towerData;

    protected override void Awake() {
        base.Awake();

        InitializeTower(towerData, axePrefab);
        TowerName = towerData.name;
    }

    protected override void ShootAtTarget(Transform target) {
        SwingAxeAtTarget(target);
    }

    private void SwingAxeAtTarget(Transform target) {
        if (target == null) return;

        Vector3 axePosition = firePoint.position + firePoint.forward;
        GameObject axeObject = Instantiate(axePrefab, axePosition, Quaternion.identity);

        StartCoroutine(AxeSwing(axeObject, target));
    }

    private IEnumerator AxeSwing(GameObject axeObject, Transform target) {
        if (axeObject == null) {
            yield break; // Ensure axe is valid
        }

        Unit targetUnit = target != null ? target.GetComponent<Unit>() : null;
        if (targetUnit == null) {
            Destroy(axeObject);
            yield break; // Ensure target has a Unit component
        }

        float swingDuration = 0.5f;
        float elapsedTime = 0f;

        // Swing the axe
        while (elapsedTime < swingDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / swingDuration;

            if (target == null) {
                Destroy(axeObject);
                yield break;
            }

            // Calculate the direction from the firePoint to the target
            Vector3 directionToTarget = (target.position - firePoint.position).normalized;

            // Calculate the rotation that aligns the axe with the direction to the target
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Set the X rotation for the swing
            float currentXRotation = Mathf.Lerp(-65f, 65f, t);
            axeObject.transform.rotation = Quaternion.Euler(currentXRotation, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

            axeObject.transform.position = firePoint.position + firePoint.forward * 0.5f;

            yield return null;
        }

        // Final rotation and position adjustments
        if (target != null) {
            Vector3 finalDirectionToTarget = (target.position - firePoint.position).normalized;
            Quaternion finalTargetRotation = Quaternion.LookRotation(finalDirectionToTarget);
            axeObject.transform.rotation = Quaternion.Euler(65, finalTargetRotation.eulerAngles.y, finalTargetRotation.eulerAngles.z);
        }

        if (target != null && targetUnit != null) {
            targetUnit.TakeDamage(towerData.Damage);
            Debug.Log($"Dealt {towerData.Damage} damage to {target.name}");
        }

        if (axeObject != null) {
            Destroy(axeObject);
        }
    }

    public int GetPrice() {
        return towerData.price;
    }
}
