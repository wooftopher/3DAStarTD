using UnityEngine;

public class Level1Tower : BaseTower {
    [SerializeField] private string level1TowerName = "Level 1 Tower"; // Name of the tower
    [SerializeField] private float levelDamage = 10f; // Damage for Level 1 Tower
    [SerializeField] private float levelRange = 8f;   // Range for Level 1 Tower
    [SerializeField] private float levelShootCooldown = 1.5f; // Cooldown for Level 1 Tower
    [SerializeField] private GameObject levelMissilePrefab; // Prefab for the missile
    [SerializeField] private int level1TowerPrice = 100; // Price for Level 1 Tower

    protected override void Start() {
        base.Start(); // Call base Start method
        
        // Initialize the base tower with specific values for Level 1 Tower
        InitializeTower(levelDamage, levelRange, levelShootCooldown, levelMissilePrefab, level1TowerPrice);
        TowerName = level1TowerName;
        Debug.Log($"Level1Tower initialized with name: {TowerName}, Damage: {Damage}, Range: {Range}, ShootCooldown: {ShootCooldown}, Price: {level1TowerPrice}");
    }

    protected override void ShootAtTarget(Transform target) {
        base.ShootAtTarget(target); // Call the base shooting logic
        // Any additional behavior for Level 1 Tower shooting can be added here
    }

    public int GetPrice() {
        return level1TowerPrice; // Method to get the price of the tower
    }
}
