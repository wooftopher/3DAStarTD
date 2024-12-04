using UnityEngine;

public class RangedTower : BaseTower {
    [SerializeField] private GameObject levelMissilePrefab;
    [SerializeField] public TowerDataSO rangedTowerData;

    protected override void Awake() {
        base.Awake();
        
        InitializeTower(rangedTowerData, levelMissilePrefab);
    }

    protected override void ShootAtTarget(Transform target) {
        base.ShootAtTarget(target);
    }

    public int GetPrice() {
        return rangedTowerData.price;
    }
}
