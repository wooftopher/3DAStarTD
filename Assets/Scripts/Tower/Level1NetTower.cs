using UnityEngine;

public class Level1NetTower : BaseTower {
    [SerializeField] private string level1TowerName;
    [SerializeField] private float levelDamage;
    [SerializeField] private float levelRange;
    [SerializeField] private float levelShootCooldown;
    [SerializeField] private float netStunDuration; // Stun duration specific to this tower level
    [SerializeField] private GameObject netPrefab;
    [SerializeField] private int level1TowerPrice;

    protected override void Awake() {
        base.Awake();
        
        // Initialize the base tower with specific values for Level 1 Tower
        InitializeTower(levelDamage, levelRange, levelShootCooldown, netPrefab, level1TowerPrice); // No missile prefab for net tower
        TowerName = level1TowerName;
    }

    protected override void ShootAtTarget(Transform target) {
        // Instantiate a net missile instead of a regular missile
        GameObject netMissile = Instantiate(netPrefab, firePoint.position, netPrefab.transform.rotation);
        NetMissile missileComponent = netMissile.GetComponent<NetMissile>();

        if (missileComponent != null) {
            // Set the target, damage, and stun duration for the net missile
            missileComponent.SetTarget(target, levelDamage, netStunDuration); 
        }
    }
}
