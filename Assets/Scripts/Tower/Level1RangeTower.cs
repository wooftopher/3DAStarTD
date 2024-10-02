using UnityEngine;

public class Level1Tower : BaseTower {
    [SerializeField] private string level1TowerName = "Range Tower lvl 1"; 
    [SerializeField] private float levelDamage = 10f;
    [SerializeField] private float levelRange = 8f;
    [SerializeField] private float levelShootCooldown = 1.5f;
    [SerializeField] private GameObject levelMissilePrefab;
    [SerializeField] private int level1TowerPrice = 100;

    protected override void Awake() {
        base.Awake(); // Call base Start method
        
        // Initialize the base tower with specific values for Level 1 Tower
        InitializeTower(levelDamage, levelRange, levelShootCooldown, levelMissilePrefab, level1TowerPrice);
        TowerName = level1TowerName;
        // Debug.Log($"Level1Tower initialized with name: {TowerName}, Damage: {Damage}, Range: {Range}, ShootCooldown: {ShootCooldown}, Price: {level1TowerPrice}");
    }

    protected override void ShootAtTarget(Transform target) {
        base.ShootAtTarget(target); // Call the base shooting logic
        // Any additional behavior for Level 1 Tower shooting can be added here
    }

    public int GetPrice() {
        return level1TowerPrice; // Method to get the price of the tower
    }
}
